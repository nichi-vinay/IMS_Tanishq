using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using JMS.Models;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class CustomerMasterController : Controller
    {
        
        private static readonly ILog Log = LogManager.GetLogger(typeof(CustomerMasterController));
        public ActionResult GetCustomerJsonData(JqueryDatatableParam param)
        {
            try
            {
                var customerBALObj = new CustomerBAL();
                var customers = customerBALObj.GetAllCustomers().Items;
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    var encrypted = Helper.encrypt(param.sSearch);
                    customers = customers.Where(x =>x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                                                  ||x.CustomerName!=null && x.CustomerName.ToLower().Contains(param.sSearch.ToLower()) 
                                                  || x.CustomerPhone!=null&& x.CustomerPhone.ToLower().Contains(param.sSearch.ToLower()) 
                                                  || x.Email!=null&& x.Email.ToLower().Contains(param.sSearch.ToLower())
                                                   || x.State != null && x.State.ToLower().Contains(param.sSearch.ToLower())
                                                    || x.City != null && x.City.ToLower().Contains(param.sSearch.ToLower())
                                                     || x.Zip != null && x.Zip.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.DLNumber!=null&& x.DLNumber.ToLower().Contains(encrypted.ToLower())).ToList();
                }


                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.Id).ToList() : customers.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.CustomerName).ToList() : customers.OrderByDescending(c => c.CustomerName).ToList();
                        break;
                    case 2:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.CustomerPhone).ToList() : customers.OrderByDescending(c => c.CustomerPhone).ToList();
                        break;
                    case 3:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.Address).ToList() : customers.OrderByDescending(c => c.Address).ToList();
                        break;
                    case 4:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.DLNumber).ToList() : customers.OrderByDescending(c => c.DLNumber).ToList();
                        break;
                    case 5:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.ExpDate).ToList() : customers.OrderByDescending(c => c.ExpDate).ToList();
                        break;
                    case 6:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.DOB).ToList() : customers.OrderByDescending(c => c.DOB).ToList();
                        break;
                    case 7:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.City).ToList() : customers.OrderByDescending(c => c.City).ToList();
                        break;
                    case 8:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.State).ToList() : customers.OrderByDescending(c => c.State).ToList();
                        break;
                    case 9:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.Email).ToList() : customers.OrderByDescending(c => c.Email).ToList();
                        break;
                    case 10:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.Zip).ToList() : customers.OrderByDescending(c => c.Zip).ToList();
                        break;
                    case 11:
                        customers = sortDirection == "asc" ? customers.OrderBy(c => c.StatusName).ToList() : customers.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = customers.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength==-1?customers.Count:param.iDisplayLength).ToList();

                foreach (var item in displayResult)
                {
                    item.Address = Helper.Decrypt(item.Address);
                    item.DLNumber = Helper.Decrypt(item.DLNumber);
                    item.ExpDate = Helper.Decrypt(item.ExpDate);
                    item.DOB = Helper.Decrypt(item.DOB);
                }

                var totalRecords = customers.Count;

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
                Log.Error("CustomerMasterController-GetCompanyById-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            try
            {
                var customerViewModel = new CustomerViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(customerViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerMasterController-Index-Exception", ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult CreateOrUpdate(CustomerModel model)
        {
            try
            {
                var customerBALObj = new CustomerBAL();
                if (customerBALObj.GetCustomerById(model.Id) != null)
                {
                    customerBALObj.UpdateCustomer(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    customerBALObj.AddCustomer(model);
                    return Json(new { data = "Added" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CustomerMasterController-CreateOrUpdate-Exception", ex);
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteCustomer(int Id)
        {
            try
            {
                var customerBALObj = new CustomerBAL();
                customerBALObj.DeleteCustomer(Id);

                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("CustomerMasterController-DeleteCustomer-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

    }
}