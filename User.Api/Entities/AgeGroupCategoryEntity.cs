namespace User.Api.Entities
{
    public class AgeGroupCategoryEntity : BaseEntity
    {
        public int Id { get; set; }       

        public int AgeGroupId { get; set; }

        public int CategoryId { get; set; }

        public virtual AgeGroupEntity AgeGroup { get; set; }

        public virtual CategoryEntity Category { get; set; }
    }
}
