using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using log4net;

namespace JMS.Controllers
{
    public class DownloadController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryMasterController));
        // GET: Download
        public ActionResult Index()
        {
            try
            {

                var inventoryViewModel = new InventoryViewModel
                {
                    StatusList = HelperBAL.StatusList,
                    Company = HelperBAL.CompanyList(),
                    JewelType = HelperBAL.JewelTypeList(),
                    Category = HelperBAL.CategoryList(),
                    Supplier = HelperBAL.SupplierList(),
                    InventoryStatus = HelperBAL.InventoryStatusList(),
                };
                return View(inventoryViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-Index-Exception", ex);
                return null;//To-do: Redirect to Error Page 
            }
        }

        public FileContentResult ExportList(string categoryId, string supplierId, string inventoryStatusId, string status)
        {
            var inventoryBalObj = new InventoryBAL();
            var inventories = inventoryBalObj.GetAllInventory().Items;
            if (categoryId != "0")
            {
                inventories = inventories.Where(x => !string.IsNullOrEmpty(x.CategoryId.ToString()) && x.CategoryId.ToString().Equals(categoryId)).ToList();
            }
            if (supplierId != "0")
            {
                inventories = inventories.Where(x => !string.IsNullOrEmpty(x.SupplierId.ToString()) && x.SupplierId.ToString().Equals(supplierId)).ToList();
            }
            if (inventoryStatusId != "0")
            {
                inventories = inventories.Where(x => !string.IsNullOrEmpty(x.InventoryStatusId.ToString()) && x.InventoryStatusId.ToString().Equals(inventoryStatusId)).ToList();
            }
            if (status != "0")
            {
                var boolstatus = bool.Parse(status);
                inventories = inventories.Where(x => x.Status == boolstatus).ToList();
            }
            var companyBalObj = new CompanyBAL();
            var allCompany = companyBalObj.GetAllCompany().Items;
            var categoryBalObj = new CategoryBAL();
            var categories = categoryBalObj.GetAllCategory().Items;
            var jewlTypeBalObj = new JewelTypeBAL();
            var jewelTypes = jewlTypeBalObj.GetAllJewelTypes().Items;
            var supplierBalObj = new SupplierBAL();
            var suppliers = supplierBalObj.GetAllSuppliers().Items;
            var inventoryStatusBalObj = new InventoryStatusBAL();
            var allInventoryStatus = inventoryStatusBalObj.GetAllInventoryStatus().Items;
            foreach (var item in inventories)
            {
                item.Company = allCompany.Where(x => x.Id == item.CompanyId).FirstOrDefault().CompanyName;
                item.JewelType = jewelTypes.Where(x => x.Id == item.JewelTypeId).FirstOrDefault().JewelTypeName;
                if (item.SupplierId != null && item.SupplierId!=0)
                {
                    item.Supplier = suppliers.Where(x => x.Id == item.SupplierId).FirstOrDefault().SupplierName;
                }
                item.Category = categories.Where(x => x.Id == item.CategoryId).FirstOrDefault().CategoryName;
                item.InventoryStatus = allInventoryStatus.Where(x => x.Id == item.InventoryStatusId).FirstOrDefault().InventoryStatusName;
            }
            var csvString = GenerateCSVString(inventories);
            var fileName = "InventoryData" + DateTime.Now.ToString() + ".csv";
            return File(new System.Text.UTF8Encoding().GetBytes(csvString), "text/csv", fileName);
        }

        public FileContentResult InvoiceExportList(string fromDate, string toDate, string custPhone, string status)
        {
            DateTime frmDate = string.IsNullOrEmpty(fromDate) ? new DateTime() : DateTime.ParseExact(fromDate, "MM-dd-yyyy", CultureInfo.InvariantCulture);
            DateTime tDate = string.IsNullOrEmpty(toDate) ? new DateTime() : DateTime.ParseExact(toDate, "MM-dd-yyyy", CultureInfo.InvariantCulture);
            var invoiceBalObj = new InvoiceBAL();
            var invoices = invoiceBalObj.GetInvoiveList();

            if (frmDate != DateTime.MinValue && tDate != DateTime.MinValue)
            {
                invoices = invoices.Where(x => DateTime.Parse(x.InvoiceDate) >= DateTime.Parse(frmDate.ToString()) && (DateTime.Parse(x.InvoiceDate) <= DateTime.Parse(tDate.ToString()))).ToList();
            }
            if (!string.IsNullOrEmpty(custPhone))
            {
                var customer = new CustomerBAL().GetCustomersByPhoneNumber(custPhone);
                invoices = invoices.Where(x => x.CustomerId == customer.Id
                                           ).ToList();
            }
            if (!string.IsNullOrEmpty(status))
            {
                invoices = invoices.Where(x => !string.IsNullOrEmpty(x.ValidStatus.ToString()) && x.ValidStatus.ToString().ToLower().Equals(status.ToLower())).ToList();
            }
            var customerBalObj = new CustomerBAL();
            var customers = customerBalObj.GetAllCustomers().Items;
            var users = new UserBAL().GetAllUsers().Items;
            var allInventoryStatus = new InventoryStatusBAL().GetAllInventoryStatus().Items;
            var companies = new CompanyBAL().GetAllCompany().Items;
            foreach (var invoice in invoices)
            {
                if (invoice.CustomerId!=null)
                {
                    invoice.CustomerName = customers.Where(x => x.Id == invoice.CustomerId).FirstOrDefault().CustomerName;
                }
                if (!string.IsNullOrEmpty(invoice.UserId))
                {
                    invoice.Employee = users.Where(x => x.Id == int.Parse(invoice.UserId)).FirstOrDefault().UserName;
                }
                if (invoice.CompanyId!=null)
                {
                    invoice.Company = companies.Where(x => x.Id == invoice.CompanyId).FirstOrDefault().CompanyName;
                }
                invoice.InventoryStatus = allInventoryStatus.Where(x => x.Id == invoice.InventoryStatusId).FirstOrDefault().InventoryStatusName;
                
            }
            var csvString= GenerateInvoiceCSVString(invoices);
            var fileName = "InvoiceData" + DateTime.Now.ToString() + ".csv";
            return File(new System.Text.UTF8Encoding().GetBytes(csvString), "text/csv", fileName);

        }
        private string GenerateInvoiceCSVString(List<InvoiceListViewModel> invoices)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id");
            sb.Append(",");
            sb.Append("Customer");
            sb.Append(",");
            sb.Append("SubTotal");
            sb.Append(",");
            sb.Append("Cheque");
            sb.Append(",");
            sb.Append("CreditCard");
            sb.Append(",");
            sb.Append("Cash");
            sb.Append(",");
            sb.Append("TotalAmount");
            sb.Append(",");
            sb.Append("Balance");
            sb.Append(",");
            sb.Append("User");
            sb.Append(",");
            sb.Append("InventoryStatus");
            sb.Append(",");
            sb.Append("TaxType");
            sb.Append(",");
            sb.Append("LayAwayInvoiceId");
            sb.Append(",");
            sb.Append("CompanyId");
            sb.Append(",");
            sb.Append("Status");
            sb.AppendLine();
            foreach (var invoice in invoices)
            {
                sb.Append(invoice.InvoiceId);
                sb.Append(",");
                sb.Append(invoice.CustomerName);
                sb.Append(",");
                sb.Append(invoice.Total);
                sb.Append(",");
                sb.Append(invoice.Check);
                sb.Append(",");
                sb.Append(invoice.CreditCard);
                sb.Append(","); 
                sb.Append(invoice.Cash);
                sb.Append(",");
                sb.Append(invoice.TotalPaid);
                sb.Append(",");
                sb.Append(invoice.Balance);
                sb.Append(",");
                sb.Append(invoice.Employee);
                sb.Append(",");
                sb.Append(invoice.InventoryStatus);
                sb.Append(",");
                sb.Append(invoice.TaxType);
                sb.Append(",");
                sb.Append(invoice.LayAwayInvoiceId);
                sb.Append(",");
                sb.Append(invoice.Company);
                sb.Append(",");
                sb.Append(invoice.ValidStatus);
                sb.AppendLine();
            }
            return sb.ToString();
        }
        private string GenerateCSVString(List<InventoryModel> inventoryList)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Id");
            sb.Append(",");
            sb.Append("Company");
            sb.Append(",");
            sb.Append("JewelType");
            sb.Append(",");
            sb.Append("Category");
            sb.Append(",");
            sb.Append("Supplier");
            sb.Append(",");
            sb.Append("CaratWeight");
            sb.Append(",");
            sb.Append("GoldWight");
            sb.Append(",");
            sb.Append("GoldContent");
            sb.Append(",");
            sb.Append("Pieces");
            sb.Append(",");
            sb.Append("DiamondPieces");
            sb.Append(",");
            sb.Append("DateRecived");
            sb.Append(",");
            sb.Append("Price");
            sb.Append(",");
            sb.Append("InventoryStatus");
            sb.Append(",");
            sb.Append("Description");
            sb.Append(",");
            sb.Append("Status");
            sb.AppendLine();
            foreach (var item in inventoryList)
            {
                sb.Append(item.Id);
                sb.Append(",");
                sb.Append(item.Company);
                sb.Append(",");
                sb.Append(item.JewelType);
                sb.Append(",");
                sb.Append(item.Category);
                sb.Append(",");
                sb.Append(item.Supplier);
                sb.Append(",");
                sb.Append(item.CaratWeight);
                sb.Append(",");
                sb.Append(item.GoldWeight);
                sb.Append(",");
                sb.Append(item.GoldContent);
                sb.Append(",");
                sb.Append(item.Pieces);
                sb.Append(",");
                sb.Append(item.DiamondPieces);
                sb.Append(",");
                sb.Append(item.DateReceived);
                sb.Append(",");
                sb.Append(item.Price);
                sb.Append(",");
                sb.Append(item.InventoryStatus);
                sb.Append(",");
                sb.Append(item.Description);
                sb.Append(",");
                sb.Append(item.Status);
                sb.AppendLine();
            }
            return sb.ToString();
        }
    }
}