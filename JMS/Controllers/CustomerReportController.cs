using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class CustomerReportController : Controller
    {
        //GET: CustomerReport
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomerReportController));
        public ActionResult Index()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                Log.Error("CustomerReportController-Index-Exception", ex);
                throw;
            }
        }
        public ActionResult GetCustomerReportJsonData(JqueryDatatableParam param)
        {
            try
            {

                var customerBALObj = new CustomerBAL();
                var invoiceBALObj = new InvoiceBAL();
                var invoices = invoiceBALObj.GetInvoiveList().Where(x => string.IsNullOrEmpty(x.LayAwayInvoiceId)).ToList();
                var customerWiseDetails = invoiceBALObj.GetInvoiceDetailsforEachCustomer();

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    customerWiseDetails = customerWiseDetails.Where(x => x.CustomerId.ToString() != null && x.CustomerId.ToString().ToLower().Contains(param.sSearch.ToLower())
                                                  || x.CustomerName != null && x.CustomerName.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.NumberOfInvoices.ToString() != null && x.NumberOfInvoices.ToString().ToLower().Contains(param.sSearch.ToLower())
                                                  || x.TotalAmount != null && x.TotalAmount.ToLower().Contains(param.sSearch.ToLower())).ToList();

                }

                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        customerWiseDetails = sortDirection == "asc" ? customerWiseDetails.OrderBy(c => c.CustomerId).ToList() : customerWiseDetails.OrderByDescending(c => c.CustomerId).ToList();
                        break;
                    case 1:
                        customerWiseDetails = sortDirection == "asc" ? customerWiseDetails.OrderBy(c => c.CustomerName).ToList() : customerWiseDetails.OrderByDescending(c => c.CustomerName).ToList();
                        break;
                    case 2:
                        customerWiseDetails = sortDirection == "asc" ? customerWiseDetails.OrderBy(c => c.NumberOfInvoices).ToList() : customerWiseDetails.OrderByDescending(c => c.NumberOfInvoices).ToList();
                        break;
                    case 3:
                        customerWiseDetails = sortDirection == "asc" ? customerWiseDetails.OrderBy(c => c.TotalAmount).ToList() : customerWiseDetails.OrderByDescending(c => c.TotalAmount).ToList();
                        break;
                }

                var displayResult = customerWiseDetails.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? customerWiseDetails.Count : param.iDisplayLength).ToList();

                foreach (var items in displayResult)
                {
                    float total = 0;
                    var customerInvoice = invoices.Where(x => x.CustomerId == items.CustomerId).ToList();
                   
              
                    foreach (var customerItems in customerInvoice)
                    {
                       
                      
                        total = total + float.Parse(customerItems.Total);
                        items.TotalAmount = "$"+total.ToString("#0.00");
                    }
                }

                var totalRecords = customerWiseDetails.Count;

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
                Log.Error("CustomerReportController-GetCustomerReportJsonData-Exception,", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}