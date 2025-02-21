using HumanResourceManagementSystem.Data.Models.HumanResource;
using HumanResourceManagementSystem.Data.Repositories.HumanResources.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HumanResourceManagementSystem.Data.Repositories.HumanResources.Implementations
{
    public class BillingRequestRepository(DbContext context) : Repository<BillingRequest>(context), IBillingRequestRepository
    {
    }
}
