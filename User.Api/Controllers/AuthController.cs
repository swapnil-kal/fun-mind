using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Api.Dto;
using User.Api.Exceptions;
using User.Api.Services;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;
        private readonly IJwtAuthProvider _jwtAuthProvider;

        public AuthController(IAuthService authService, IJwtAuthProvider jwtAuthProvider)
        {
            _authService = authService;
            _jwtAuthProvider = jwtAuthProvider;
        }

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<RegisterResponse>> Register([FromBody] RegisterRequest request)
        {
            try
            {
                return await _authService.Register(request);
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnAuthenticateException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            try
            {
                return await _authService.Login(request);                
            }
            catch (NotFoundException ex)
            {
                return NotFound(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnAuthenticateException ex)
            {
                return Unauthorized(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> ExternalLogin([FromBody] ExternalAuthDto request)
        {
            try
            {
                
                // return await _authService.Authenticate(request);
                return null;
            }
            catch (NotFoundException ex)
            {
                return NotFound(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnAuthenticateException ex)
            {
                return Unauthorized(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("guest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginResponse>> GuestLogin([FromBody] LoginRequest request)
        {
            try
            {
                return await _authService.GuestLogin(request);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnAuthenticateException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpPost("changePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeneralResponse>> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            try
            {
                var result = await _authService.ChangePassword(request);
                if (result)
                {
                    return Ok(new GeneralResponse
                    {
                        Success = true,
                        Message = "Password changed successfully."
                    });
                }
                else
                {
                    return Ok(new GeneralResponse
                    {
                        Success = false,
                        Message = "Failed to changed password Successfully."
                    });
                }

            }
            catch (NotFoundException ex)
            {
                return NotFound(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
            catch (UnAuthenticateException ex)
            {
                return Unauthorized(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("resetPassword/sendOTP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeneralResponse>> SendResetPasswordOTP([FromBody] ResetPasswordOTPRequest request)
        {
            try
            {
                var result = await _authService.SendResetPasswordOTP(request.Email);
                if (result)
                {
                    return Ok(new GeneralResponse
                    {
                        Success = true,
                        Message = "OTP send successfully."
                    });
                }
                else
                {
                    return Ok(new GeneralResponse
                    {
                        Success = false,
                        Message = "Failed to send OTP. Please try again."
                    });
                }
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("resetPassword/verifyOTP")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeneralResponse>> VerifyResetPasswordOTP([FromBody] VerifyResetPasswordOTPRequest request)
        {
            try
            {
                var result = await _authService.VerifyResetPasswordOTP(request.Email, request.Otp);
                if (result)
                {
                    return Ok(new GeneralResponse
                    {
                        Success = true,
                        Message = "OTP verified successfully."
                    });
                }
                else
                {
                    return Ok(new GeneralResponse
                    {
                        Success = false,
                        Message = "Failed to verify OTP. Please try again."
                    });
                }
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new GeneralResponse
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("resetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GeneralResponse>> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            try
            {
                var result = await _authService.ResetPassword(request);
                if (result)
                {
                    return Ok(new GeneralResponse
                    {
                        Success = true,
                        Message = "Password changed successfully."
                    });
                }
                else
                {
                    return Ok(new GeneralResponse
                    {
                        Success = false,
                        Message = "Failed to changed password. Please try again."
                    });
                }
            }
            catch (BadRequestException ex)
            {
                return BadRequest(new GeneralResponse { 
                
                    Success = false,
                    Message = ex.Message
                });
            }            
        }
    }
}
