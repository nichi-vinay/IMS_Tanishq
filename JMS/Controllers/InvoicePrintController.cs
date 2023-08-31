using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class InvoicePrintController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoicePrintController));
        // GET: InvoicePrint

        public ActionResult Index(int invoiceId, int paymentId, bool reprint)
        {
            //invoiceId = 1068;
            try
            {
                TempData["invoiceId"] = invoiceId;
                var payments = new InvoiocePaymentModel();
                var invoice = new InvoiceBAL().GetInvoicesById(invoiceId);
                var customer = invoice.CustomerId == null ? null : (new CustomerBAL().GetCustomerById(invoice.CustomerId));
                var invoiceItems = new InvoiceItemsBAL().GetInvoiceItemsByInvoiceId(invoiceId);
                InvoicePrintModel invoicePrintModel = new InvoicePrintModel();
                invoicePrintModel.InventoryStatusId = invoice.InventoryStatusId;
                invoicePrintModel.salesPerson = new UserBAL().GetUserById(int.Parse(invoice.UserId)).Name;
                invoicePrintModel.companyName = invoice.CompanyId == null ? "Kirti Jewelers" : new CompanyBAL().GetCompanyById(invoice.CompanyId.Value).CompanyName;
                invoicePrintModel.companyAddressLine1 = "6655 Harwin Dr Ste 101A";
                invoicePrintModel.companyAddressLine2 = "Houston, TX 77036";
                invoicePrintModel.companyPhone = "+1 (713) 789-{4653}";
                invoicePrintModel.companyEmail = "kirtijewelers@gmail.com";
                invoicePrintModel.customerName = customer.CustomerName;
                invoicePrintModel.customerAddressLine1 = Helper.Decrypt(customer.Address);
                invoicePrintModel.customerAddressLine1 += string.IsNullOrEmpty( Helper.Decrypt(customer.Address))?"":",";
                invoicePrintModel.customerAddressLine2 = string.IsNullOrEmpty(customer.City) ? "" : customer.City;
                invoicePrintModel.customerAddressLine2 += string.IsNullOrEmpty(customer.State) ? "" : ", " + customer.State;
                invoicePrintModel.customerAddressLine2 += string.IsNullOrEmpty(customer.Zip) ? "" : ", " + customer.Zip;
                invoicePrintModel.customerPhone = customer.CustomerPhone;
                invoicePrintModel.customerEmail = customer.Email;
                invoicePrintModel.invoice = invoiceId.ToString();
                invoicePrintModel.orderId = "";
                invoicePrintModel.currentDate = invoice.CreatedAt;
                invoicePrintModel.dueDate = DateTime.Today.AddDays(10);
                invoicePrintModel.subTotal = invoice.Total;
                invoicePrintModel.tax = invoice.TaxType;
                invoicePrintModel.balance = invoice.Balance;
                int layawayId;
                if (reprint)//if reprint
                {
                    if ((!string.IsNullOrEmpty(invoice.LayAwayInvoiceId)) && int.TryParse(invoice.LayAwayInvoiceId, out layawayId) && layawayId > 0)//If it is layaway payment
                    {
                        invoicePrintModel.layAway = true;
                        invoicePrintModel.layAwayId = layawayId;
                        var paymentDetails = new InvoicePaymentsBAL().GetInvoicePaymentDetailsByNewInvoiceId(invoiceId);
                        invoicePrintModel.cash = paymentDetails.Cash;
                        invoicePrintModel.creditCard = paymentDetails.CreditCard;
                        invoicePrintModel.cheque = paymentDetails.Cheque;
                        invoicePrintModel.totalPaid = double.Parse(paymentDetails.PaymentAmount).ToString("#0.00");
                        invoicePrintModel.balance = "0.00";
                    }
                    else
                    {
                        var lstPayments = new InvoicePaymentsBAL().GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                        var cash = 0.00;
                        var credit = 0.00;
                        var cheque = 0.00;
                        var paid = 0.00;
                        if (string.IsNullOrEmpty(invoice.Cash) && string.IsNullOrEmpty(invoice.CreditCard) && string.IsNullOrEmpty(invoice.Check))
                        {
                            cash += string.IsNullOrEmpty(lstPayments[0].Cash) ? 0.00 : double.Parse(lstPayments[0].Cash);
                            credit += string.IsNullOrEmpty(lstPayments[0].CreditCard) ? 0.00 : double.Parse(lstPayments[0].CreditCard);
                            cheque += string.IsNullOrEmpty(lstPayments[0].Cheque) ? 0.00 : double.Parse(lstPayments[0].Cheque);
                            paid += double.Parse(lstPayments[0].PaymentAmount);
                        }
                        else
                        {
                            cash += string.IsNullOrEmpty(invoice.Cash) ? 0.00 : double.Parse(invoice.Cash);
                            credit += string.IsNullOrEmpty(invoice.CreditCard) ? 0.00 : double.Parse(invoice.CreditCard);
                            cheque += string.IsNullOrEmpty(invoice.Check) ? 0.00 : double.Parse(invoice.Check);
                            paid += double.Parse(invoice.TotalPaid);
                        }
                        invoicePrintModel.cash = cash.ToString();
                        invoicePrintModel.creditCard = credit.ToString();
                        invoicePrintModel.cheque = cheque.ToString();
                        invoicePrintModel.totalPaid = paid.ToString("#0.00");
                    }
                }
                else//if it is not reprint
                {
                    if (string.IsNullOrEmpty(invoice.LayAwayInvoiceId))//If it is not layaway payment
                    {
                        if (string.IsNullOrEmpty(invoice.Cash) && string.IsNullOrEmpty(invoice.CreditCard) && string.IsNullOrEmpty(invoice.Check))
                        {
                            var lstPayments = new InvoicePaymentsBAL().GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                            invoicePrintModel.cash = string.IsNullOrEmpty(lstPayments[0].Cash) ? "0.00" : lstPayments[0].Cash;
                            invoicePrintModel.creditCard = string.IsNullOrEmpty(lstPayments[0].CreditCard) ? "0.00" : lstPayments[0].CreditCard;
                            invoicePrintModel.cheque = string.IsNullOrEmpty(lstPayments[0].Cheque) ? "0.00" : lstPayments[0].Cheque;
                            invoicePrintModel.totalPaid = double.Parse(lstPayments[0].PaymentAmount).ToString("#0.00");
                        }
                        else
                        {
                            invoicePrintModel.cash = string.IsNullOrEmpty(invoice.Cash) ? "0.00" : invoice.Cash;
                            invoicePrintModel.creditCard = string.IsNullOrEmpty(invoice.CreditCard) ? "0.00" : invoice.CreditCard;
                            invoicePrintModel.cheque = string.IsNullOrEmpty(invoice.Check) ? "0.00" : invoice.Check;
                            invoicePrintModel.totalPaid = double.Parse(invoice.TotalPaid).ToString("#0.00");
                        }
                    }
                    else//if it is layaway payment
                    {
                        payments = new InvoicePaymentsBAL().GetinvoicePaymentId(paymentId);
                        invoicePrintModel.cash = string.IsNullOrEmpty(payments.Cash) ? "0.00" : payments.Cash;
                        invoicePrintModel.creditCard = string.IsNullOrEmpty(payments.CreditCard) ? "0.00" : payments.CreditCard;
                        invoicePrintModel.cheque = string.IsNullOrEmpty(payments.Cheque) ? "0.00" : payments.Cheque;
                        invoicePrintModel.totalPaid = double.Parse(payments.PaymentAmount).ToString("#0.00");
                        invoicePrintModel.balance = "0.00";
                    }
                }
                if ((!string.IsNullOrEmpty(invoice.LayAwayInvoiceId)) && int.TryParse(invoice.LayAwayInvoiceId, out layawayId) && layawayId > 0)//if layaway change print design
                {
                    invoicePrintModel.layAway = true;
                    invoicePrintModel.layAwayId = layawayId;
                    invoicePrintModel.balance = "0.00";
                }
                else//else print design
                {
                    invoicePrintModel.layAway = false;
                    invoicePrintModel.Items = GetData(invoiceItems);
                }
                invoicePrintModel.paymentMethod = "";
                if (double.Parse(invoicePrintModel.cash) > 0)
                {
                    invoicePrintModel.paymentMethod = "Cash";
                }
                if (double.Parse(invoicePrintModel.cheque) > 0)
                {
                    invoicePrintModel.paymentMethod = string.IsNullOrEmpty(invoicePrintModel.paymentMethod) ? "Cheque" : invoicePrintModel.paymentMethod + " /Cheque";
                }
                if (double.Parse(invoicePrintModel.creditCard) > 0)
                {
                    invoicePrintModel.paymentMethod = string.IsNullOrEmpty(invoicePrintModel.paymentMethod) ? "Credit Card" : invoicePrintModel.paymentMethod + " /Credit Card";
                }
                return View(invoicePrintModel);
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePrintController-Index-Exception", ex);
                return View(new InvoicePrintModel());
            }
        }

        [HttpPost]
        public ActionResult RedirectToInvoice()
        {
            TempData["edit"] = true;
            return RedirectToAction("Index", "Invoice");
        }

        private List<InventoryModel> GetData(List<InvoiceItemsViewModel> invoiceItems)
        {
            try
            {
                if (invoiceItems != null)
                {
                    List<InventoryModel> inventories = new List<InventoryModel>();
                    foreach (var item in invoiceItems)
                    {
                        var obj1 = new InventoryModel()
                        {
                            Id = item.InventoryId == null ? default(int) : item.InventoryId.Value,
                            Price = item.Price
                        };
                        var inventoryItem = new InventoryBAL().GetInventoryById(obj1.Id);
                        obj1.JewelType = inventoryItem.JewelType;
                        obj1.Category = inventoryItem.Category;
                        obj1.CaratWeight = inventoryItem.CaratWeight.Contains("CT") || inventoryItem.CaratWeight.Contains("ct") ? inventoryItem.CaratWeight.Replace("CT", "ct") : inventoryItem.CaratWeight + "ct";
                        obj1.GoldWeight = inventoryItem.GoldWeight;
                        obj1.GoldContent = inventoryItem.GoldContent;
                        obj1.Pieces = inventoryItem.Pieces;
                        obj1.DiamondPieces = inventoryItem.DiamondPieces;
                        obj1.Description = inventoryItem.Description;
                        obj1.Description = string.IsNullOrEmpty(inventoryItem.GoldContent) ? obj1.Description :
                            string.IsNullOrEmpty(obj1.Description) ? obj1.Description + inventoryItem.GoldContent + "kt gold" :
                            obj1.Description + ", " + inventoryItem.GoldContent + "kt gold";
                        obj1.Description = inventoryItem.CompanyId == 2 ? string.IsNullOrEmpty(obj1.CaratWeight) ? obj1.Description
                            : string.IsNullOrEmpty(inventoryItem.Description) ? obj1.Description + obj1.CaratWeight
                            : obj1.Description + ", " + obj1.CaratWeight : obj1.Description;
                        obj1.Description = string.IsNullOrEmpty(inventoryItem.GoldWeight) ? obj1.Description :
                            string.IsNullOrEmpty(obj1.Description) ? obj1.Description + inventoryItem.GoldWeight + "gm" :
                            obj1.Description + ", " + inventoryItem.GoldWeight + "gm";
                        inventories.Add(obj1);
                    }
                    while (inventories.Count < 10)
                    {
                        inventories.Add(new InventoryModel());
                    }
                    return inventories;
                }
                else
                {
                    return new List<InventoryModel>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePrintController-GetData-Exception", ex);
                return new List<InventoryModel>();
            }
        }
    }
}