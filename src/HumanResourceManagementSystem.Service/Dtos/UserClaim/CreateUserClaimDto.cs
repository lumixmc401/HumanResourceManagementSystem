﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.UserClaim
{
    public class CreateUserClaimDto
    {
        public Guid UserId { get; set; }
        public string Type { get; set; } = "";
        public string Value { get; set; } = "";
    }
}
