using User.Api.Dto;
using User.Api.Entities;
using User.Api.Exceptions;
using User.Api.Repositories;

namespace User.Api.Services
{
    public class AgeGroupService : IAgeGroupService
    {
        private readonly IAgeGroupRepository _ageGroupRepository;
        public AgeGroupService(IAgeGroupRepository ageGroupRepository)
        {
            _ageGroupRepository = ageGroupRepository;             
        }

        public async Task<IEnumerable<AgeGroupDto>> GetAllAsync()
        {
            var ageGroups = await _ageGroupRepository.GetAllAsync();
            return ageGroups.Select(x => new AgeGroupDto {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.ShortDescription,
                LongDescription = x.LongDescription,
                FileName = x.FileName,
                FilePath = x.FilePath,
            }).ToList();
        }
        
        public async Task<AgeGroupDto> GetByIdAsync(int id)
        {
            var ageGroupEntity = await _ageGroupRepository.GetById(id);

            if (ageGroupEntity == null)
                throw new BadRequestException($"The Id does not exists.");

            return  new AgeGroupDto {  Id = ageGroupEntity.Id, Title = ageGroupEntity.Title };
        }
    }
}
