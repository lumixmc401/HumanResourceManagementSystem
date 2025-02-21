using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.Token
{
    public class TokenCacheDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public IEnumerable<Guid> RoleIds { get; set; }
        public string Token { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DeviceInfo { get; set; }
    }
}
