namespace User.Api.Entities
{
    public class BaseEntity
    {
        public int? CreatedBy { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public int? UpdatedBy { get; set; }

        public DateTimeOffset? UpdatedOn { get; set; }

        public int? DeletedBy { get; set; }

        public DateTimeOffset? Deleted { get; set; }        
    }
}
