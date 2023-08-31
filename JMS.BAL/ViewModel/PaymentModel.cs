using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.ViewModel
{
    public class PaymentModel
    {
        public string CustomerName { get; set; } //create
        public int InvoiceId { get; set; } //create
        public string CustomerPhone { get; set; } //create
        public string Address { get; set; } //create
        public string State { get; set; } //create
        public string City { get; set; } //create
        public string Zip { get; set; } //create
        public string DLNumber { get; set; } //create
        public string DOB { get; set; } //create
        public string ExpDate { get; set; } //create
        public string Email { get; set; }
        public string InventoryStatusId { get; set; }
        public int CompanyId { get; set; }
        public string EmployeeId { get; set; }
        public string Tax { get; set; }
        public string SubTotal { get; set; }
        public string Cheque { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string Balance { get; set; }
        public List<string> arrayOfIds { get; set; }
        public List<string> arrayOfCompanyId { get; set; }
        public List<string> arrayOfJewelTypeId { get; set; }
        public List<string> arrayOfCategoryId { get; set; }
        public List<string> arrayOfGoldWeight { get; set; }
        public List<string> arrayOfCaratWeight { get; set; }
        public List<string> arrayOfGoldContent { get; set; }
        public List<string> arrayOfPieces { get; set; }
        public List<string> arrayOfDescription { get; set; }
        public List<string> arrayOfPrice { get; set; }
        public List<string> arrayOfSupplier { get; set; }
        public List<string> arrayOfStatus { get; set; }
        public List<string> arrayOfDiamondPieces { get; set; }
        public List<string> arrayOfInventoryStatus { get; set; }
        public List<string> arrayOfDateReceived { get; set; }
        public List<string> arrayOfImg { get; set; }
    }
}
