using BuildingBlock.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Exceptions.Claim
{
    internal class ClaimNotFoundException(Guid id) : NotFoundException("Claim", id)
    {
    }
}
