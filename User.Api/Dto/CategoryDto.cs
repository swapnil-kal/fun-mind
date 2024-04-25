namespace User.Api.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string BackgroundColor { get; set; }

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
