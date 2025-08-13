using LRA.Account.Application.Interfaces;
using LRA.Account.Domain.Entities;
using LRA.Account.Presentation.Interfaces;
using LRA.Shared.Abstractions.Dtos.Verification;
using LRA.Shared.Abstractions.Models;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace LRA.Account.Presentation.Controllers;

[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IAccountService _accountService;
    private readonly IKeycloakService _keycloakService;

    public AuthController(IAuthService authService, IKeycloakService keycloakService, IAccountService accountService)
    {
        _authService = authService;
        _keycloakService = keycloakService;
        _accountService = accountService;
    }

    [Authorize]
    [HttpGet("keycloak/config")]
    public async Task<IActionResult> GetRealmConfig(CancellationToken cancellationToken)
    {
        var response = await _keycloakService.GetKeycloakRealmConfigAsync(cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpPost("account/registration")]
    public async Task<IActionResult> CreateUser([FromBody] RegistrationModel registrationModel, CancellationToken cancellationToken)
    {
        var keycloakId = await _keycloakService.CreateKeycloakUserAsync(registrationModel, cancellationToken);

        await _accountService.CreateAccountAsync(registrationModel.Email, Guid.Parse(keycloakId), cancellationToken);

        await _authService.SendEmailVerificationTokenAsync(registrationModel.Email, 60, cancellationToken);

        return Ok(keycloakId);
    }

    [Authorize]
    [HttpPost("account-with-temp-pass/registration")]
    public async Task<IActionResult> CreateUserWithTempPassword([FromQuery] string email, string? role, CancellationToken cancellationToken)
    {
        var tempPassword = StringGenerator.GeneratePassword();

        var registrationModel = new RegistrationModel
        {
            Email = email,
            Password = tempPassword,
            ConfirmPassword = tempPassword,
        };
        var keycloakIdStr = await _keycloakService.CreateKeycloakUserAsync(registrationModel, cancellationToken);

        var keycloakId = Guid.Parse(keycloakIdStr);
        await _keycloakService.EnableKeycloakUserAsync(keycloakId, cancellationToken);

        await _accountService.CreateAccountWithTempPasswordAsync(registrationModel.Email, keycloakId, cancellationToken);
        if (role != null)
        {
            await _accountService.AddRoleAsync(role, keycloakId, cancellationToken);
        }

        var passwordModel = new TempPasswordModel() 
        { 
            TempPassword = tempPassword 
        };    
        return Ok(passwordModel);
    }

    [Authorize]
    [HttpPost("keycloak/login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        if (!await _accountService.CanUserLoginAsync(model.Email, cancellationToken))
        {
            throw new Exception("Cant login exception");
        }

        var response = await _keycloakService.LoginKeycloakUserAsync(model, cancellationToken);
        await _accountService.MakeTempPasswordUsedAsync(model.Email, cancellationToken);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(response.AccessToken);
        Guid.TryParse(jwtToken.Claims.First(c => c.Type == "sub").Value, out var keycloakId);

        if (await _accountService.IsTwoFaEnabledAsync(keycloakId, cancellationToken))
        {
            await _authService.SendTwoFactorTokenAsync(model.Email, 5, cancellationToken);
            var requiredVerificationDto = new VerificationDto()
            {
                IsVerificationRequired = true,
            };
            return Ok(requiredVerificationDto);
        }

        var verificationDto = new VerificationDto()
        {
            IsVerificationRequired = false,
            Tokens = response
        };
        return Ok(verificationDto);
    }

    [Authorize]
    [HttpPost("twofactor/verification")]
    public async Task<IActionResult> VerifyUser([FromBody] TwoFactorLoginModel twoFaModel, CancellationToken cancellationToken)
    {
        var response = await _authService.VerifyTwoFaTokenAsync(twoFaModel.TwoFactorToken, twoFaModel.Email, cancellationToken);

        if (response)
        {
            var tokens = await _keycloakService.LoginKeycloakUserAsync(twoFaModel, cancellationToken);
            var verificationDto = new VerificationDto()
            {
                IsVerificationRequired = false,
                Tokens = tokens,
            };
            return Ok(verificationDto);
        }
        else
        {
            return BadRequest("Invalid twofactor token");
        }
    }

    [Authorize]
    [HttpPut("email/verification")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token, [FromQuery] string email, CancellationToken cancellationToken)
    {
        var result = await _authService.VerifyEmailAsync(token, email, cancellationToken);
        if (result)
        {
            var account = await _accountService.GetAccountByEmailAsync(email, cancellationToken);
            await _keycloakService.EnableKeycloakUserAsync(account.KeycloakId, cancellationToken);
            await _accountService.VerifyEmailAsync(email, cancellationToken);
            return Ok();
        }
        else
            return BadRequest();
    }

    [Authorize]
    [HttpPost("keycloak/refreshment")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var response = await _keycloakService.RefreshTokenAsync(refreshTokenModel, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpDelete("keycloak/logout")]
    public async Task<IActionResult> LogoutSession([FromBody] RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        await _keycloakService.LogoutUserSessionAsync(refreshTokenModel, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("new-password/{keycloakId}")]
    public async Task<IActionResult> ChangeUserPassword([FromQuery] string newPassword, [FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        await _keycloakService.ChangeUserPasswordAsync(newPassword, keycloakId, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("reset-code")]
    public async Task<IActionResult> SendResetUserPasswordCode([FromQuery] string email, CancellationToken cancellationToken)
    {
        await _authService.SendPasswordResetTokenAsync(email, 5, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("reset-code/verification")]
    public async Task<IActionResult> ResetUserPassword([FromBody] ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        var verified = await _authService.VerifyPasswordResetTokenAsync(resetPasswordModel.ResetPasswordToken, resetPasswordModel.Email, cancellationToken);
        if (verified)
        {
            var response = await _accountService.GetAccountByEmailAsync(resetPasswordModel.Email, cancellationToken);
            await _keycloakService.ChangeUserPasswordAsync(resetPasswordModel.NewPassword, response.KeycloakId, cancellationToken);
            return Ok("Password changed");
        }
        else
        {
            return BadRequest("Password reset failed");
        }
    }

    [Authorize]
    [HttpGet("admin-reset-code")]
    public async Task<IActionResult> SendAdminResetUserPasswordCode([FromQuery] string email, CancellationToken cancellationToken)
    {
        await _authService.SendAdminPasswordResetTokenAsync(email, 2880, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("admin-reset-code/verification")]
    public async Task<IActionResult> AdminResetUserPassword([FromBody] ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        var verified = await _authService.VerifyAdminPasswordResetTokenAsync(resetPasswordModel.ResetPasswordToken, resetPasswordModel.Email, cancellationToken);
        if (verified)
        {
            var response = await _accountService.GetAccountByEmailAsync(resetPasswordModel.Email, cancellationToken);
            await _keycloakService.ChangeUserPasswordAsync(resetPasswordModel.NewPassword, response.KeycloakId, cancellationToken);
            return Ok("Password changed");
        }
        else
        {
            return BadRequest("Password reset failed");
        }
    }

    [Authorize]
    [HttpGet("new-email-code")]
    public async Task<IActionResult> SendChangeUserEmailCode([FromQuery] string newEmail, CancellationToken cancellationToken)
    {
        await _authService.SendEmailChangingTokenAsync(newEmail, 5, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("new-email-code/verification/{keycloakId}")]
    public async Task<IActionResult> ChangeUserEmail([FromBody] EmailChangeModel newEmailModel, [FromRoute] Guid keycloakId, CancellationToken cancellationToken)
    {
        var verified = await _authService.VerifyEmailChangeAsync(newEmailModel.EmailChangeToken, newEmailModel.NewEmail, cancellationToken);
        if (verified)
        {
            
            await _keycloakService.ChangeUserEmailAsync(keycloakId, newEmailModel.NewEmail, cancellationToken);

            await _accountService.UpdateEmailAsync(newEmailModel.NewEmail, keycloakId, cancellationToken);
            return Ok("Email changed succsessfully");
        }
        else
        {
            return BadRequest("Email changing failed");
        }
    }

    [Authorize]
    [HttpGet("keycloak/google-login")]
    public async Task<IActionResult> LoginUserByGoogleCode([FromQuery] string code, CancellationToken cancellationToken)
    {
        var response = await _keycloakService.ExchangeGoogleCodeForTokenAsync(code, cancellationToken);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(response.AccessToken);
        Guid.TryParse(jwtToken.Claims.First(c => c.Type == "sub").Value, out var keycloakId);
        var email = jwtToken.Claims.First(c => c.Type == "email").Value;

        await _accountService.CreateAccountAsync(email, keycloakId, cancellationToken);
        return Ok(response);
    }

    [Authorize]
    [HttpGet("google-url")]
    public IActionResult GetGoogleLoginUrl()
    {
        var url = _keycloakService.GetGoogleLoginUrl();
        return Ok(url);
    }

    [Authorize]
    [HttpPut("temp-password/{id}")]
    public async Task<IActionResult> GenerateTempPassword([FromRoute] Guid id, [FromQuery] Guid adminKeycloakId, CancellationToken cancellationToken)
    {
        var passModel = await _accountService.CreateTempPassForAccountAsync(id, adminKeycloakId, cancellationToken);

        var account = await _accountService.GetAccountByIdAsync(id, cancellationToken);
        await _keycloakService.ChangeUserPasswordAsync(passModel.TempPassword, account.KeycloakId, cancellationToken);

        return Ok(passModel);
    }
}
