using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Api.Constants;
using User.Api.Dto;
using User.Api.Exceptions;
using User.Api.Services;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : BaseController
    {
        private readonly IUserService _userService;

        public UserProfileController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("update")]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dto.UserProfileDto>> UpdateProfile([FromBody] UpdateUserProfileRequest request)
        {
            try
            {
                return await _userService.CreateUpdateUserProfile(request);
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

        [HttpGet]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dto.UserProfileDto>> GetUserProfile()
        {
            try
            {
                return await _userService.GetUserProfile();
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
    }
}
