using User.Api.Dto;
using User.Api.Entities;
using User.Api.Repositories;

namespace User.Api.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository) {
            _roleRepository = roleRepository;
        }
        public async Task<IEnumerable<RoleDto>> GetAllAsync()
        {
           var roles = await _roleRepository.GetAllAsync();
           return roles.Select(x => new RoleDto
            {
                Id = x.Id,
                Name = x.Name,
            }).ToList();
        }

        public async Task<RoleDto> GetByIdAsync(int id)
        {
            var role = await _roleRepository.GetById(id);

            return new RoleDto
            {
                Id = role.Id,
                Name = role.Name,
            };
        }
    }
}
