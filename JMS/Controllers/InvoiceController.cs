using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class InvoiceController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceController));
        // GET: Invoice
        public ActionResult Index()
        {
            try
            {
                var user = Session["LoggedInUser"] as LoggedInUserModel;
                var invoiceViewModel = new InvoiceViewModel
                {
                    StatusList = HelperBAL.StatusList,
                    JewelTypeList = HelperBAL.JewelTypeList(),
                    CategoryList = HelperBAL.CategoryList(),
                    Supplier = HelperBAL.SupplierList(),
                    InventoryStatus = HelperBAL.InventoryStatusList(),
                    EmployeeList = HelperBAL.EmployeeList(),
                    CompanyList = HelperBAL.CompanyList(),
                    TaxType = HelperBAL.TaxTypeList
                };
                invoiceViewModel.UserName = user.UserName;
                return View(invoiceViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceController-Index-Exception", ex);
                return View(new InvoiceViewModel());
            }
        }

        [HttpPost]
        public ActionResult RedirectToInvoicePrint(PaymentModel model)
        {
            try
            {
               
                var customerObj = new CustomerBAL();
                var customer = new CustomerModel();
                if (string.IsNullOrEmpty(model.CustomerPhone) && string.IsNullOrEmpty(model.DLNumber))
                {
                    var customerId = customerObj.AddCustomer(new CustomerModel
                    {
                        Address = model.Address,
                        State = model.State,
                        City = model.City,
                        Zip = model.Zip,
                        CustomerName = model.CustomerName,
                        CustomerPhone = model.CustomerPhone,
                        DLNumber = model.DLNumber,
                        DOB = model.DOB,
                        Email = model.Email,
                        ExpDate = model.ExpDate,
                        Status = true
                    });
                    customer = customerObj.GetCustomerById(customerId);
                }
                else
                {
                    customer = string.IsNullOrEmpty(model.CustomerPhone)?new CustomerModel():customerObj.GetCustomersByPhoneNumber(model.CustomerPhone);
                    if (customer.Id == 0&& !string.IsNullOrEmpty(model.DLNumber))
                    {
                        customer = customerObj.GetCustomersByDLNumber(Helper.encrypt(model.DLNumber));
                    }
                    if (customer.Id == 0)
                    {
                        var customerId = customerObj.AddCustomer(new CustomerModel
                        {
                            Address = model.Address,
                            State = model.State,
                            City = model.City,
                            Zip = model.Zip,
                            CustomerName = model.CustomerName,
                            CustomerPhone = model.CustomerPhone,
                            DLNumber = model.DLNumber,
                            DOB = model.DOB,
                            Email = model.Email,
                            ExpDate = model.ExpDate,
                            Status = true
                        });
                        customer = customerObj.GetCustomerById(customerId);
                    }
                    else
                    {
                        customer.Address = model.Address;
                        customer.State = model.State;
                        customer.City = model.City;
                        customer.Zip = model.Zip;
                        customer.CustomerName = model.CustomerName;
                        customer.CustomerPhone = model.CustomerPhone;
                        customer.DLNumber = model.DLNumber;
                        customer.DOB = model.DOB;
                        customer.Email = model.Email;
                        customer.ExpDate = model.ExpDate;
                        customer.Status = true;
                        customerObj.UpdateCustomer(customer);
                    }
                }
                string invoiceId = "";
                string invoicePaymentId = "0";
                for (int i = 0; i < model.arrayOfIds.Count; i++)
                {
                    if (string.IsNullOrEmpty(model.arrayOfIds[i]))
                    {
                        byte[] img = null;
                       
                        model.arrayOfIds[i] = new InventoryBAL().AddInventory(
                            new InventoryModel
                            {
                                GoldWeight = model.arrayOfGoldWeight[i],
                                CaratWeight = model.arrayOfCaratWeight[i],
                                GoldContent = model.arrayOfGoldContent[i],
                                Pieces = model.arrayOfPieces[i],
                                Description = model.arrayOfDescription[i],
                                CompanyId = int.Parse(model.arrayOfCompanyId[i]),
                                CategoryId = int.Parse(model.arrayOfCategoryId[i]),
                                JewelTypeId = int.Parse(model.arrayOfJewelTypeId[i]),
                                Price = model.arrayOfPrice[i],
                                SupplierId=int.Parse(model.arrayOfSupplier[i]),
                                Status=bool.Parse(model.arrayOfStatus[i]),
                                DiamondPieces=model.arrayOfDiamondPieces[i],
                                InventoryStatusId=int.Parse(model.arrayOfInventoryStatus[i]),
                                DateReceived=model.arrayOfDateReceived[i],
                                Image=img,
                            });
                    }
                }

                if (model.InventoryStatusId == "1")
                {
                    invoiceId = new InvoiceBAL().AddInvoice(new InvoiceListViewModel
                    {
                        Balance = model.Balance,
                        CompanyId = model.CompanyId,
                        CustomerId = customer.Id,
                        UserId = model.EmployeeId,
                        Total = model.SubTotal,
                        Cash = "",
                        CreditCard = "",
                        Check = "",
                        TotalPaid = "0.00",
                        InventoryStatusId = int.Parse(model.InventoryStatusId),
                        TaxType = model.Tax
                    }); 
                    string secondInvoiceId = new InvoiceBAL().AddInvoice(new InvoiceListViewModel
                    {
                        Balance = "0.00",
                        CompanyId = model.CompanyId,
                        CustomerId = customer.Id,
                        UserId = model.EmployeeId,
                        Total = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString(),
                        Cash = model.Cash,
                        CreditCard = model.CreditCard,
                        Check = model.Cheque,
                        TotalPaid = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString(),
                        InventoryStatusId = 5,
                        TaxType = model.Tax,
                        LayAwayInvoiceId = invoiceId
                    });
                    invoicePaymentId = new InvoicePaymentsBAL().AddInvoicePayment(new InvoiocePaymentModel
                    {
                        InvoiceId = int.Parse(invoiceId),
                        Cash = model.Cash,
                        CreditCard = model.CreditCard,
                        Cheque = model.Cheque,
                        PaymentAmount = (float.Parse(model.Cash) + float.Parse(model.Cheque) + float.Parse(model.CreditCard)).ToString(),
                        NewInvoiceId = secondInvoiceId
                    });
                }
                else
                {
                    invoiceId = new InvoiceBAL().AddInvoice(new InvoiceListViewModel
                    {
                        Balance = model.Balance,
                        CompanyId = model.CompanyId,
                        CustomerId = customer.Id,
                        UserId = model.EmployeeId,
                        Total = model.SubTotal,
                        Cash = model.Cash,
                        CreditCard = model.CreditCard,
                        Check = model.Cheque,
                        TotalPaid = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString(),
                        InventoryStatusId = int.Parse(model.InventoryStatusId),
                        TaxType = model.Tax
                    });

                }

                var invoiceItems = new InvoiceItemsBAL();
                var inventory = new InventoryBAL();
                for (int i = 0; i < model.arrayOfIds.Count; i++)
                {
                    invoiceItems.AddInvoiceItem(new InvoiceItemsViewModel
                    {
                        InvoiceId = int.Parse(invoiceId),
                        InventoryId = int.Parse(model.arrayOfIds[i]),
                        Price = model.arrayOfPrice[i]
                    });
                    var inventoryItem = inventory.GetInventoryById(int.Parse(model.arrayOfIds[i]));
                    if (inventoryItem.InventoryStatusId != 4)
                    {
                        inventoryItem.InventoryStatusId = int.Parse(model.InventoryStatusId);
                        inventory.UpdateInventory(inventoryItem);
                    }
                }
                TempData["btnEdit"] = true;
                return Json(new { data = "Redirect", url = Url.Action("Index", "InvoicePrint", new { invoiceId = invoiceId, paymentId = invoicePaymentId, reprint = false }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceController-RedirectToInvoicePrint-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult UpdateInvoice(PaymentModel model)
        {
            try
            {
                var invoiceBalObj = new InvoiceBAL();
                var invoice = invoiceBalObj.GetInvoicesById(model.InvoiceId);
                var customer = invoice.CustomerId == null ? null : (new CustomerBAL().GetCustomerById(invoice.CustomerId));
                customer.Address = model.Address;
                customer.State = model.State;
                customer.City = model.City;
                customer.Zip = model.Zip;
                customer.CustomerName = model.CustomerName;
                customer.CustomerPhone = model.CustomerPhone;
                customer.DLNumber = model.DLNumber;
                customer.DOB = model.DOB;
                customer.Email = model.Email;
                customer.ExpDate = model.ExpDate;
                customer.Status = true;
                new CustomerBAL().UpdateCustomer(customer);
                var lstPayments = new InvoicePaymentsBAL().GetInvoicePaymentDetailsByInvoiceId(model.InvoiceId);
                var balance = double.Parse(model.Balance);
                for (int i = 1; i < lstPayments.Count; i++)
                {
                    balance -= double.Parse(lstPayments[i].Cash) + double.Parse(lstPayments[i].Cheque) + double.Parse(lstPayments[i].CreditCard);
                }
                invoice.Balance = balance.ToString();
                invoice.UserId = model.EmployeeId;
                invoice.TaxType = model.Tax;
                invoice.CompanyId = model.CompanyId;
                string invoicePaymentId = "0";
                for (int i = 0; i < model.arrayOfIds.Count; i++)
                {
                    if (string.IsNullOrEmpty(model.arrayOfIds[i]))
                    {
                        model.arrayOfIds[i] = new InventoryBAL().AddInventory(
                            new InventoryModel
                            {
                                GoldWeight = model.arrayOfGoldWeight[i],
                                CaratWeight = model.arrayOfCaratWeight[i],
                                GoldContent = model.arrayOfGoldContent[i],
                                Pieces = model.arrayOfPieces[i],
                                Description = model.arrayOfDescription[i],
                                CompanyId = int.Parse(model.arrayOfCompanyId[i]),
                                CategoryId = int.Parse(model.arrayOfCategoryId[i]),
                                JewelTypeId = int.Parse(model.arrayOfJewelTypeId[i]),
                                Status = true,
                                Price = model.arrayOfPrice[i],
                                InventoryStatusId = 4
                            });
                    }//adding adhoc item to inventory

                }
                if (model.InventoryStatusId == "1" && invoice.InventoryStatusId == 1)//if both previous and updated status are layaway
                {
                    invoice.InventoryStatusId = int.Parse(model.InventoryStatusId);
                    invoice.Total = model.SubTotal;
                    invoice.Cash = "";
                    invoice.CreditCard = "";
                    invoice.Check = "";
                    invoice.TotalPaid = "0.00";
                    invoiceBalObj.UpdateInvoice(invoice);
                    var layawayInvoices = invoiceBalObj.GetInvoicesByLayawayInvoiceId(invoice.InvoiceId.ToString());
                    var secondaryInvoice = layawayInvoices[0];
                    foreach (var item in layawayInvoices)
                    {
                        item.TaxType = model.Tax;
                        invoiceBalObj.UpdateInvoice(item);
                    }
                    var id = invoice.InvoiceId;
                    invoice.InvoiceId = secondaryInvoice.InvoiceId;
                    invoice.Total = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString();
                    invoice.Cash = model.Cash;
                    invoice.CreditCard = model.CreditCard;
                    invoice.Check = model.Cheque;
                    invoice.TotalPaid = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString();
                    invoice.InventoryStatusId = 5;
                    invoice.LayAwayInvoiceId = secondaryInvoice.LayAwayInvoiceId;
                    invoice.Balance = "0.00";
                    invoiceBalObj.UpdateInvoice(invoice);
                    var invoicePayment = invoice.InvoiceId == null ? new InvoiocePaymentModel() : new InvoicePaymentsBAL().GetInvoicePaymentDetailsByNewInvoiceId(invoice.InvoiceId.Value);
                    invoicePayment.Cash = model.Cash;
                    invoicePayment.CreditCard = model.CreditCard;
                    invoicePayment.Cheque = model.Cheque;
                    invoicePayment.PaymentAmount = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString();
                    new InvoicePaymentsBAL().UpdateInvoicePayments(invoicePayment);
                    invoicePaymentId = invoicePayment.Id.ToString();
                    invoice.InvoiceId = id;
                }
                else if (model.InventoryStatusId == "5" && invoice.InventoryStatusId == 1)//If previous status was layaway and updated status is sold
                {
                    invoice.InventoryStatusId = int.Parse(model.InventoryStatusId);
                    invoice.Total = model.SubTotal;
                    invoice.Cash = model.Cash;
                    invoice.CreditCard = model.CreditCard;
                    invoice.Check = model.Cheque;
                    invoice.TotalPaid = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString();
                    invoiceBalObj.UpdateInvoice(invoice);
                    var secondInvoice = invoiceBalObj.GetInvoicesByLayawayInvoiceId(invoice.InvoiceId.ToString())[0];
                    invoiceBalObj.DeleteInvoice(secondInvoice.InvoiceId.Value);
                    var paymentBalObj = new InvoicePaymentsBAL();
                    var payments = paymentBalObj.GetInvoicePaymentDetailsByInvoiceId(invoice.InvoiceId.Value)[0];
                    payments.Cash = "0.00";
                    payments.Cheque = "0.00";
                    payments.CreditCard = "0.00";
                    payments.PaymentAmount = "0.00";
                    payments.NewInvoiceId = "0";
                    paymentBalObj.UpdateInvoicePayments(payments);
                   
                }
                else if (model.InventoryStatusId == "1")//if previous status was sold and current status is layaway
                {
                    invoice.InventoryStatusId = int.Parse(model.InventoryStatusId);
                    invoice.Total = model.SubTotal;
                    invoice.Cash = "";
                    invoice.CreditCard = "";
                    invoice.Check = "";
                    invoice.TotalPaid = "0.00";
                    invoiceBalObj.UpdateInvoice(invoice);
                    string secondInvoiceId = new InvoiceBAL().AddInvoice(new InvoiceListViewModel
                    {
                        Balance = "0.00",
                        CompanyId = model.CompanyId,
                        CustomerId = customer.Id,
                        UserId = model.EmployeeId,
                        Total = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString(),
                        Cash = model.Cash,
                        CreditCard = model.CreditCard,
                        Check = model.Cheque,
                        TotalPaid = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString(),
                        InventoryStatusId = 5,
                        TaxType = model.Tax,
                        LayAwayInvoiceId = invoice.InvoiceId.ToString()
                    });
                    invoicePaymentId = new InvoicePaymentsBAL().AddInvoicePayment(new InvoiocePaymentModel
                    {
                        InvoiceId = invoice.InvoiceId.Value,
                        Cash = model.Cash,
                        CreditCard = model.CreditCard,
                        Cheque = model.Cheque,
                        PaymentAmount = (float.Parse(model.Cash) + float.Parse(model.Cheque) + float.Parse(model.CreditCard)).ToString(),
                        NewInvoiceId = secondInvoiceId
                    });
                }
                else//if both previous and updated status are sold
                {
                    invoice.InventoryStatusId = int.Parse(model.InventoryStatusId);
                    invoice.Total = model.SubTotal;
                    invoice.Cash = model.Cash;
                    invoice.CreditCard = model.CreditCard;
                    invoice.Check = model.Cheque;
                    invoice.TotalPaid = (double.Parse(model.Cash) + double.Parse(model.CreditCard) + double.Parse(model.Cheque)).ToString();
                    invoiceBalObj.UpdateInvoice(invoice);
                }
                var invoiceItems = new InvoiceItemsBAL();
                var inventory = new InventoryBAL();
                var lstInvoiceItems = invoiceItems.GetInvoiceItemsByInvoiceId(invoice.InvoiceId);
                foreach (var item in lstInvoiceItems)
                {
                    var inventoryItem = inventory.GetInventoryById(item.InventoryId);
                    if (inventoryItem.InventoryStatusId == 4)
                    {
                        bool deleted = true;
                        for (int i = 0; i < model.arrayOfIds.Count; i++)
                        {
                            if (model.arrayOfIds[i]==inventoryItem.Id.ToString())
                            {
                                deleted = false;
                            }
                        }
                        if (deleted)
                        {
                            inventoryItem.Status = false;
                            inventory.UpdateInventory(inventoryItem);
                        }
                    }
                }
                foreach (var item in lstInvoiceItems)
                {
                    var inventoryItem = inventory.GetInventoryById(item.InventoryId);
                    if (inventoryItem.InventoryStatusId != 4)
                    {
                        inventoryItem.InventoryStatusId = 6;
                        inventory.UpdateInventory(inventoryItem);
                    }
                }//update inventory status
                for (int i = 0; i < model.arrayOfIds.Count; i++)
                {
                    if (i < lstInvoiceItems.Count)
                    {
                        lstInvoiceItems[i].InventoryId = int.Parse(model.arrayOfIds[i]);
                        lstInvoiceItems[i].Price = model.arrayOfPrice[i];
                        invoiceItems.UpdateInvoiceItems(lstInvoiceItems[i]);
                    }
                    else
                    {
                        invoiceItems.AddInvoiceItem(new InvoiceItemsViewModel
                        {
                            InvoiceId = invoice.InvoiceId,
                            InventoryId = int.Parse(model.arrayOfIds[i]),
                            Price = model.arrayOfPrice[i]
                        });
                    }

                    var inventoryItem = inventory.GetInventoryById(int.Parse(model.arrayOfIds[i]));
                    inventoryItem.InventoryStatusId = int.Parse(model.InventoryStatusId);
                    inventory.UpdateInventory(inventoryItem);
                }//add or update invoice items
                if (model.arrayOfIds.Count < lstInvoiceItems.Count)
                {
                    for (int i = model.arrayOfIds.Count; i < lstInvoiceItems.Count; i++)
                    {
                        lstInvoiceItems[i].Status = false;
                        invoiceItems.UpdateInvoiceItems(lstInvoiceItems[i]);
                    }
                }//if new items count is less than previous items count
                TempData["btnEdit"] = true;
                return Json(new { data = "Redirect", url = Url.Action("Index", "InvoicePrint", new { invoiceId = invoice.InvoiceId, paymentId = invoicePaymentId, reprint = false }) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceController-RedirectToInvoicePrint-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetCustomerByDlOrPhone(string value)
        {
            try
            {
                var customerBAlObj = new CustomerBAL();
                var customer = new CustomerModel();
                if (value.Length == 10)//find by phone number
                {
                    customer = customerBAlObj.GetCustomersByPhoneNumber(Regex.Replace(value, @"(\d{3})(\d{3})(\d{4})", "($1) $2-$3"));
                }
                else if (value.Length > 15)
                {
                    string[] output1 = value.Split('\n');
                    customer = ScanDLData(output1);
                    return Json(new { data = customer }, JsonRequestBehavior.AllowGet);
                }
                else//find by DL number
                {
                    customer = customerBAlObj.GetCustomersByDLNumber(Helper.encrypt(value));
                }
                if (customer.Id != 0)
                {
                    customer.Address = string.IsNullOrEmpty(customer.Address) ? "" : Helper.Decrypt(customer.Address);
                    customer.DLNumber = string.IsNullOrEmpty(customer.DLNumber) ? "" : Helper.Decrypt(customer.DLNumber);
                    customer.DOB = string.IsNullOrEmpty(customer.DOB) ? "" : Helper.Decrypt(customer.DOB);
                    customer.ExpDate = string.IsNullOrEmpty(customer.ExpDate) ? "" : Helper.Decrypt(customer.ExpDate);
                    return Json(new { data = customer }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { data = "null" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceController-GetCustomerByDlOrPhone-Exception", ex);
                return Json(new { data = "null" }, JsonRequestBehavior.AllowGet);
            }
        }

        private CustomerModel ScanDLData(string[] output1)
        {
            string customerName = "";
            string dateOfBirth = "";
            string address = "";
            string state = "";
            string city = "";
            string zip = "";
            string expDate = "";
            string dlNumber = "";
            for (int i = 0; i < output1.Length; i++)
            {
                if (output1[i].Length > 3)
                {
                    string header = output1[i].Substring(0, 3);

                    if (header == "DCS")
                    {
                        customerName = output1[i].ToString().Substring(3, output1[i].Length - 3);
                    }

                    if (header == "DCT")
                    {
                        customerName = output1[i].ToString().Substring(3, output1[i].Length - 3) + " " + customerName;
                    }

                    if (header == "DBB")
                    {
                        string dob = output1[i].ToString().Substring(3, output1[i].Length - 3);
                        string dobwslash = "";

                        for (int j = 0; j < dob.Length; j++)
                        {
                            if (j == 2 || j == 4)
                            {
                                dobwslash = dobwslash + "/" + dob[j];
                            }
                            else
                            {
                                dobwslash = dobwslash + dob[j];
                            }
                        }

                        dateOfBirth = dobwslash;
                    }


                    if (header == "DAG")
                    {
                        address = output1[i].ToString().Substring(3, output1[i].Length - 3);
                    }

                    if (header == "DAI")
                    {
                        city = output1[i].ToString().Substring(3, output1[i].Length - 3);
                       
                    }

                    if (header == "DAJ")
                    {
                        state = output1[i].ToString().Substring(3, output1[i].Length - 3);
                       
                    }

                    if (header == "DAQ")
                    {
                        dlNumber = output1[i].ToString().Substring(3, output1[i].Length - 3);
                    }

                    if (header == "DAK")
                    {
                        zip = output1[i].ToString().Substring(3, output1[i].Length - 3);
                      
                    }

                    if (header == "DBA")
                    {
                        expDate = output1[i].ToString().Substring(3, output1[i].Length - 3);
                    }
                }
            }
            var customerBAlObj = new CustomerBAL();
            var customer = customerBAlObj.GetCustomersByDLNumber(Helper.encrypt(dlNumber));
            if (customer.Id != 0)
            {
                customer.Address = string.IsNullOrEmpty(customer.Address) ? "" : Helper.Decrypt(customer.Address);
                customer.DLNumber = string.IsNullOrEmpty(customer.DLNumber) ? "" : Helper.Decrypt(customer.DLNumber);
                customer.DOB = string.IsNullOrEmpty(customer.DOB) ? "" : Helper.Decrypt(customer.DOB);
                customer.ExpDate = string.IsNullOrEmpty(customer.ExpDate) ? "" : Helper.Decrypt(customer.ExpDate);
                return customer;
            }
            else
            {
                customer.Address = address;
                customer.CustomerName = customerName;
                customer.ExpDate = expDate;
                customer.DOB = dateOfBirth;
                customer.DLNumber = dlNumber;
                customer.State = state;
                customer.City = city;
                customer.Zip = zip;
                return customer;
            }
        }

        [HttpPost]
        public ActionResult GetInvoiceDetails(int invoiceId)
        {
            try
            {
                var invoiceItems = new InvoiceItemsBAL().GetInvoiceItemsByInvoiceId(invoiceId);
                var invoice = new InvoiceBAL().GetInvoicesById(invoiceId);
                var customer = invoice.CustomerId == null ? null : (new CustomerBAL().GetCustomerById(invoice.CustomerId));
                invoice.CustomerName = customer.CustomerName;
                invoice.CustomerPhone = customer.CustomerPhone;
                invoice.DLNumber = Helper.Decrypt(customer.DLNumber);
                invoice.Address = Helper.Decrypt(customer.Address);
                invoice.DOB = Helper.Decrypt(customer.DOB);
                invoice.ExpDate = Helper.Decrypt(customer.ExpDate);
                invoice.City = customer.City;
                invoice.State = customer.State;
                invoice.Zip = customer.Zip;
                invoice.Email = customer.Email;
                if (string.IsNullOrEmpty(invoice.Cash) && string.IsNullOrEmpty(invoice.CreditCard) && string.IsNullOrEmpty(invoice.Check))
                {
                    var lstPayments = new InvoicePaymentsBAL().GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                    invoice.Cash = string.IsNullOrEmpty(lstPayments[0].Cash) ? "0.00" : lstPayments[0].Cash;
                    invoice.CreditCard = string.IsNullOrEmpty(lstPayments[0].CreditCard) ? "0.00" : lstPayments[0].CreditCard;
                    invoice.Check = string.IsNullOrEmpty(lstPayments[0].Cheque) ? "0.00" : lstPayments[0].Cheque;
                    invoice.TotalPaid = double.Parse(lstPayments[0].PaymentAmount).ToString("#0.00");
                }
                return Json(new { items = invoiceItems, invoiceDetails = invoice }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceController-GetInvoiceDetails-Exception", ex);
                return Json(new { data = "null" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}