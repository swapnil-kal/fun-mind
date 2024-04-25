namespace User.Api.Dto
{
    public class UserProfileDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AgeGroupId { get; set; }

        public List<int> CategoryIds { get; set; }

        public List<CategoryDto> Categories { get; set; }

        public int? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTimeOffset? Deleted { get; set; }

        public UserDto User { get; set; }

        public AgeGroupDto AgeGroup { get; set; }

    }
}
