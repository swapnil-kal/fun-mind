using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class AgeCategoryMappingRequest
    {              
        public List<int> NewCategories { get; set; }

        public List<int> RemoveCategories { get; set; }
    }
}
