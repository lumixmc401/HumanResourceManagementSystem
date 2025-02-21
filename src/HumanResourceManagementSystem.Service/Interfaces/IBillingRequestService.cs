using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HumanResourceManagementSystem.Service.DTOs.Billing;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface IBillingRequestService
    {
        Task CreateRequestAsync(BillingCreateRequestDto request);
        Task UpdateRequestAsync(BillingUpdateRequestDto request);
        Task DeleteRequestAsync(Guid requestId);
        Task<BillingRequestDto> GetRequestAsync(Guid requestId);
        Task<IEnumerable<BillingRequestDto>> GetUserRequestsAsync(Guid userId);
    }
}
