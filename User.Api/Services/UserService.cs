using System.Security.Claims;
using User.Api.Constants;
using User.Api.Dto;
using User.Api.Entities;
using User.Api.Exceptions;
using User.Api.Extensions;
using User.Api.Helpers;
using User.Api.ModelFactories;
using User.Api.Repositories;

namespace User.Api.Services
{
    public class UserService : ClaimsIdentity, IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IClaimsProvider _claimsProvider;

        public UserService(
            IUserRepository userRepository,
            IUserProfileRepository userProfileRepository,
            ICategoryRepository categoryRepository,
            IClaimsProvider claimsProvider)
        {
            _userRepository = userRepository;
            _userProfileRepository = userProfileRepository;
            _categoryRepository = categoryRepository;
            _claimsProvider = claimsProvider;
        }

        public async Task<UserDto> CreateUser(CreateUserRequest request)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);

            if (user != null)
            {
                throw new BadRequestException($"The email {request.Email} is already exists. Please try different email address.");
            }

            var userEntity = ModelFactory.MapUserEntityFromCreateUserRequest(request);

            PasswordHashUtility.CreatePasswordHash(request.Password, out string passwordHash, out string passwordSalt);
            userEntity.Password = passwordHash;
            userEntity.PasswordSalt = passwordSalt;
            userEntity.LoginProvider = LoginProviderConstant.Local;
            await _userRepository.Create(userEntity);
            var result = ModelFactory.MapUserDtoFromUserEntity(userEntity);
            return result;
        }

        public async Task<bool> DeleteUser(int userId)
        {
            var userEntity = await _userRepository.GetById(userId);

            if (userEntity == null)
                throw new BadRequestException($"The User Id does not exists.");

            userEntity.Deleted = DateTime.UtcNow;
            userEntity.DeletedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity);
            return await _userRepository.Update(userEntity);
        }

        public async Task<IEnumerable<UserDto>> GetAll()
        {
            var users = await _userRepository.GetAllAsync();

            return users.Select(x => new UserDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                Email = x.Email,
                PhoneNumber = x.PhoneNumber,
                Address = x.Address,
                RoleId = x.RoleId,
                Role = new RoleDto
                {
                    Id = x.Role.Id,
                    Name = x.Role.Name
                }
            });
        }

        public async Task<UserDto> GetById(int id)
        {
            var userEntity = await _userRepository.GetById(id);
            var result = ModelFactory.MapUserDtoFromUserEntity(userEntity);
            return result;
        }

        public async Task<UserDto> UpdateUser(int userId, UpdateUserRequest request)
        {
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                throw new BadRequestException($"The user Id {userId} does not exists.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;

            await _userRepository.Update(user);
            var result = ModelFactory.MapUserDtoFromUserEntity(user);
            return result;
        }

        public async Task<UserProfileDto> GetUserProfile()
        {
            var userId = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity);
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                throw new BadRequestException($"The user {userId} does not exists.");
            }

            var userProfileEntity = await _userProfileRepository.GetByUserIdAsync(userId);
            var result = ModelFactory.MapUserProfileDtoFromUserProfileEntity(userProfileEntity);
            var userCategories = await _categoryRepository.GetByIdsAsync(result.CategoryIds);
            result.Categories = userCategories.Select(x =>
            new CategoryDto
            {
                Id = x.Id,
                Title = x.Title,
                BackgroundColor = x.BackgroundColor,
                FileName = x.FileName,
                FilePath = x.FilePath
            }).ToList();

            return result;
        }

        public async Task<UserProfileDto> CreateUpdateUserProfile(UpdateUserProfileRequest request)
        {
            var userId = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity);
            var user = await _userRepository.GetById(userId);

            if (user == null)
            {
                throw new BadRequestException($"The user {userId} does not exists.");
            }

            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.Gender = request.Gender;
            user.PhoneNumber = request.PhoneNumber;
            user.Address = request.Address;
            await _userRepository.Update(user);

            var categories = await _categoryRepository.GetByIdsAsync(request.CategoryIds);

            var userProfile = await _userProfileRepository.GetByUserIdAsync(userId);

            if (userProfile == null)
            {
                userProfile = new UserProfileEntity
                {
                    AgeGroupId = request.AgeGroupId,
                    UserId = userId,
                    CategoryIds = string.Join(",", categories.Select(x => x.Id).ToList()),
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity)
                };
                await _userProfileRepository.Create(userProfile);
            }
            else
            {
                userProfile.AgeGroupId = request.AgeGroupId;
                userProfile.UserId = userId;
                userProfile.CategoryIds = string.Join(",", categories.Select(x => x.Id).ToList());
                userProfile.UpdatedOn = DateTime.UtcNow;
                userProfile.UpdatedBy = UserClaimsHelper.GetUserId(_claimsProvider.UserIdentity);
                await _userProfileRepository.Update(userProfile);
            }

            var result = ModelFactory.MapUserProfileDtoFromUserProfileEntity(userProfile);
            return result;
        }
    }
}
