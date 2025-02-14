using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlock.Core.Common.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException(string message) : base(message)
        {
        }
        public UnauthorizedException(string name, object key) : base($"實體 \"{name}\" {key}沒有找到")
        {
        }
    }
}
