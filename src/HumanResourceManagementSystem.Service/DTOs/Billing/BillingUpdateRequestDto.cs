using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Service.DTOs.Billing
{
    public class BillingUpdateRequestDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public Guid TypeId { get; set; }
    }
}
