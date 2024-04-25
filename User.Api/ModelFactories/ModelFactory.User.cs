using User.Api.Dto;
using User.Api.Entities;

namespace User.Api.ModelFactories
{
    public static partial class ModelFactory
    {
        public static UserEntity MapUserEntityFromCreateUserRequest(CreateUserRequest request)
        {
            var user = new UserEntity()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = request.RoleId,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedOn = DateTimeOffset.Now
            };
            return user;
        }

        public static UserEntity MapUserEntityFromUpdateUserRequest(UpdateUserRequest request)
        {
            var user = new UserEntity()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                UpdatedOn = DateTimeOffset.Now          
            };
            return user;
        }

        public static UserDto MapUserDtoFromUserEntity(UserEntity request)
        {
            var user = new UserDto()
            {
                Id = request.Id,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = request.RoleId,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn,
                UpdatedBy = request.UpdatedBy,
                UpdatedOn = request.UpdatedOn,
                Deleted = request.Deleted,
                DeletedBy = request.DeletedBy,
                Role = request.Role != null ?
                    new RoleDto {
                        Id = request.Role.Id,
                        Name = request.Role.Name
                    } : null,
                UserProfile = request.UserProfile != null ?
                    new UserProfileDto
                    {
                        Id = request.UserProfile.Id,
                        UserId = request.UserProfile.UserId,
                        AgeGroupId = request.UserProfile.AgeGroupId,
                        CategoryIds = request.UserProfile.CategoryIds != null ? request.UserProfile.CategoryIds.Split(',').Select(int.Parse).ToList() : null
                    } : null,
            };
            return user;
        }
    }
}
