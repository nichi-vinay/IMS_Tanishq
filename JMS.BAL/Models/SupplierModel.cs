using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.Models
{
    public class SupplierModel
    {
        public int Id { get; set; }
        public string Supplier1 { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public System.DateTime UpdatedAt { get; set; }
    }
}
