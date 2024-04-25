using System.ComponentModel.DataAnnotations;

namespace User.Api.Dto
{
    public class GeneralResponse
    {        
        public bool Success { get; set; }

        public string Message { get; set; }
    }
}
