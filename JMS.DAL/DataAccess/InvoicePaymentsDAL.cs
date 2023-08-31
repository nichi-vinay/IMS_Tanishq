using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class InvoicePaymentsDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoicePaymentsDAL));
        public List<InvoicePayment> GetAllInvoicePayments()
        {
            try
            {
                List<InvoicePayment> _invoicePayments = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _invoicePayments = context.InvoicePayments.ToList<InvoicePayment>();

                }
                return _invoicePayments;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsDAL-GetAllInvoicePayments-Exception", ex);
                return null;
            }
        }
        public string AddInvoicePayments(InvoicePayment invoicePayment)
        {
            try
            {

                var invoicePaymentsId = "";
                //InvoicePayment newInvoicePayment = new InvoicePayment
                //{

                //    InvoiceId = invoicePayment.InvoiceId,
                //    Cash = invoicePayment.Cash,
                //    CreditCard = invoicePayment.CreditCard,
                //    Cheque = invoicePayment.Cheque,
                //    PaymentAmount = invoicePayment.PaymentAmount,
                //    NewInvoiceId=invoicePayment.NewInvoiceId,

                //    CreatedAt = DateTime.Now,
                //    UpdatedAt = null,
                //    MISC1 = null,
                //    MISC2 = null
                //};

                using (JMSEntities context = new JMSEntities())
                {
                    context.InvoicePayments.Add(invoicePayment);
                    context.SaveChanges();
                    invoicePaymentsId = invoicePayment.Id.ToString();
                }
                return invoicePaymentsId;
            }
            catch (Exception ex)
            {
                
                Log.Error("InvoicePaymentsDAL-AddInvoicePayments-Exception", ex);
                throw;
            }
        }
        public InvoicePayment GetinvoicePaymentId(int invoicePaymentId)
        {
            try
            {
                InvoicePayment invoicePayment = new InvoicePayment();
                using (JMSEntities content = new JMSEntities())
                {
                    invoicePayment = content.InvoicePayments.FirstOrDefault((paymentId) => paymentId.Id == invoicePaymentId);
                }

                return invoicePayment;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsDAL-GetinvoicePaymentId-Exception", ex);
                return new InvoicePayment();
            }
        }
        public void UpdateInvoicePayments(InvoicePayment newInvoicePayment)
        {
            try
            {
                //InvoicePayment invoicePayment = new InvoicePayment();
                using (JMSEntities content = new JMSEntities())
                {
                    var invoicePayment = content.InvoicePayments.FirstOrDefault((payment) => payment.Id == newInvoicePayment.Id);
                    if (invoicePayment != null)
                    {
                        invoicePayment.InvoiceId = newInvoicePayment.InvoiceId;
                        invoicePayment.Cash = newInvoicePayment.Cash;
                        invoicePayment.CreditCard = newInvoicePayment.CreditCard;
                        invoicePayment.Cheque = newInvoicePayment.Cheque;
                        invoicePayment.PaymentAmount = newInvoicePayment.PaymentAmount;
                        invoicePayment.NewInvoiceId = newInvoicePayment.NewInvoiceId;
                        invoicePayment.UpdatedAt = DateTime.Now;
                        content.SaveChanges();
                    }


                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsDAL-UpdateInvoicePayments-Exception", ex);
                throw;
            }
        }
        public void DeleteInvoicePayments(int id)
        {
            try
            {
                InvoicePayment invoicePayment = new InvoicePayment();
                using (JMSEntities content = new JMSEntities())
                {
                    invoicePayment = content.InvoicePayments.FirstOrDefault((payment) => payment.Id == id);
                    //invoicePayment.Status = false;
                    invoicePayment.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsDAL-DeleteInvoicePayments-Exception", ex);
                throw;
            }
        }
        public List<InvoicePayment> GetInvoicePaymentDetailsByInvoiceId(int? InvoiceId)
        {
            try
            {
                List<InvoicePayment> invoicePayment = new List<InvoicePayment>();
                using (JMSEntities content = new JMSEntities())
                {
                    invoicePayment = content.InvoicePayments.Where((x) => x.InvoiceId == InvoiceId).ToList<InvoicePayment>();
                }
                return invoicePayment;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsDAL-GetInvoiceDetailsByInvoiceId-Exception", ex);
                return new List<InvoicePayment>();
            }
        }

        public List<InvoicePayment> GetInvoicePaymentDetailsByNewInvoiceId(int? newInvoiceId)
        {
            try
            {
                List<InvoicePayment> invoicePayment = new List<InvoicePayment>();
                using (JMSEntities content = new JMSEntities())
                {
                    invoicePayment = content.InvoicePayments.Where((x) => x.NewInvoiceId == newInvoiceId.ToString()).ToList<InvoicePayment>();
                }
                return invoicePayment;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsDAL-GetInvoiceDetailsByInvoiceId", ex);
                return new List<InvoicePayment>();
            }
        }
    }
}

