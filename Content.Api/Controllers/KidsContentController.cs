using Content.Api.Constants;
using Content.Api.Dto;
using Content.Api.Exceptions;
using Content.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Content.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KidsContentController : ControllerBase
    {
        private readonly IKidsContentService _kidsContentService;

        public KidsContentController(IKidsContentService kidsContentService)
        {
            _kidsContentService = kidsContentService;
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = UserRoleConstant.Administrator)]
        public async Task<ActionResult<KidsContentDto>> CreateKidsContent([FromForm] CreateKidsContentRequest request)
        {
            try
            {
                return await _kidsContentService.Create(request);
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
        //TODO :: Need to work on once Update UI is finalize
        public async Task<ActionResult<KidsContentDto>> UpdateKidsContent(string id, [FromBody] UpdateKidsContentRequest request)
        {
            try
            {
                return await _kidsContentService.Update(id, request);
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
        public async Task<ActionResult<KidsContentDto>> GetById(string id)
        {
            try
            {
                return await _kidsContentService.GetById(id);
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
        public async Task<ActionResult<List<CategoryKidsContentResponse>>> GetByCategoryId(int categoryId)
        {
            try
            {
                return await _kidsContentService.GetByCategoryId(categoryId);
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

        [HttpGet("subCategory/{subCategoryId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        public async Task<ActionResult<List<KidsContentDto>>> GetBySubCategoryId(string subCategoryId)
        {
            try
            {
                return await _kidsContentService.GetBySubCategoryId(subCategoryId);
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

        [HttpGet("download/{contentId}/{documentId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = $"{UserRoleConstant.Administrator},{UserRoleConstant.User}")]
        public async Task<ActionResult> GetContentDocument(string contentId, string documentId)
        {
            try
            {
                var result = await _kidsContentService.GetContentDocument(contentId, documentId);

                var fileName = result.Item1;
                var mimeType = result.Item2;
                var fileStream = result.Item3;

                return File(fileStream, mimeType, fileName);
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
