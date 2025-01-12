using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlock.Exceptions;

namespace HumanResourceManagementSystem.Service.Exceptions.User
{
    public class UserNotFoundException(Guid id) : NotFoundException("User", id)
    {
    }
}
