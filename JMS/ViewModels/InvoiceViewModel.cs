using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class InvoiceViewModel
    {
        public string ScanId { get; set; }
        public string DLNumber { get; set; } //create
        public string ExpDate { get; set; } //create
        public string Status { get; set; } //create
        public string DOB { get; set; } //create
        public string CustomerName { get; set; } //create
        public string CustomerPhone { get; set; } //create
        public string Address { get; set; } //create
        public string EmployeeId { get; set; } //create
        public string Tax { get; set; } //create
        public string SubTotal { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string Balance { get; set; }
        public int SelectedEmployee { get; set; }
        public List<Inventory> Items { get; set; } //list
    }
    public class Inventory
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