using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class Permission
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public ICollection<RolePermission> RolePermissions { get; set; } = [];
    }
}
