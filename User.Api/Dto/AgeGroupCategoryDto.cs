using User.Api.Entities;

namespace User.Api.Dto
{
    public class AgeGroupCategoryDto
    {
        public int Id { get; set; }

        public int AgeGroupId { get; set; }

        public int CategoryId { get; set; }

        public AgeGroupDto AgeGroup { get; set; }

        public CategoryDto Category { get; set; }
    }
}
