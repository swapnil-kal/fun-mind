using User.Api.Dto;
using User.Api.Entities;
using User.Api.Repositories;

namespace User.Api.Services
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleDto>> GetAllAsync();

        Task<RoleDto> GetByIdAsync(int id);
    }
}
