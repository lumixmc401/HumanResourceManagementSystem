using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildingBlock.Core.Common.Exceptions;

namespace HumanResourceManagementSystem.Service.Exceptions.User
{
    public class LoginFailException : UnauthorizedException
    {
        public LoginFailException(string message) : base(message)
        {
        }

        public LoginFailException(string name, object key) : base(name, key)
        {
        }
    }
}
