using HumanResourceManagementSystem.Service.DTOs.UserClaim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface IUserClaimService
    {
        Task<IEnumerable<UserClaimDto>> GetAllAsync();
        Task GetByIdAsync(Guid id);
        Task CreateAsync(CreateUserClaimDto createClaimDto);
        Task UpdateAsync(Guid id, UpdateUserClaimDto updateClaimDto);
        Task DeleteAsync(Guid id);
    }
}
