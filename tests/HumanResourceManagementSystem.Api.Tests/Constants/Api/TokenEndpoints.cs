using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Api.Tests.Constants.Api
{
    public static class TokenEndpoints
    {
        public const string Base = "/api/token";
        public const string Login = Base + "/login";
        public const string Refresh = Base + "/refresh";
        public const string Revoke = Base + "/revoke";
    }
}
