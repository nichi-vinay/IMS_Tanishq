using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class InvoicePrintModel
    {
        public string companyName { get; set; }
        public string companyAddressLine1 { get; set; }
        public string companyAddressLine2 { get; set; }
        public string companyPhone { get; set; }
        public string companyEmail { get; set; }
        public string customerName { get; set; }
        public string customerAddressLine1 { get; set; }
        public string customerAddressLine2 { get; set; }
        public string customerPhone { get; set; }
        public string customerEmail { get; set; }
        public string invoice { get; set; }
        public string orderId { get; set; }
        public DateTime currentDate { get; set; }
        public DateTime dueDate { get; set; }
        public double subTotal { get; set; }
        public double tax { get; set; }
        public double shipping { get; set; }
        public double total { get; set; }
        public List<Inventory> Items { get; set; }
    }
}