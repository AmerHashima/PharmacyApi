using Pharmacy.Application.Commands.Auth;
using Pharmacy.Application.DTOs.Auth;
using Pharmacy.Api.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FluentValidation;
using System.IdentityModel.Tokens.Jwt;

namespace Pharmacy.Api.Controllers;

[Route("api/[controller]")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// User login
    /// </summary>
    /// <param name="loginDto">Login credentials</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return ErrorResponse<AuthResponseDto>("Validation failed", 400, errors);
        }

        try
        {
            var command = new LoginCommand { LoginDto = loginDto };
            var result = await _mediator.Send(command);
            return SuccessResponse(result, "Login successful");
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            return ErrorResponse<AuthResponseDto>("Validation failed", 400, errors);
        }
        catch (UnauthorizedAccessException ex)
        {
            return ErrorResponse<AuthResponseDto>(ex.Message, 401);
        }
        catch (Exception ex)
        {
            return ErrorResponse<AuthResponseDto>("An error occurred during login", 500);
        }
    }

    /// <summary>
    /// User registration
    /// </summary>
    /// <param name="registerDto">Registration data</param>
    /// <returns>Authentication response with JWT token</returns>
    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return ErrorResponse<AuthResponseDto>("Validation failed", 400, errors);
        }

        try
        {
            // Normalize gender input
            //if (registerDto.Gender.HasValue)
            //{
            //    registerDto.Gender = char.ToUpperInvariant(registerDto.Gender.Value);
            //    if (registerDto.Gender != 'M' && registerDto.Gender != 'F')
            //    {
            //        return ErrorResponse<AuthResponseDto>("Gender must be 'M' for Male or 'F' for Female", 400);
            //    }
            //}

            var command = new RegisterCommand { RegisterDto = registerDto };
            var result = await _mediator.Send(command);
            return SuccessResponse(result, "Registration successful");
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            return ErrorResponse<AuthResponseDto>("Validation failed", 400, errors);
        }
        catch (InvalidOperationException ex)
        {
            return ErrorResponse<AuthResponseDto>(ex.Message, 400);
        }
        catch (Exception ex)
        {
            return ErrorResponse<AuthResponseDto>("An error occurred during registration", 500);
        }
    }

    /// <summary>
    /// User logout
    /// </summary>
    /// <returns>Success response</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResponse>> Logout()
    {
        try
        {
            var token = GetTokenFromRequest();
            if (string.IsNullOrEmpty(token))
            {
                return ErrorResponse("Token not found", 400);
            }

            var command = new LogoutCommand { Token = token };
            var result = await _mediator.Send(command);

            if (result)
            {
                return SuccessResponse("Logout successful");
            }
            else
            {
                return ErrorResponse("Logout failed", 400);
            }
        }
        catch (Exception ex)
        {
            return ErrorResponse("An error occurred during logout", 500);
        }
    }

    /// <summary>
    /// Refresh JWT token
    /// </summary>
    /// <param name="refreshTokenDto">Refresh token data</param>
    /// <returns>New authentication response</returns>
    [HttpPost("refresh")]
    public async Task<ActionResult<ApiResponse<AuthResponseDto>>> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            return ErrorResponse<AuthResponseDto>("Validation failed", 400, errors);
        }

        try
        {
            var command = new RefreshTokenCommand { RefreshTokenDto = refreshTokenDto };
            var result = await _mediator.Send(command);
            return SuccessResponse(result, "Token refreshed successfully");
        }
        catch (ValidationException ex)
        {
            var errors = ex.Errors.Select(e => e.ErrorMessage).ToList();
            return ErrorResponse<AuthResponseDto>("Validation failed", 400, errors);
        }
        catch (UnauthorizedAccessException ex)
        {
            return ErrorResponse<AuthResponseDto>(ex.Message, 401);
        }
        catch (Exception ex)
        {
            return ErrorResponse<AuthResponseDto>("An error occurred during token refresh", 500);
        }
    }

    private string? GetTokenFromRequest()
    {
        var authHeader = Request.Headers["Authorization"].FirstOrDefault();
        if (authHeader != null && authHeader.StartsWith("Bearer "))
        {
            return authHeader.Substring("Bearer ".Length).Trim();
        }
        return null;
    }
}