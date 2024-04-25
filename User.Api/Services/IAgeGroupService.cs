using User.Api.Dto;
using User.Api.Entities;

namespace User.Api.Services
{
    public interface IAgeGroupService
    {
        Task<IEnumerable<AgeGroupDto>> GetAllAsync();

        Task<AgeGroupDto> GetByIdAsync(int Id);
    }
}
