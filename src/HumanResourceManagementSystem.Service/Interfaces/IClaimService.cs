using HumanResourceManagementSystem.Service.Dtos.Claim;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface IClaimService
    {
        Task<IEnumerable<ClaimDto>> GetAllAsync();
        Task GetByIdAsync(Guid id);
        Task CreateAsync(CreateClaimDto createClaimDto);
        Task UpdateAsync(Guid id, UpdateClaimDto updateClaimDto);
        Task DeleteAsync(Guid id);
    }
}
