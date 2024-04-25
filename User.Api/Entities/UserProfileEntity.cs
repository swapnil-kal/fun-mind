using System.ComponentModel.DataAnnotations.Schema;

namespace User.Api.Entities
{
    public partial class UserProfileEntity : BaseEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int AgeGroupId { get; set; }       

        public string CategoryIds { get; set; }

        public virtual AgeGroupEntity AgeGroup { get; set; }

        public virtual UserEntity User { get; set; }
    }
}
