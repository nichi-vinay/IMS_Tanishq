using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.ViewModel
{
  public class InvoicePaymentViewModel
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public string Cash { get; set; }
        public string CreditCard { get; set; }
        public string Cheque { get; set; }
        public string PaymentAmount { get; set; }
        public string NewInvoiceId { get; set; }
        public List<InvoiocePaymentModel> Items { get; set; }
    }
    public class InvoiocePaymentModel
    {
        public int Id { get; set; }
        public int? InvoiceId { get; set; }
        public string Cash { get; set; }
        public string CreditCard { get; set; }
        public string Cheque { get; set; }
        public string Balance { get; set; }
        public string PaymentAmount { get; set; }
        public string NewInvoiceId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string DisplayDate { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
        public Nullable<int> MISC1 { get; set; }
        public string MISC2 { get; set; }
    }
}
