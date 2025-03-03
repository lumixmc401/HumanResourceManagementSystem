﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.User
{
    public class AuthenticationResultDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; } = "";
        public IEnumerable<Guid> RoleIds = [];
    }
}
