using Asp.Versioning;
using LRA.Gateways.Client.Interfaces;
using LRA.Shared.Abstractions.Models.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LRA.Gateways.Client.Controllers;

[Route("api/v{version:apiVersion}/auth")]
[ApiController]
[ApiVersion("1.0")]
public class AuthController : ControllerBase
{
    private readonly IClientAuthService _authService;

    public AuthController(IClientAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("config")]
    public async Task<IActionResult> GetAuthConfig(CancellationToken cancellationToken)
    {
        var response = await _authService.GetAuthServiceConfigAsync(cancellationToken);
        return Ok(response);
    }

    [HttpPost("registration")]
    public async Task<IActionResult> CreateUser([FromBody] RegistrationModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.CreateUserAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpPut("email/verification")]
    public async Task<IActionResult> VerifyEmail([FromQuery] string token, string email, CancellationToken cancellationToken)
    {
        await _authService.VerifyEmailAsync(token, email, cancellationToken);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginUserAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpGet("google-login")]
    public async Task<IActionResult> LoginUserByGoogleCode([FromQuery] string code, CancellationToken cancellationToken)
    {
        var response = await _authService.GetTokenByGoogleCode(code, cancellationToken);
        return Ok(response);
    }

    [HttpPost("twofactor/verification")]
    public async Task<IActionResult> VerifyUser([FromBody] TwoFactorLoginModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.TwoFactorVerifyAsync(model, cancellationToken);
        return Ok(response);
    }

    [HttpPost("refreshment")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        var response = await _authService.RefreshTokenAsync(refreshTokenModel, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("logout")]
    public async Task<IActionResult> LogoutSession([FromBody] RefreshTokenModel refreshTokenModel, CancellationToken cancellationToken)
    {
        await _authService.LogoutUserSessionAsync(refreshTokenModel, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("new-password")]
    public async Task<IActionResult> ChangeUserPassword([FromBody] PasswordChangeModel newPassword, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }

        await _authService.ChangeUserPasswordAsync(newPassword.Password, userId, cancellationToken);
        return Ok();
    }

    [HttpGet("reset-code")]
    public async Task<IActionResult> SendResetPasswordCode([FromQuery] string email, CancellationToken cancellationToken)
    {
        await _authService.SendResetPasswordCodeAsync(email, cancellationToken);
        return Ok();
    }

    [HttpPut("reset-code/verification")]
    public async Task<IActionResult> ResetUserPassword([FromBody] ResetPasswordModel resetPasswordModel, CancellationToken cancellationToken)
    {
        await _authService.ResetUserPasswordAsync(resetPasswordModel, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpGet("new-email-code")]
    public async Task<IActionResult> SendChangeUserEmailCode([FromQuery] string newEmail, CancellationToken cancellationToken)
    {
        await _authService.SendChangeUserEmailCodeAsync(newEmail, cancellationToken);
        return Ok();
    }

    [Authorize]
    [HttpPut("new-email-code/verification")]
    public async Task<IActionResult> ChangeUserEmail([FromBody] EmailChangeModel newEmailModel, CancellationToken cancellationToken)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
        {
            return Unauthorized("Missing 'Identifier' in token");
        }
        await _authService.ChangeUserEmail(newEmailModel, userId, cancellationToken);
        return Ok();
    }

    [HttpGet("google-url")]
    public async Task<IActionResult> GetGoogleLoginUrl(CancellationToken cancellationToken)
    {
        var url = await _authService.GetGoogleLoginUrlAsync(cancellationToken);
        return Ok(url);
    }
}
