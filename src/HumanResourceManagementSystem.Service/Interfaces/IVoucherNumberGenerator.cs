using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Interfaces
{
    public interface IVoucherNumberGenerator
    {
        Task<string> GenerateVoucherNumberAsync(DateTime date);
    }
}
