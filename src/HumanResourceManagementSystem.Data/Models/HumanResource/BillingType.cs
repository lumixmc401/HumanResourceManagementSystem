using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class BillingType
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Code { get; set; }
    }
}
