using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanResourceManagementSystem.Data.Models.HumanResource
{
    public class VoucherNumberSequence
    {
        public int Id { get; set; }
        public string Date { get; set; } = ""; // yyyyMMdd
        public int CurrentSequence { get; set; }
    }
}
