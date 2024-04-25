using System.ComponentModel.DataAnnotations.Schema;

namespace User.Api.Entities
{
    [Table("Categories")]
    public class CategoryEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string BackgroundColor { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public virtual ICollection<AgeGroupCategoryEntity> AgeGroupCategories { get; set; }
    }
}
