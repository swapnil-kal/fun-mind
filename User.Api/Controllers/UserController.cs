using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Principal;
using User.Api.Constants;
using User.Api.Dto;
using User.Api.Exceptions;
using User.Api.Extensions;
using User.Api.Services;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("create")]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dto.UserDto>> CreateUser([FromBody] CreateUserRequest request)
        {
            try
            {
                return await _userService.CreateUser(request);
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

        [HttpGet("all")]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        public async Task<ActionResult> GetUserById(int id)
        {
            return Ok(await _userService.GetById(id));
        }

        [HttpPut("update/{userId}")]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Dto.UserDto>> UpdateUser([FromRoute] int userId, [FromBody] UpdateUserRequest request)
        {
            try
            {
                return await _userService.UpdateUser(userId, request);
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

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                return Ok(await _userService.DeleteUser(id));
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
