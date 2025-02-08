using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.UnitOfWorks.HumanResource;
using HumanResourceManagementSystem.Service.DTOs.UserClaim;
using HumanResourceManagementSystem.Service.Exceptions.User;
using HumanResourceManagementSystem.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Implementations
{
    public class UserClaimService(IHumanResourceUnitOfWork db) : IUserClaimService
    {
        private readonly IHumanResourceUnitOfWork _db = db;
        public async Task CreateAsync(CreateUserClaimDto createClaimDto)
        {
            await _db.UserClaims.AddAsync(new UserClaim
            {
                UserId = createClaimDto.UserId,
                ClaimType = createClaimDto.Type,
                ClaimValue = createClaimDto.Value
            });
            await _db.CompleteAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var userClaim = await _db.UserClaims.GetByIdAsync(id) ?? throw new UserNotFoundException(id);
            await _db.UserClaims.RemoveAsync(userClaim);
            await _db.CompleteAsync();
        }

        public Task<IEnumerable<UserClaimDto>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task GetByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Guid id, UpdateUserClaimDto updateClaimDto)
        {
            throw new NotImplementedException();
        }
    }
}
