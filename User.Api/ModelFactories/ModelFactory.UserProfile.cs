using User.Api.Dto;
using User.Api.Entities;

namespace User.Api.ModelFactories
{
    public static partial class ModelFactory
    {
        public static UserProfileDto MapUserProfileDtoFromUserProfileEntity(UserProfileEntity request)
        {
            var userProfile = new UserProfileDto()
            {
                Id = request.Id,
                UserId = request.UserId,
                AgeGroupId = request.AgeGroupId,
                CategoryIds = request.CategoryIds != null ? request.CategoryIds.Split(',').Select(int.Parse).ToList() : null,
                CreatedBy = request.CreatedBy,
                CreatedOn = request.CreatedOn,
                UpdatedBy = request.UpdatedBy,
                UpdatedOn = request.UpdatedOn,
                Deleted = request.Deleted,
                DeletedBy = request.DeletedBy,
                AgeGroup = request.AgeGroup != null ? new AgeGroupDto()
                {
                    Id = request.AgeGroup.Id,
                    Title = request.AgeGroup.Title
                } : null,
                User = request.User != null ? new UserDto { 
                    Id = request.User.Id,
                    FirstName = request.User.FirstName,
                    LastName = request.User.LastName,
                    PhoneNumber = request.User.PhoneNumber,
                    Gender = request.User.Gender,
                    Address = request.User.Address
                } : null
            };
            return userProfile;
        }      
    }
}
