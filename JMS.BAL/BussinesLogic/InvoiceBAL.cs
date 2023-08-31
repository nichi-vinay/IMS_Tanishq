using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.BussinesLogic
{
    public class InvoiceBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceBAL));
        public List<InvoiceListViewModel> GetInvoiveList()
        {
            try
            {
                List<InvoiceListViewModel> invoiceListModel = new List<InvoiceListViewModel>();
                var invoiceDALObj = new InvoiceDAL();
                var lstInvoices = invoiceDALObj.GetInvoiveList();
                foreach (var item in lstInvoices)
                {
                    var obj = new InvoiceListViewModel()
                    {
                        InvoiceId = item.Id,
                        CustomerId = item.CustomerId,
                        Customer=item.Customer,
                        InvoiceDate = item.CreatedAt.ToShortDateString(),
                        UserId = item.UserId.ToString(),
                        InventoryStatusId = item.InventoryStatusId,
                        Balance = item.Balance,
                        Total = item.SubTotal,
                        Cash = item.Cash,
                        CreditCard = item.CreditCard,
                        Check = item.Cheque,
                        TotalPaid = item.TotalAmount,
                        LayAwayInvoiceId = item.LayAwayInvoiceId,
                        TaxType = item.TaxType,
                        CompanyId = item.CompanyId,
                        ValidStatus=item.Status,
                        Status=item.InventoryStatusName,
                        Employee=item.Employee
                    };

                    invoiceListModel.Add(obj);
                }

                return invoiceListModel;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-GetInvoiveList-Exception", ex);
                return null;
            }
        }

        public InvoiceListViewModel GetInvoicesById(int? id)
        {
            try
            {

                InvoiceDAL invoiceDALObj = new InvoiceDAL();
                var invoicedetails = invoiceDALObj.GetInvoiceById(id);
                if (invoicedetails != null)
                {
                    return new InvoiceListViewModel()
                    {
                        InvoiceId = invoicedetails.Id,
                        CustomerId = invoicedetails.CustomerId,
                        Total = invoicedetails.SubTotal,
                        Balance = invoicedetails.Balance,
                        Cash = invoicedetails.Cash,
                        CreditCard = invoicedetails.CreditCard,
                        Check = invoicedetails.Cheque,
                        TotalPaid = invoicedetails.TotalAmount,
                        UserId = invoicedetails.UserId.ToString(),
                        InventoryStatusId = invoicedetails.InventoryStatusId,
                        TaxType = invoicedetails.TaxType,
                        LayAwayInvoiceId = invoicedetails.LayAwayInvoiceId,
                        InvoiceDate = invoicedetails.CreatedAt.ToShortDateString(),
                        CompanyId = invoicedetails.CompanyId,
                        CreatedAt = invoicedetails.CreatedAt,
                        ValidStatus=invoicedetails.Status
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-GetInvoicesById-Exception", ex);
                throw;
            }
        }

        public string AddInvoice(InvoiceListViewModel invoice)
        {
            try
            {
                var invoiceDALObj = new InvoiceDAL();
                var invoiceId = invoiceDALObj.AddInvoice(new Invoice
                {
                    Balance = invoice.Balance,
                    CustomerId = invoice.CustomerId,
                    SubTotal = invoice.Total,
                    Cash = invoice.Cash,
                    CreditCard = invoice.CreditCard,
                    Cheque = invoice.Check,
                    TotalAmount = invoice.TotalPaid,
                    UserId = int.Parse(invoice.UserId),
                    InventoryStatusId = invoice.InventoryStatusId,
                    TaxType = invoice.TaxType,
                    LayAwayInvoiceId = invoice.LayAwayInvoiceId,
                    CompanyId = invoice.CompanyId,
                    CreatedAt = DateTime.Now,
                    Status=true
                });
                return invoiceId;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-AddInvoice-Exception", ex);
                return "";
            }
        }
        public void UpdateInvoice(InvoiceListViewModel newInvoice)
        {
            try
            {
                var invoiceDALObj = new InvoiceDAL();
                invoiceDALObj.UpdateInvoice(new Invoice()
                {
                    Id = newInvoice.InvoiceId.Value,
                    Balance = newInvoice.Balance,
                    CustomerId = newInvoice.CustomerId,
                    SubTotal = newInvoice.Total,
                    Cash = newInvoice.Cash,
                    CreditCard = newInvoice.CreditCard,
                    Cheque = newInvoice.Check,
                    TotalAmount = newInvoice.Total,
                    UserId = int.Parse(newInvoice.UserId),
                    InventoryStatusId = newInvoice.InventoryStatusId,
                    TaxType = newInvoice.TaxType,
                    LayAwayInvoiceId = newInvoice.LayAwayInvoiceId,
                    CompanyId = newInvoice.CompanyId,
                    Status=newInvoice.ValidStatus
                });
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-UpdateInvoice-Exception", ex);
                throw;
            }
        }
        public void DeleteInvoice(int invoiceId)
        {
            try
            {
                InvoiceDAL invoiceDALObj = new InvoiceDAL();
                invoiceDALObj.DeleteInvoice(invoiceId);
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-DeleteInvoice-Exception", ex);
            }
        }

        public List<CustomerWiseDetails> GetInvoiceDetailsforEachCustomer()
        {
            try
            {
                var invoices = GetInvoiveList().
                    Where(x=>string.IsNullOrEmpty(x.LayAwayInvoiceId)).
                    GroupBy(x => new { x.CustomerId }).Select(x => new CustomerWiseDetails {
                        CustomerId = x.First().CustomerId,
                        CustomerName = x.First().Customer,
                        NumberOfInvoices = x.Count(),
                        TotalAmount = x.Sum(c => decimal.Parse(c.TotalPaid)).ToString() }).ToList();                
                return invoices;
            }

            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-GetInvoiceDetailsforEachCustomer-Exception", ex);
                throw;
            }

        }
        public List<InvoiceListViewModel> GetInvoicesByLayawayInvoiceId(string layawayInvoiceId)
        {
            try
            {
                InvoiceDAL invoiceDALObj = new InvoiceDAL();
                var invoicedetails = invoiceDALObj.GetInvoicesByLayawayInvoiceId(layawayInvoiceId);
                if (invoicedetails != null)
                {
                    List<InvoiceListViewModel> listInvoiceModel = new List<InvoiceListViewModel>();
                    foreach (var item in invoicedetails)
                    {
                        var Obj1 = new InvoiceListViewModel
                        {
                            InvoiceId = item.Id,
                            CustomerId = item.CustomerId,
                            Total = item.SubTotal,
                            Balance = item.Balance,
                            Cash = item.Cash,
                            CreditCard = item.CreditCard,
                            Check = item.Cheque,
                            TotalPaid = item.TotalAmount,
                            UserId = item.UserId.ToString(),
                            InventoryStatusId = item.InventoryStatusId,
                            TaxType = item.TaxType,
                            LayAwayInvoiceId = item.LayAwayInvoiceId,
                            InvoiceDate = item.CreatedAt.ToShortDateString(),
                            CompanyId = item.CompanyId,
                            CreatedAt = item.CreatedAt,
                            ValidStatus=item.Status
                        };
                        listInvoiceModel.Add(Obj1);
                    }
                    return listInvoiceModel;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceBAL-GetInvoicesByLayawayInvoiceId-Exception", ex);
                return new List<InvoiceListViewModel>();
            }
        }
    }

}

