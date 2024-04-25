using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User.Api.Constants;

namespace User.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private Dto.UserDto userIdentity;

        //To store current user identity
        internal protected Dto.UserDto UserIdentity
        {
            get
            {
                if (this.userIdentity == null)
                {
                    if (!(HttpContext.User.Identity is ClaimsIdentity identity))
                    {
                        // throw new CustomException(string.Format(MessageConstants.InvalidUserSession, identity.Claims.FirstOrDefault(a => a.Type == "Id")?.Value));
                    }
                    else
                    {
                        this.userIdentity = new Dto.UserDto()
                        {
                            Id = Convert.ToInt32(identity.Claims.FirstOrDefault(a => a.Type == ClaimType.USER_ID)?.Value),
                            FirstName = identity.Claims.FirstOrDefault(a => a.Type == "FirstName")?.Value,
                            LastName = identity.Claims.FirstOrDefault(a => a.Type == "LastName")?.Value,                            
                            Email = identity.Claims.FirstOrDefault(a => a.Type == ClaimType.EMAIL_ADDRESS)?.Value,
                            RoleId = Convert.ToInt32(identity.Claims.FirstOrDefault(a => a.Type == ClaimType.USER_ROLE)?.Value)                            
                        };
                    }
                }
                return this.userIdentity;
            }

            set
            {
                this.userIdentity = value;
            }
        }
    }
}
