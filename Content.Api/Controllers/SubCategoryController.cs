using Content.Api.Constants;
using Content.Api.Dto;
using Content.Api.Exceptions;
using Content.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Content.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubCategoryController : ControllerBase
    {
        private readonly ISubCategoryService _subCategoryService;

        public SubCategoryController(ISubCategoryService subCategoryService)
        {
            _subCategoryService = subCategoryService;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        public async Task<ActionResult<SubCategoryDto>> CreateSubCategory([FromBody] CreateSubCategoryRequest request)
        {
            try
            {
                return await _subCategoryService.Create(request);
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

        [HttpPut("update/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        public async Task<ActionResult<SubCategoryDto>> UpdateSubCategory(string id, [FromBody] UpdateSubCategoryRequest request)
        {
            try
            {
                return await _subCategoryService.Update(id, request);
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


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        public async Task<ActionResult<SubCategoryDto>> GetSubCategory(string id)
        {
            try
            {
                return await _subCategoryService.GetById(id);
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

        [HttpGet("category/{categoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        public async Task<ActionResult<List<SubCategoryDto>>> GetByCategoryId(int categoryId)
        {
            try
            {
                return await _subCategoryService.GetByCategoryId(categoryId);
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
