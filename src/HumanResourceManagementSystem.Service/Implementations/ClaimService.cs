using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.UnitOfWorks;
using HumanResourceManagementSystem.Service.Dtos.Claim;
using HumanResourceManagementSystem.Service.Exceptions.User;
using HumanResourceManagementSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Implementations
{
    public class ClaimService(IHumanResourceUnitOfWork db) : IClaimService
    {
        private readonly IHumanResourceUnitOfWork _db = db;
        public async Task CreateAsync(CreateClaimDto createClaimDto)
        {
            await _db.Claims.AddAsync(new Claim
            {
                Type = createClaimDto.Type,
                Value = createClaimDto.Value
            });
            await _db.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var user = await _db.Users.GetByIdAsync(id) ?? throw new UserNotFoundException(userId);
            await _db.Users.RemoveAsync(user);
            await _db.CompleteAsync();
        }

        public Task<IEnumerable<ClaimDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, UpdateClaimDto updateClaimDto)
        {
            throw new NotImplementedException();
        }
    }
}
