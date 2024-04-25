using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Api.Constants;
using User.Api.Services;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AgeGroupController : BaseController
    {
        private readonly IAgeGroupService _ageGroupService;

        public AgeGroupController(IAgeGroupService ageGroupService)
        {
            _ageGroupService = ageGroupService;
        }

        [HttpGet("all")]       
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        [AllowAnonymous]
        public async Task<ActionResult> GetAll()
        {  
            return Ok(await _ageGroupService.GetAllAsync());
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]        
        [AllowAnonymous]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await _ageGroupService.GetByIdAsync(id));
        }

    }
}
