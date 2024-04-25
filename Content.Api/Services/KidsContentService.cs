using Amazon.Runtime.Internal;
using Content.Api.Constants;
using Content.Api.Dto;
using Content.Api.Entities;
using Content.Api.Exceptions;
using Content.Api.Extensions;
using Content.Api.Helpers;
using Content.Api.ModelFactories;
using Content.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Microsoft.AspNetCore.StaticFiles;

namespace Content.Api.Services
{
    public class KidsContentService : IKidsContentService
    {
        private readonly IKidsContentRepository _kidsContentRepository;
        private readonly IClaimsProvider _claimsProvider;
        private readonly IConfiguration _configuration;
        private ContentFileConfiguration _contentFileConfiguration;

        public KidsContentService(
            IKidsContentRepository kidsContentRepository,
            IClaimsProvider claimsProvider,
            IConfiguration configuration) {
            _kidsContentRepository = kidsContentRepository;
            _claimsProvider = claimsProvider;
            _configuration = configuration;
            _contentFileConfiguration = _configuration.GetSection(EnvironmentConstant.ContentFileConfiguration).Get<ContentFileConfiguration>();
        }

        public async Task<KidsContentDto> Create(CreateKidsContentRequest request)
        {
            var contentDocuments = await ProcessAndSaveFile(request.ContentDocument);
            var kidsContent = new KidsContentEntity
            {
                Title = request.Title,
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId,
                Description = request.Description,
                Metadata = request.Metadata,
                ContentDocument = contentDocuments != null ? contentDocuments : null,
                CreatedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity),
                CreatedOn = DateTime.UtcNow
            };

            var result = await _kidsContentRepository.Create(kidsContent);
            var kidsContentDto = ModelFactory.MapKidsContentDtoFromEntity(result);
            return kidsContentDto;
        }

        public async Task<ContentDocumentEntity> ProcessAndSaveFile(ContentDocumentRequest content)
        {
            var documents = new ContentDocumentEntity();
            var folderPath = Path.Combine(_contentFileConfiguration.ContentStorageLocation,
                        $"{ContentFileConstant.ContentRootFolder}/{ContentFileConstant.KidsContentFolder}");

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
           
            if (content.ImageFiles != null)
            {
                documents.ImageFiles = new List<ImageFileEntity>();
                foreach (var file in content.ImageFiles)
                {
                    var documentId = Guid.NewGuid().ToString();
                    var fileName = $"{documentId}{Path.GetExtension(file.FileName)}";

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    documents.ImageFiles.Add(new ImageFileEntity
                    {
                        DocumentId = documentId,
                        FileName = fileName,
                        FilePath = $"/{ContentFileConstant.ContentRootFolder}/{ContentFileConstant.KidsContentFolder}/{fileName}",
                    });
                }
            }
           
            if(content.AudioFiles != null)
            {
                documents.AudioFiles = new List<AudioFileEntity>();
                foreach (var file in content.AudioFiles)
                {
                    var documentId = Guid.NewGuid().ToString();
                    var fileName = $"{documentId}{Path.GetExtension(file.FileName)}";

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    documents.AudioFiles.Add(new AudioFileEntity
                    {
                        DocumentId = documentId,
                        FileName = fileName,
                        FilePath = $"/{ContentFileConstant.ContentRootFolder}/{ContentFileConstant.KidsContentFolder}/{fileName}",
                    });
                }
            }
           
            if(content.VideoFiles != null)
            {
                documents.VideoFiles = new List<VideoFileEntity>();
                foreach (var file in content.VideoFiles)
                {
                    var documentId = Guid.NewGuid().ToString();
                    var fileName = $"{documentId}{Path.GetExtension(file.FileName)}";

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }

                    documents.VideoFiles.Add(new VideoFileEntity
                    {
                        DocumentId = documentId,
                        FileName = fileName,
                        FilePath = $"/{ContentFileConstant.ContentRootFolder}/{ContentFileConstant.KidsContentFolder}/{fileName}",
                    });
                }
            }
            return documents;
        }

        public async Task<bool> Delete(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<CategoryKidsContentResponse>> GetByCategoryId(int categoryId)
        {
            var kidsContents = await _kidsContentRepository.GetByCategoryId(categoryId);
            var groupContent = kidsContents.GroupBy(x => x.SubCategoryId);
            var result = groupContent.Select(x => new CategoryKidsContentResponse
            {
                SubCategoryId = x.Key,
                CategoryId = categoryId,
                Title = x.ToList().FirstOrDefault().SubCategory.Title,
                Description = x.ToList().FirstOrDefault().SubCategory.Description,
                Contents = x.ToList().Select(c => new ContentResponseDto
                {
                    Id = c.Id,
                    Title = c.Title,
                    Description = c.Description,
                    CategoryId = c.CategoryId,
                    SubCategoryId = c.SubCategoryId,
                    ContentDocument =  ModelFactory.MapContentDocumentFromEntity(c.ContentDocument)
                }).ToList()               
            }).ToList();

            return result;

            //var kidsContentsDto = kidsContents.Select(x => ModelFactory.MapKidsContentDtoFromEntity(x)).ToList();
            // return kidsContentsDto;
        }

        public async Task<KidsContentDto> GetById(string id)
        {
            var kidsContentEntity = await _kidsContentRepository.GetById(id);
            var kidsContentDto = ModelFactory.MapKidsContentDtoFromEntity(kidsContentEntity);
            return kidsContentDto;
        }

        public async Task<List<KidsContentDto>> GetBySubCategoryId(string subCategoryId)
        {
            var kidsContents = await _kidsContentRepository.GetBySubCategoryId(subCategoryId);
            var kidsContentsDto = kidsContents.Select(x => ModelFactory.MapKidsContentDtoFromEntity(x)).ToList();
            return kidsContentsDto;
        }

        public async Task<KidsContentDto> Update(string id, UpdateKidsContentRequest subCategory)
        {
            throw new NotImplementedException();
        }

        public async Task<Tuple<string, string, MemoryStream>> GetContentDocument(string contentId, string documentId)
        {
            var result = await _kidsContentRepository.GetContentDocument(contentId, documentId);

            if (result == null)
            {
                throw new BadRequestException($"The file does not exists.");
            }

            var filePath = $"{_contentFileConfiguration.ContentStorageLocation}{result.FilePath}";
            if (!File.Exists(filePath))
            {
                throw new BadRequestException($"The file does not exists.");
            }

            var mimeType = GetMimeTypeForFileExtension(filePath);
            var bytes = File.ReadAllBytes(filePath);
            var stream = new MemoryStream(bytes);
            return new Tuple<string, string, MemoryStream>(result.FileName, mimeType, stream);
        }

        public string GetMimeTypeForFileExtension(string filePath)
        {
            const string DefaultContentType = "application/octet-stream";

            var provider = new FileExtensionContentTypeProvider();

            if (!provider.TryGetContentType(filePath, out string contentType))
            {
                contentType = DefaultContentType;
            }

            return contentType;
        }
    }
}
