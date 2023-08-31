using JMS.BAL.ViewModel;
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
    public class InvoicePaymentsBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoicePaymentsBAL));
        public InvoicePaymentViewModel GetAllInvoicePayments()
        {
            try
            {
                InvoicePaymentViewModel invoicePaymentViewModel = new InvoicePaymentViewModel();
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                List<InvoiocePaymentModel> invoicePaymentModels = new List<InvoiocePaymentModel>();
                var lstOfinvoicePayments = invoicePaymentDALObj.GetAllInvoicePayments();
                foreach (var item in lstOfinvoicePayments)
                {
                    var obj = new InvoiocePaymentModel()
                    {
                        InvoiceId = item.InvoiceId,
                        Cash = item.Cash,
                        CreditCard = item.CreditCard,
                        Cheque = item.Cheque,
                        PaymentAmount = item.PaymentAmount,
                        NewInvoiceId = item.NewInvoiceId,
                    };
                    invoicePaymentModels.Add(obj);
                }
                invoicePaymentViewModel.Items = invoicePaymentModels;
                return invoicePaymentViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-GetAllInvoicePayments-Exception", ex);
                return null;
            }
        }
        public string AddInvoicePayment(InvoiocePaymentModel newInvoicePayment)
        {
            try
            {
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                var invoicePaymentsId = invoicePaymentDALObj.AddInvoicePayments(new InvoicePayment()
                {
                    InvoiceId = newInvoicePayment.InvoiceId,
                    Cash = newInvoicePayment.Cash,
                    CreditCard = newInvoicePayment.CreditCard,
                    Cheque = newInvoicePayment.Cheque,
                    PaymentAmount = newInvoicePayment.PaymentAmount,
                    NewInvoiceId = newInvoicePayment.NewInvoiceId,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    MISC1 = null,
                    MISC2 = null
                });
                return invoicePaymentsId;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-AddInvoicePayment-Exception", ex);
                throw;
            }
        }
        public InvoiocePaymentModel GetinvoicePaymentId(int invoicePaymentId)
        {
            try
            {
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                var dalInvoicePayments = invoicePaymentDALObj.GetinvoicePaymentId(invoicePaymentId);
                if (dalInvoicePayments != null)
                {
                    return new InvoiocePaymentModel()
                    {
                        Id = dalInvoicePayments.Id,
                        InvoiceId = dalInvoicePayments.InvoiceId,
                        Cash = dalInvoicePayments.Cash,
                        CreditCard = dalInvoicePayments.CreditCard,
                        Cheque = dalInvoicePayments.Cheque,
                        PaymentAmount = dalInvoicePayments.PaymentAmount,
                        NewInvoiceId = dalInvoicePayments.NewInvoiceId,
                        CreatedAt = dalInvoicePayments.CreatedAt

                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-GetinvoicePaymentId-Exception", ex);
                return null;
            }
        }
        public void UpdateInvoicePayments(InvoiocePaymentModel newInvoivePayment)
        {
            try
            {
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                invoicePaymentDALObj.UpdateInvoicePayments(new InvoicePayment()
                {
                    Id=newInvoivePayment.Id,
                    InvoiceId = newInvoivePayment.InvoiceId,
                    Cash = newInvoivePayment.Cash,
                    CreditCard = newInvoivePayment.CreditCard,
                    Cheque = newInvoivePayment.Cheque,
                    PaymentAmount = newInvoivePayment.PaymentAmount,
                    NewInvoiceId = newInvoivePayment.NewInvoiceId,
                });
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-UpdateInvoicePayments-Exception", ex);
                throw;
            }
        }
        public void DeleteInvoicePayments(int invoicePaymentId)
        {
            try
            {
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                invoicePaymentDALObj.DeleteInvoicePayments(invoicePaymentId);
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-DeleteInvoicePayments-Exception", ex);
                throw;
            }
        }
        public List<InvoiocePaymentModel> GetInvoicePaymentDetailsByInvoiceId(int invoiceId)
        {
            try
            {
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                var dalInvoicePayments = invoicePaymentDALObj.GetInvoicePaymentDetailsByInvoiceId(invoiceId);
                var invoiceBAL = new InvoiceBAL();
                var balance = invoiceBAL.GetInvoicesById(invoiceId).Balance;
                if (dalInvoicePayments != null)
                {
                    List<InvoiocePaymentModel> listInvoicePaymentModel = new List<InvoiocePaymentModel>();
                    foreach (var item in dalInvoicePayments)
                    {
                        var Obj1 = new InvoiocePaymentModel
                        {
                            Id = item.Id,
                            InvoiceId = item.InvoiceId,
                            Cash = string.IsNullOrEmpty(item.Cash) ? "0.00" : item.Cash,
                            CreditCard = string.IsNullOrEmpty(item.CreditCard) ? "0.00" : item.CreditCard,
                            Cheque = string.IsNullOrEmpty(item.Cheque) ? "0.00" : item.Cheque,
                            PaymentAmount = string.IsNullOrEmpty(item.PaymentAmount) ? "0.00" : item.PaymentAmount,
                            NewInvoiceId = item.NewInvoiceId,
                            Balance = balance,
                            CreatedAt = item.CreatedAt,
                            DisplayDate = item.CreatedAt.ToString("dd/MM/yyyy")
                        };
                        listInvoicePaymentModel.Add(Obj1);
                    }
                    return listInvoicePaymentModel;
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-GetInvoiceDetailsByInvoiceId-Exception", ex);
                return null;
            }
        }

        public InvoiocePaymentModel GetInvoicePaymentDetailsByNewInvoiceId(int newInvoiceId)
        {
            try
            {
                var invoicePaymentDALObj = new InvoicePaymentsDAL();
                var dalInvoicePayments = invoicePaymentDALObj.GetInvoicePaymentDetailsByNewInvoiceId(newInvoiceId);
                var invoiceBAL = new InvoiceBAL();
                var balance = invoiceBAL.GetInvoicesById(newInvoiceId).Balance;
                if (dalInvoicePayments != null)
                {
                    List<InvoiocePaymentModel> listInvoicePaymentModel = new List<InvoiocePaymentModel>();
                    foreach (var item in dalInvoicePayments)
                    {
                        var Obj1 = new InvoiocePaymentModel
                        {
                            Id = item.Id,
                            InvoiceId = item.InvoiceId,
                            Cash = string.IsNullOrEmpty(item.Cash) ? "0.00" : item.Cash,
                            CreditCard = string.IsNullOrEmpty(item.CreditCard) ? "0.00" : item.CreditCard,
                            Cheque = string.IsNullOrEmpty(item.Cheque) ? "0.00" : item.Cheque,
                            PaymentAmount = string.IsNullOrEmpty(item.PaymentAmount) ? "0.00" : item.PaymentAmount,
                            NewInvoiceId = item.NewInvoiceId,
                            Balance = balance,
                            CreatedAt = item.CreatedAt,
                            DisplayDate = item.CreatedAt.ToString("dd/MM/yyyy")
                        };
                        listInvoicePaymentModel.Add(Obj1);
                    }
                    return listInvoicePaymentModel[0];
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("InvoicePaymentsBAL-GetInvoiceDetailsByInvoiceId", ex);
                return null;
            }
        }
    }
}
