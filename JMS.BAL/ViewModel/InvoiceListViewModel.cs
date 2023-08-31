using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
  
   public class InvoiceListViewModel
    {
        public int? InvoiceId { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int? CompanyId { get; set; }
        public string Total { get; set; }
        public string TotalPaid { get; set; }
        public string Balance { get; set; }
        public string Tax { get; set; }
        public string InvoiceDate { get; set; }
        public string Customer { get; set; }
        public string CustomerPhone { get; set; }
        public string Address { get; set; }
        public string DLNumber { get; set; }
        public string DOB { get; set; }
        public string ExpDate { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string UserId { get; set; }
        public string Employee { get; set; }
        public string Email { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string InventoryStatus { get; set; }
        public int InventoryStatusId { get; set; }
        public string TaxType { get; set; }
        public string Description { get; set; }
        public int NumberOfInvoices { get; set; }
        public string Company { get; set; }
        public List<SelectListItem> CompanyList { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> TaxTypeList { get; set; }
        public List<SelectListItem> ValidStatusList { get; set; }
        public string LayAwayInvoiceId { get; set; }
        public string InvoicePaymentId { get; set; }
        public string Payment { get; set; }
        public bool? ValidStatus { get; set; }
        public string Status { get; set; }
        public string MISC2 { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<InvoiceItemsViewModel> invoiceItems { get; set; }
    }
 
    public  class InvoiceItemsViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public int? InvoiceId { get; set; }
        public int? InventoryId { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string GoldContent { get; set; }
        public string Pieces { get; set; }
        public string OtherStones { get; set; }
        public string Price { get; set; }
        public string Category { get; set; }
        public string JewelType { get; set; }
        public string DiamondPieces { get; set; }
        public bool? Status { get; set; }
    }

    public class InvoiceModel
    {
        public int InvoiceId { get; set; }
        public string Customer { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string Tax { get; set; }
        public string DLNumber { get; set; }
        public string DOB { get; set; }
        public string ExpiryDate { get; set; }
        public string Company { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string Balance { get; set; }
        public string Total { get; set; }
    }
    public class Dates
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }

    }
    public class CustomerandPaymentDetails
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Status { get; set; }
        public string Address { get; set; }
        public string Tax { get; set; }
        public string DLNumber { get; set; }
        public string DOB { get; set; }
        public string ExpiryDate { get; set; }
        public string Company { get; set; }
        public float Check { get; set; }
        public float CreditCard { get; set; }
        public float Cash { get; set; }
        public float Total { get; set; }
    }
    public class InvoicePaymentDetails
    {
        public int Id { get; set; }
        public string Payment { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string Date { get; set; }
        public int InvoiceId { get; set; }

    }
    public class NewInvoiceDetails
    {
        public int InvoiceId { get; set; }
        public string Check { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string Total { get; set; }
        public string TaxType { get; set; }
    }
    public class CustomerWiseDetails
    {
        public int? NumberOfInvoices { get; set; }
        public string CustomerName { get; set; }
        public int? CustomerId { get; set; }
        public string TotalAmount { get; set; }

    }
    

  

}
