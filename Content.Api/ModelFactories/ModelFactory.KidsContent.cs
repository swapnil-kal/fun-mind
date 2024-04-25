using Content.Api.Dto;
using Content.Api.Entities;

namespace Content.Api.ModelFactories
{
    public static partial class ModelFactory
    {
        public static KidsContentDto MapKidsContentDtoFromEntity(KidsContentEntity request)
        {
            var content = new KidsContentDto()
            {
                Id = request.Id,
                Title = request.Title,
                Description = request.Description,
                CategoryId = request.CategoryId,
                SubCategoryId = request.SubCategoryId,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn,
                UpdatedBy = request.UpdatedBy,
                UpdatedOn = request.UpdatedOn,
                Deleted = request.Deleted,
                DeletedBy = request.DeletedBy,
                ContentDocument = new ContentDocument()
            };

            if (request.ContentDocument != null && request.ContentDocument.ImageFiles != null)
            {
                content.ContentDocument.ImageFiles = new List<ImageFile>();
                foreach (var file in request.ContentDocument.ImageFiles)
                {
                    content.ContentDocument.ImageFiles.Add(new ImageFile
                    {
                        DocumentId = file.DocumentId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                    });
                }
            }

            if (request.ContentDocument != null && request.ContentDocument.AudioFiles != null)
            {
                content.ContentDocument.AudioFiles = new List<AudioFile>();
                foreach (var file in request.ContentDocument.AudioFiles)
                {
                    content.ContentDocument.AudioFiles.Add(new AudioFile
                    {
                        DocumentId = file.DocumentId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                    });
                }
            }

            if (request.ContentDocument != null && request.ContentDocument.VideoFiles != null)
            {
                content.ContentDocument.VideoFiles = new List<VideoFile>();
                foreach (var file in request.ContentDocument.VideoFiles)
                {
                    content.ContentDocument.VideoFiles.Add(new VideoFile
                    {
                        DocumentId = file.DocumentId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                    });
                }
            }
            return content;
        }

        public static ContentDocument MapContentDocumentFromEntity(ContentDocumentEntity request)
        {
            var contentDocument = new ContentDocument();
            if (request != null && request.ImageFiles != null)
            {
                contentDocument.ImageFiles = new List<ImageFile>();
                foreach (var file in request.ImageFiles)
                {
                    contentDocument.ImageFiles.Add(new ImageFile
                    {
                        DocumentId = file.DocumentId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                    });
                }
            }

            if (request != null && request.AudioFiles != null)
            {
                contentDocument.AudioFiles = new List<AudioFile>();
                foreach (var file in request.AudioFiles)
                {
                    contentDocument.AudioFiles.Add(new AudioFile
                    {
                        DocumentId = file.DocumentId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                    });
                }
            }

            if (request != null && request.VideoFiles != null)
            {
                contentDocument.VideoFiles = new List<VideoFile>();
                foreach (var file in contentDocument.VideoFiles)
                {
                    contentDocument.VideoFiles.Add(new VideoFile
                    {
                        DocumentId = file.DocumentId,
                        FileName = file.FileName,
                        FilePath = file.FilePath,
                    });
                }
            }
            return contentDocument;
        }
    }
}
