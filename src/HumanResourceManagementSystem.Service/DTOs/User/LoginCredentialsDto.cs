using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.User
{
    public class LoginCredentialsDto
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
