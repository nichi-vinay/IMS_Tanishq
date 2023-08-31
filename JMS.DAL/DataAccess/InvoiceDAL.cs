using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class InvoiceDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceDAL));
        public List<InvoiceCustomization> GetInvoiveList()
        {
            try
            {
                List<InvoiceCustomization> _invoice = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _invoice = context.Invoices.Select(x => new InvoiceCustomization
                    {
                        CreatedAt = x.CreatedAt,
                        Id = x.Id,
                        Customer = x.CustomerId==null?"" : x.CustomerId ==0?"": x.Customer.CustomerName,
                        CustomerId=x.CustomerId,
                        UserId = x.UserId,
                        SubTotal = x.SubTotal,
                        InventoryStatusId = x.InventoryStatusId,
                        Balance = x.Balance,
                        Status=x.Status,
                        Cheque=x.Cheque,
                        CreditCard=x.CreditCard,
                        Cash=x.Cash,
                        TotalAmount=x.TotalAmount,
                        CompanyId=x.CompanyId,
                        LayAwayInvoiceId=x.LayAwayInvoiceId,
                        UpdatedAt=x.UpdatedAt,
                        TaxType=x.TaxType,
                        InventoryStatusName=x.InventoryStatu.InventoryStatusName,
                        Employee=x.User.Name
                    })/*.Where(x=>x.Status==true)*/.ToList();

                }
                return _invoice;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-GetInvoiveList-Exception", ex);
                throw new Exception(ex.Message);

            }
        }
        public List<Invoice> GetAllInvoices()
        {
            try
            {
                List<Invoice> _invoices = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _invoices = context.Invoices.Where(x=>x.Status==true)
                    .ToList<Invoice>();
                }
                return _invoices;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-GetAllInvoices-Exception", ex);
                return new List<Invoice>();
            }
        }
        public Invoice GetInvoiceById(int? id)
        {
            try
            {
                Invoice invoices = new Invoice();
                using (JMSEntities content = new JMSEntities())
                {
                    invoices = content.Invoices.FirstOrDefault((invoice) => invoice.Id == id);
                }

                return invoices;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-GetInvoiceById-Exception", ex);
                return new Invoice();
            }
        }
        public string AddInvoice(Invoice invoice)
        {
            try
            {
                string insertedId = "";
                using (JMSEntities content = new JMSEntities())
                {
                    content.Invoices.Add(invoice);
                    content.SaveChanges();
                    insertedId = invoice.Id.ToString();
                }
                return insertedId;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-AddInvoice-Exception", ex);
                return "";
            }
        }
        public void UpdateInvoice(Invoice invoice)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Invoices.FirstOrDefault((invoices) => invoices.Id == invoice.Id);
                    if (selected != null)
                    {
                        selected.Balance = invoice.Balance;
                        selected.Customer = invoice.Customer;
                        selected.CustomerId = invoice.CustomerId;
                        selected.User = invoice.User;
                        selected.UserId = invoice.UserId;
                        selected.LayAwayInvoiceId = invoice.LayAwayInvoiceId;
                        selected.CompanyId = invoice.CompanyId;
                        selected.SubTotal = invoice.SubTotal;
                        selected.Cash = invoice.Cash;
                        selected.CreditCard = invoice.CreditCard;
                        selected.Cheque = invoice.Cheque;
                        selected.TotalAmount = invoice.TotalAmount;
                        selected.TaxType = invoice.TaxType;
                        selected.InventoryStatusId = invoice.InventoryStatusId;
                        selected.UpdatedAt = DateTime.Now;
                        selected.Status = invoice.Status;
                        context.SaveChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-UpdateInvoice-Exception", ex);
                new Exception(ex.Message);
            }
        }
        public void DeleteInvoice(int invoiceId)
        {

            try
            {
                using (JMSEntities content = new JMSEntities())
                {
                    var selected = content.Invoices.FirstOrDefault((invoice) => invoice.Id == invoiceId);
                    //content.Invoices.Remove(selected);
                    selected.Status = false;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-DeleteInvoice-Exception", ex);
                
            }
        }
        public  List<Invoice> GetInvoicesByLayawayInvoiceId(string layawayInvoiceId)
        {
            try
            {
                List<Invoice> invoice = new List<Invoice>();
                using (JMSEntities content = new JMSEntities())
                {
                    invoice = content.Invoices.Where((x) => x.LayAwayInvoiceId == layawayInvoiceId && x.Status==true).ToList<Invoice>();
                }
                return invoice;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceDAL-GetInvoicesByLayawayInvoiceId-Exception", ex);
                return new List<Invoice>();
            }
        }
    }
}

