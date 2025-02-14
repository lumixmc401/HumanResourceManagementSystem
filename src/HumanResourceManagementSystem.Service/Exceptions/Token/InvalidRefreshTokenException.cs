using BuildingBlock.Core.Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.Exceptions.Token
{
    public class InvalidRefreshTokenException: BadRequestException
    {
        public InvalidRefreshTokenException(string message) : base(message)
        {
        }
    }
}
