using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.UserClaim
{
    public class UpdateUserClaimDto
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
