﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.Token
{
    public class RevokeRefreshTokenRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
