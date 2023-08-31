using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;
using JMS.Models;
using System.Linq;
using log4net;

namespace JMS.Controllers
{
    public class CompanyMasterController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CompanyMasterController));
      
        public ActionResult Index()
        {
            try
            {
                var companyViewModel = new CompanyViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(companyViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CompanyMasterController-Index-Exception", ex);
                return null;//To-do: Redirect to Error Page 
            }
        }
        public ActionResult GetCompanyJsonData(JqueryDatatableParam param)
        {
            try
            {
                var companyBalObj = new CompanyBAL();
                var companies = companyBalObj.GetAllCompany().Items;

                if (!string.IsNullOrEmpty(param.sSearch))
                {

                    companies = companies.Where(x => x.Id.ToString().ToLower() != null && x.CompanyName.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.Id.ToString().ToLower()!=null&&x.Id.ToString().ToLower().Contains( param.sSearch.ToLower())).ToList();
                }

                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        companies = sortDirection == "asc" ? companies.OrderBy(c => c.Id).ToList() : companies.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        companies = sortDirection == "asc" ? companies.OrderBy(c => c.CompanyName).ToList() : companies.OrderByDescending(c => c.CompanyName).ToList();
                        break;
                    case 2:
                        companies = sortDirection == "asc" ? companies.OrderBy(c => c.StatusName).ToList() : companies.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = companies.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? companies.Count : param.iDisplayLength).ToList();

                var totalRecords = companies.Count;

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
                Log.Error("CompanyMasterController-GetCompanyJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CreateOrUpdate(CompanyModel model)
        {
            try
            {
                var companyBalObj = new CompanyBAL();
                if (companyBalObj.GetCompanyById(model.Id) != null)
                {
                    companyBalObj.UpdateCompany(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    companyBalObj.AddCompany(model);
                    return Json(new { data = "Created" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CompanyMasterController-CreateOrUpdate-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteCompany(int Id)
        {
            try
            {
                var companyBalObj = new CompanyBAL();
                companyBalObj.DeleteCompany(Id);
                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("CompanyMasterController-DeleteCompany-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}