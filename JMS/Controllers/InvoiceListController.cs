using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using JMS.Models;
using log4net;
using Newtonsoft.Json.Linq;

namespace JMS.Controllers
{
    public class InvoiceListController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceListController));
        // GET: InvoiceList
        public ActionResult Index()
        {
             
            try
            {
                var invoiceListViewModel = new BAL.ViewModel.InvoiceListViewModel
                {
                    StatusList = HelperBAL.InventoryStatusList(),
                    CompanyList = HelperBAL.CompanyList(),
                    TaxTypeList = HelperBAL.TaxTypeList,
                    ValidStatusList = HelperBAL.StatusList
                };
                return View(invoiceListViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-Index-Exception", ex);
                return View(new InvoiceListViewModel());
            }
        }

        public ActionResult GetInvoiceListJsonData(JqueryDatatableParam param)
        {
            try
            {
                DateTime fromDate = string.IsNullOrEmpty(param.FromDate) ? new DateTime() : DateTime.ParseExact(param.FromDate, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = string.IsNullOrEmpty(param.ToDate) ? new DateTime() : DateTime.ParseExact(param.ToDate, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                CustomerBAL customerBALObj = new CustomerBAL();
                UserBAL userBALObj = new UserBAL();
                InventoryStatusBAL inventoryStatusBALObj = new InventoryStatusBAL();
                InvoiceBAL invoiceBALObj = new InvoiceBAL();
                InvoiceItemsBAL invoiceItemsBALObj = new InvoiceItemsBAL();
                var inventoryBALObj = new InventoryBAL();
                var lstInvoice = invoiceBALObj.GetInvoiveList().ToList();
                if (!string.IsNullOrEmpty(param.PhoneNumber))
                {
                    var customer = customerBALObj.GetCustomersByPhoneNumber(param.PhoneNumber);
                    lstInvoice= lstInvoice.Where(x => x.CustomerId==customer.Id
                                               ).ToList();
                }
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    bool isNumeric = int.TryParse(param.sSearch, out int n);
                    if (isNumeric)
                    {
                        lstInvoice = lstInvoice.Where(x => x.InvoiceId.ToString().ToLower().Contains(param.sSearch.ToLower())
                                               ).ToList();
                    }
                    else
                    {
                        var customers = customerBALObj.GetCustomersByName(param.sSearch);
                        var filteredList =new List<InvoiceListViewModel>();
                        foreach (var item in customers)
                        {
                            var lst = lstInvoice.Where(x => x.CustomerId == item.Id).ToList();
                            foreach (var singleInvoice in lst)
                            {
                                filteredList.Add(singleInvoice);
                            }                            
                        }
                        lstInvoice = filteredList;
                    }
                }
                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    lstInvoice = lstInvoice.Where(x => DateTime.Parse(x.InvoiceDate) >= DateTime.Parse(fromDate.ToString()) && (DateTime.Parse(x.InvoiceDate) <= DateTime.Parse(toDate.ToString()))).ToList();
                }
                if (!string.IsNullOrEmpty(param.statusSearch))
                {
                    lstInvoice = lstInvoice.Where(x => !string.IsNullOrEmpty(x.ValidStatus.ToString()) && x.ValidStatus.ToString().ToLower().Equals(param.statusSearch.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(param.statusSearchId))
                {
                    lstInvoice = lstInvoice.Where(x => !string.IsNullOrEmpty(x.InvoiceId.ToString()) && x.InvoiceId.ToString().ToLower().Equals(param.statusSearchId.ToLower())).ToList();
                }
               
                var sortColumnIndex = param.iSortCol_0;
                var sortDirection = param.sSortDir_0;
                switch (sortColumnIndex)
                {
                    case 0:
                    
                        break;
                    case 1:
                       
                        break;
                    case 2:
                        lstInvoice = sortDirection == "asc" ? lstInvoice.OrderBy(c => c.InvoiceId).ToList() : lstInvoice.OrderByDescending(c => c.InvoiceId).ToList();
                        break;
                    case 3:
                        lstInvoice = sortDirection == "asc" ? lstInvoice.OrderBy(c => c.Customer).ToList() : lstInvoice.OrderByDescending(c => c.Customer).ToList();
                        break;
                    case 4:
                        lstInvoice = sortDirection == "asc" ? lstInvoice.OrderBy(c => double.Parse( c.Total)).ToList() : lstInvoice.OrderByDescending(c => double.Parse(c.Total)).ToList();

                        break;
                    case 5:
                       
                        break;
                    case 6:
                     
                        break;
                    case 7:
                        lstInvoice = sortDirection == "asc" ? lstInvoice.OrderBy(c =>double.Parse( c.Balance)).ToList() : lstInvoice.OrderByDescending(c =>double.Parse( c.Balance)).ToList();
                        break;
                    case 8:
                        lstInvoice = sortDirection == "asc" ? lstInvoice.OrderBy(c => c.ValidStatus).ToList() : lstInvoice.OrderByDescending(c => c.ValidStatus).ToList();
                        break;
                }
                
                var displayResult = lstInvoice.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength).ToList();
                
                foreach (var item in displayResult)
                {
                    item.Total = "$" + decimal.Parse(item.Total).ToString("#0.00");
                    item.Balance= "$" + decimal.Parse(item.Balance).ToString("#0.00");
                   
                    var invoiceItems = invoiceItemsBALObj.GetInvoiceItemsByInvoiceId(item.InvoiceId);
                    var invoiceListItems = new List<InvoiceItemsViewModel>();
                    foreach (var inventory in invoiceItems)
                    {
                        var inventoryRecords = inventoryBALObj.GetInventoryById(inventory.InventoryId);
                        if (inventoryRecords != null)
                        {
                            InvoiceItemsViewModel invoiceItemsViewModel = new InvoiceItemsViewModel
                            {
                                Id = inventoryRecords.Id,
                                Description = inventoryRecords.Description,
                                GoldWeight = inventoryRecords.GoldWeight,
                                Pieces = inventoryRecords.Pieces,
                                CaratWeight = inventoryRecords.CaratWeight,
                                GoldContent = "22",//inventoryRecords.GoldContent,
                                Price = "$"+inventory.Price
                            };
                            invoiceListItems.Add(invoiceItemsViewModel);
                        }
                        item.invoiceItems = invoiceListItems;
                    }

                }
                var totalRecords = lstInvoice.Count;
               
                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = displayResult
                }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Log.Error("InvoiceListController-GetInvoiceListJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetTaxDetails(string from,string to,string phNo,string filter,string status,string invoiceId)
        {
            try
            {
                DateTime fromDate = string.IsNullOrEmpty(from) ? new DateTime() : DateTime.ParseExact(from, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                DateTime toDate = string.IsNullOrEmpty(to) ? new DateTime() : DateTime.ParseExact(to, "MM-dd-yyyy", CultureInfo.InvariantCulture);
                InvoiceBAL invoiceBALObj = new InvoiceBAL();
                CustomerBAL customerBALObj = new CustomerBAL();
                var lstInvoice = invoiceBALObj.GetInvoiveList().ToList();
                if (!string.IsNullOrEmpty(status))
                {
                    lstInvoice = lstInvoice.Where(x => x.ValidStatus == bool.Parse(status)).ToList();
                }                
                if (!string.IsNullOrEmpty(invoiceId))
                {
                    lstInvoice = lstInvoice.Where(x => x.InvoiceId.ToString() == invoiceId).ToList();
                }
                if (!string.IsNullOrEmpty(phNo))
                {
                    var customer = customerBALObj.GetCustomersByPhoneNumber(phNo);
                    lstInvoice = lstInvoice.Where(x => x.CustomerId == customer.Id
                                               ).ToList();
                }
                if (!string.IsNullOrEmpty(filter))
                {
                    bool isNumeric = int.TryParse(filter, out int n);
                    if (isNumeric)
                    {
                        lstInvoice = lstInvoice.Where(x => x.InvoiceId.ToString().ToLower().Contains(filter.ToLower())
                                               ).ToList();
                    }
                    else
                    {
                        var customers = customerBALObj.GetCustomersByName(filter);
                        var filteredList = new List<InvoiceListViewModel>();
                        foreach (var item in customers)
                        {
                            var lst = lstInvoice.Where(x => x.CustomerId == item.Id).ToList();
                            foreach (var singleInvoice in lst)
                            {
                                filteredList.Add(singleInvoice);
                            }
                        }
                        lstInvoice = filteredList;
                    }
                }
                if (fromDate != DateTime.MinValue && toDate != DateTime.MinValue)
                {
                    lstInvoice = lstInvoice.Where(x => DateTime.Parse(x.InvoiceDate) >= DateTime.Parse(fromDate.ToString()) && (DateTime.Parse(x.InvoiceDate) <= DateTime.Parse(toDate.ToString()))).ToList();
                }
                double taxable = 0;
                double nontaxable = 0;
                double total = 0;
                double sum = 0;
                foreach (var item in lstInvoice)
                {
                    double cash = string.IsNullOrEmpty(item.Cash)?0.00: double.Parse(item.Cash);
                    double cheque = string.IsNullOrEmpty(item.Check) ?0.00: double.Parse(item.Check);
                    double creditCard = string.IsNullOrEmpty(item.CreditCard) ?0.00: double.Parse(item.CreditCard);
                    sum = cash+cheque+creditCard ;
                    if (item.TaxType == "INC")
                    {
                        taxable += sum;
                    }
                    else
                    {
                        nontaxable += sum;
                    }
                    total += sum;
                }
                return Json(new { data="success", taxable = taxable,nontaxable=nontaxable,total=total }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
            [HttpPost]
        public ActionResult ViewOrUpdate(BAL.ViewModel.InvoiceModel model)
        {
            try
            {
                var inventoryBALObj = new InventoryBAL();
                var invoiceItemsBAL = new InvoiceItemsBAL();
                var invoiceBALObj = new InvoiceBAL();
                var invoiceListBalObj = new InvoiceBAL();
                var invoiceItemdetails = invoiceItemsBAL.GetAllInvoiceItemsList();
                var inventoryId = invoiceItemdetails.FirstOrDefault(x => x.InvoiceId == model.InvoiceId).InventoryId;
                var inventories = inventoryBALObj.GetInventoryById(inventoryId);
                inventories.InventoryStatusId = int.Parse(model.Status);
                inventories.CompanyId = int.Parse(model.Company);
                var inventoryStatusBAlObj = new InventoryStatusBAL();
                var inventoryStatus = inventoryStatusBAlObj.GetInventoryStatusById(int.Parse(model.Status));
                var invoices = invoiceListBalObj.GetInvoicesById(model.InvoiceId);
                invoices.InventoryStatusId = int.Parse(model.Status);
                invoices.TaxType = model.Tax;
                invoices.Balance = model.Balance;
                invoiceBALObj.UpdateInvoice(invoices);
                var customerBALObj = new CustomerBAL();
                var customer = customerBALObj.GetCustomerById(invoices.CustomerId);
                customer.CustomerName = model.Customer;
                customer.CustomerPhone = model.CustomerPhone;
                customer.DLNumber = model.DLNumber;
                customer.DOB = model.DOB;
                customer.ExpDate = model.ExpiryDate;
                customer.Address = model.Address;
                customerBALObj.UpdateCustomer(customer);
                var invoicePaymentsBALObj = new InvoicePaymentsBAL();
                var invoicePayments = invoicePaymentsBALObj.GetInvoicePaymentDetailsByInvoiceId(model.InvoiceId);
                foreach(var item in invoicePayments)
                {
                    item.Cheque = model.Check;
                    item.CreditCard = model.CreditCard;
                    item.Cash = model.Cash;
                }
                foreach(var item in invoicePayments)
                {
                    invoicePaymentsBALObj.UpdateInvoicePayments(item);
                }
                return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-ViewOrUpdate-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public ActionResult DeleteInvoice(int invoiceId)
        {
            try
            {
                var invoiceBALObj = new InvoiceBAL();
                invoiceBALObj.DeleteInvoice(invoiceId);
                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-DeleteInvoice-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetBackDeletedInvoice(int invoiceId)
        {
            try
            {
                var invoiceBALObj = new InvoiceBAL();
               var invoices= invoiceBALObj.GetInvoicesById(invoiceId);
                invoices.ValidStatus = true;
                invoiceBALObj.UpdateInvoice(invoices);
                return Json(new { data = " Activated" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-GetBackDeletedInvoice-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetPaymentDetailsByInvoiceId(int invoiceId)
        {
            try
            {
                var invoicePaymentsBALObj = new InvoicePaymentsBAL();
                var result = invoicePaymentsBALObj.GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                foreach(var item in result)
                {
                    item.DisplayDate = item.CreatedAt.ToString("MM-dd-yyyy");
                }
                var invoiceBALObj = new InvoiceBAL();
              var balance= invoiceBALObj.GetInvoicesById(invoiceId).Balance;
              
                return Json(new { data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-GetPaymentDetailsByInvoiceId-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetCustomerDetailsandInvoicePaymentsByInvoiceId(int invoiceId)
        {
            try
            {

                InvoiceBAL invoiceBALObj = new InvoiceBAL();
                CustomerBAL customerBALObj = new CustomerBAL();
                var lstinvoices = invoiceBALObj.GetInvoicesById(invoiceId);
                var customerId = lstinvoices.CustomerId;
                var customers = customerBALObj.GetCustomerById(customerId);
                InvoicePaymentsBAL invoicePaymentsBALObj = new InvoicePaymentsBAL();
                var invoicePayments = invoicePaymentsBALObj.GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                float cash = 0;
                float creditCard = 0;
                float cheque = 0;
                foreach (var item in invoicePayments)
                {
                    cash = cash + float.Parse(item.Cash);
                    creditCard = creditCard + float.Parse(item.CreditCard);
                    cheque = cheque + float.Parse(item.Cheque);
                }
                var customerandPaymentDetails = new CustomerandPaymentDetails
                {
                    CustomerName = customers.CustomerName,
                    CustomerPhone = customers.CustomerPhone,
                    Status = lstinvoices.InventoryStatusId.ToString(),
                    Address = Helper.Decrypt(customers.Address),
                    DLNumber = Helper.Decrypt(customers.DLNumber),
                    DOB = Helper.Decrypt(customers.DOB),
                    ExpiryDate = Helper.Decrypt(customers.ExpDate),
                    Cash = cash,
                    CreditCard = creditCard,
                    Check = cheque,
                    Total = cash + creditCard + cheque,
                    Tax = lstinvoices.TaxType,
                    Company = string.IsNullOrEmpty(lstinvoices.CompanyId.ToString()) ? "1" : lstinvoices.CompanyId.ToString()
                };
                return Json(new { data = customerandPaymentDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-GetCustomerDetailsandInvoicePaymentsByInvoiceId-Exception", ex);

                throw;
            }
        }
        [HttpPost]
        public ActionResult AddNewInvoice(BAL.ViewModel.NewInvoiceDetails model)
        {
            try
            {
                var user = Session["LoggedInUser"] as LoggedInUserModel;
                InvoiceBAL invoiceBALObj = new InvoiceBAL();
                double total = double.Parse(model.Total);
                var invoices = invoiceBALObj.GetInvoicesById(model.InvoiceId);
                invoices.UserId = user.UserId.ToString();
                invoices.Balance = (double.Parse(invoices.Balance) - total).ToString("#0.00");
                invoiceBALObj.UpdateInvoice(invoices);
                invoices.Total = model.Total;
                invoices.TotalPaid= total.ToString();
                invoices.LayAwayInvoiceId = model.InvoiceId.ToString();
                invoices.InventoryStatusId = 5;
                invoices.Balance = "0.00";
                invoices.Cash = model.Cash;
                invoices.CreditCard = model.CreditCard;
                invoices.Check = model.Check;
                var invoiceId = invoiceBALObj.AddInvoice(invoices);
                var invoicePaymentsBALObj = new InvoicePaymentsBAL();
                var InvoicePaymentId = invoicePaymentsBALObj.AddInvoicePayment(new InvoiocePaymentModel
                {
                    Cash = model.Cash,
                    InvoiceId = model.InvoiceId,
                    CreditCard = model.CreditCard,
                    Cheque = model.Check,
                    PaymentAmount = total.ToString(),
                    NewInvoiceId = invoiceId
                }) ;
                return Json(new { data = "Redirect", url = Url.Action("Index", "InvoicePrint", new { invoiceId = invoiceId,paymentId= InvoicePaymentId,reprint=false})},JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-AddNewInvoice-Exception", ex);
                throw;
            }
        }
        public ActionResult ReprintInvoice(int invoiceId,string fromDate,string toDate,string phoneNumber, int currentPage,string statusValue,string filterInvoiceId)
        {
            try
            {
                TempData["rePrint"] = true;
                TempData["fromDate"] = fromDate;
                TempData["toDate"] = toDate;
                TempData["phoneNumber"] = phoneNumber;
                TempData["currentPage"] = currentPage;
                TempData["statusValue"] = statusValue;
                TempData["filterInvoiceId"] = filterInvoiceId;
                return Json(new { data = "Redirect", url = Url.Action("Index", "InvoicePrint", new { invoiceId = invoiceId, paymentId= 0,reprint=true }) },JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-ReprintInvoice-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        public ActionResult PrintInvoiceFunction(int invoiceId, int paymentId)
        {
            try
            {
                return Json(new { data = "Redirect", url = Url.Action("Index", "InvoicePrint", new { invoiceId = invoiceId, paymentId = paymentId, reprint = false }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-PrintInvoiceFunction-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetListofInvoicePaymentForOneInvoiceId(int invoiceId)
        {
            try
            {
                var invoicePayments = new InvoicePaymentsBAL();
              var invoicePaymentDetails=  invoicePayments.GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                return Json(new { data = invoicePaymentDetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-GetListofInvoicePaymentForOneInvoiceId-Exception", ex);
                throw;
            }
        }

        [HttpPost]
        public ActionResult RedirectToInvoice(int editInvoiceId)
        {
            TempData["invoiceId"] = editInvoiceId;
            TempData["edit"] = true;
            return RedirectToAction("Index", "Invoice");
        }

        [HttpPost]
        public ActionResult UpdateLayawayPayment(NewInvoiceDetails model)
        {
            try
            {
                var invoicePaymentsBALObj = new InvoicePaymentsBAL();
                var invoicePayment = invoicePaymentsBALObj.GetInvoicePaymentDetailsByNewInvoiceId(model.InvoiceId);
                invoicePayment.Cash = model.Cash;
                invoicePayment.Cheque = model.Check;
                invoicePayment.CreditCard = model.CreditCard;
                var total= double.Parse(model.Total);
                invoicePayment.PaymentAmount= total.ToString();
                invoicePaymentsBALObj.UpdateInvoicePayments(invoicePayment);
                InvoiceBAL invoiceBALObj = new InvoiceBAL();
                var invoice = invoiceBALObj.GetInvoicesById(model.InvoiceId);
                invoice.Cash = model.Cash;
                invoice.CreditCard = model.CreditCard;
                invoice.Check = model.Check;
                invoice.Total = model.Total;
                invoice.TotalPaid = model.Total;
                invoiceBALObj.UpdateInvoice(invoice);
                var masterInvoice = invoiceBALObj.GetInvoicesById(invoicePayment.InvoiceId);
                var lstInvoice = invoiceBALObj.GetInvoicesByLayawayInvoiceId(invoicePayment.InvoiceId.ToString());
                var price = double.Parse(masterInvoice.Total);
                foreach (var item in lstInvoice)
                {
                    price -= double.Parse(item.Cash) + double.Parse(item.Check) + double.Parse(item.CreditCard);
                }
                masterInvoice.Balance = price.ToString();
                invoiceBALObj.UpdateInvoice(masterInvoice);
                return Json(new { data = "Redirect", url = Url.Action("Index", "InvoicePrint", new { invoiceId = model.InvoiceId, paymentId = invoicePayment.Id, reprint = false }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-UpdateLayawayPaymentFunction-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetLayawayInvoiceDetails(int invoiceId)
        {
            try
            {
                InvoiceBAL invoiceBALObj = new InvoiceBAL();
                var invoice = invoiceBALObj.GetInvoicesById(invoiceId);
                var invoiceBalance = invoiceBALObj.GetInvoicesById(int.Parse(invoice.LayAwayInvoiceId)).Balance;
                return Json(new { data = "success",values=invoice,balance=invoiceBalance }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceListController-GetLayawayInvoiceDetails-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
