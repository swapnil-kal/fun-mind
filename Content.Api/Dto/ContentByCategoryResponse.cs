namespace Content.Api.Dto
{
    public class CategoryKidsContentResponse
    {
        public int CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public List<ContentResponseDto> Contents { get; set; }
    }    

    public class ContentResponseDto
    {
        public string Id { get; init; }

        public int CategoryId { get; set; }

        public string SubCategoryId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ContentDocument ContentDocument { get; set; }
    }
}
