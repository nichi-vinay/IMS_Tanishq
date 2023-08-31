using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.ViewModel
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
        public string subTotal { get; set; }
        public string totalPaid { get; set; }
        public string paymentMethod { get; set; }
        public string cash { get; set; }
        public string creditCard { get; set; }
        public string cheque { get; set; }
        public string tax { get; set; }
        public string shipping { get; set; }
        public string total { get; set; }
        public string balance { get; set; }
        public string salesPerson { get; set; }
        public bool layAway { get; set; }
        public int layAwayId { get; set; }
        public int InventoryStatusId { get; set; }
        public List<InventoryModel> Items { get; set; }
    }
}
