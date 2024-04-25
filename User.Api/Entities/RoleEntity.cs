using System.ComponentModel.DataAnnotations.Schema;

namespace User.Api.Entities
{
    [Table("Roles")]
    public class RoleEntity
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<UserEntity> Users { get; set; }
    }
}
