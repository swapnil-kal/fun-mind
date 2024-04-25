using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using User.Api.Constants;
using User.Api.Dto;
using User.Api.Services;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryAgeMappingController : BaseController
    {
        private readonly IAgeGroupCategoryService _ageGroupCategoryService;

        public CategoryAgeMappingController(IAgeGroupCategoryService ageGroupCategoryService)
        {
            _ageGroupCategoryService = ageGroupCategoryService;
        }

        [HttpGet("all/{ageGroupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        // [Authorize(Roles = $"{UserRoleConstant.Administrator}")]
        [AllowAnonymous]
        public async Task<ActionResult> FindByAgeGroupIdAsync(int ageGroupId)
        {
            return Ok(await _ageGroupCategoryService.FindByAgeGroupIdAsync(ageGroupId));
        }

        [HttpPost("assignCategories/{ageGroupId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{UserRoleConstant.Administrator}")]
        public async Task<ActionResult> AssignCategories(int ageGroupId, [FromBody] AgeCategoryMappingRequest request)
        {
            return Ok(await _ageGroupCategoryService.AssignCategories(ageGroupId, request));
        }
    }
}
