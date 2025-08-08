using LRA.Gateways.AdminGateway.Inetrfaces;
using LRA.Shared.Abstractions.Models.Auth;
using LRA.Shared.Utils.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LRA.Gateways.AdminGateway.Controllers;

[Route("api/auth")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAdminAuthService _authService;

    public AuthController(IAdminAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> LoginUser([FromBody] LoginModel model, CancellationToken cancellationToken)
    {
        var response = await _authService.LoginUserAsync(model, cancellationToken);
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

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
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

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
    [HttpGet("new-email-code")]
    public async Task<IActionResult> SendChangeUserEmailCode([FromQuery] string newEmail, CancellationToken cancellationToken)
    {
        await _authService.SendChangeUserEmailCodeAsync(newEmail, cancellationToken);
        return Ok();
    }

    [Authorize(Policy = ServerPolicy.RequireAdmin)]
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
}
