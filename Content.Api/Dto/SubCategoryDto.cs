using Content.Api.Entities;

namespace Content.Api.Dto
{
    public class SubCategoryDto
    {
        public string Id { get; set; }

        public int CategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public int? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTimeOffset? Deleted { get; set; }

    }
}
