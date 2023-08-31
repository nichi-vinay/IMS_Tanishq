using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public class InvoiceViewModel
    {
        public string ScanId { get; set; }
        public string DLNumber { get; set; } //create
        public string ExpDate { get; set; } //create
        public string Status { get; set; } //create
        public bool InventoryStatusValue { get; set; } //create
        public string DOB { get; set; }
        public string UserName { get; set; }
        public string CaratWeight { get; set; }
        public string NoOfPieces { get; set; }
        public string Price { get; set; }
        public string GoldWeight { get; set; }
        public string GoldContent { get; set; }
        public string pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string Description { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> JewelTypeList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }//create
        public string CustomerName { get; set; }
        public string Company { get; set; }
        public string Category { get; set; }
        public string JewelType { get; set; }//create
        public string CustomerPhone { get; set; } //create
        public string Address { get; set; } //create
        public string State { get; set; } //create
        public string City { get; set; } //create
        public string Zip { get; set; } //create
        public string EmployeeId { get; set; } //create
        public string Tax { get; set; } //create
        public string SubTotal { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string Balance { get; set; }
        public string Email { get; set; }
        public int SelectedEmployee { get; set; }
        public List<SelectListItem> Supplier { get; set; }
        public List<SelectListItem> InventoryStatus { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> EmployeeList { get; set; }
        public List<SelectListItem> TaxType { get; set; } //list
        public List<InventoryTemp> Items { get; set; } //list
    }
    public class InventoryTemp
    {
        public int id { get; set; }
        public string jewelType { get; set; }
        public string category { get; set; }
        public double caratWeight { get; set; }
        public double goldWeight { get; set; }
        public int goldContent { get; set; }
        public int pieces { get; set; }
        public int diamondPieces { get; set; }
        public float price { get; set; }
        public string imgName { get; set; }
        public List<CustomerModel> Items { get; set; }
    }
}
