using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class BillingRequest
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        public bool IsPaid { get; set; }
        public Guid VoucherNumberId { get; set; }
        public VoucherNumber VoucherNumber { get; set; } = null!;
        public string Description { get; set; } = "";
        public decimal Amount { get; set; }
        public Guid BillingTypeId { get; set; }
        public BillingType BillingType { get; set; } = null!;
        public DateTime CreateAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
