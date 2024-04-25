using System.ComponentModel.DataAnnotations;

namespace Content.Api.Dto
{
    public class CreateSubCategoryRequest
    {

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
