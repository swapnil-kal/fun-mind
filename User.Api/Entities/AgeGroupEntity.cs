using System.ComponentModel.DataAnnotations.Schema;

namespace User.Api.Entities
{
    [Table("AgeGroups")]
    public class AgeGroupEntity : BaseEntity
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ShortDescription { get; set; }

        public string LongDescription { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public virtual ICollection<AgeGroupCategoryEntity> AgeGroupCategories { get; set; }

    }
}
