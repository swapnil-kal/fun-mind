using Microsoft.AspNetCore.Mvc;
using User.Api.Dto;

namespace User.Api.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAll();

        Task<UserDto> GetById(int Id);

        Task<UserDto> CreateUser(CreateUserRequest user);

        Task<UserDto> UpdateUser(int userId, UpdateUserRequest user);        

        Task<bool> DeleteUser(int id);

        Task<UserProfileDto> GetUserProfile();

        Task<UserProfileDto> CreateUpdateUserProfile(UpdateUserProfileRequest user);
    }
}
