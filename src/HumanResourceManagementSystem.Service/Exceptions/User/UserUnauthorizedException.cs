using BuildingBlock.Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Exceptions.User
{
    public class UserUnauthorizedException: UnauthorizedException
    {
        public UserUnauthorizedException(string message) : base(message)
        {
        }

        public UserUnauthorizedException(string name, object key) : base(name, key)
        {
        }
    }
}
