using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class RolePermission
    {
        public Guid RoleId { get; set; }
        public Role Role { get; set; } = null!;

        public Guid PermissionId { get; set; }
        public Permission Permission { get; set; } = null!;
    }
}
