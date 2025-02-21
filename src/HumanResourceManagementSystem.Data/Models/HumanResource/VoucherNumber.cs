using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class VoucherNumber
    {
        public Guid Id { get; set; }
        public string Number { get; set; } = ""; // yyyyMMddNN 格式
        public DateTime CreateAt { get; set; }
    }
}
