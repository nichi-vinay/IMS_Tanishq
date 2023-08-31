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
    public class SupplierMasterController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(SupplierMasterController));
        public ActionResult GetSupplierJsonData(JqueryDatatableParam param)
        {
            try
            {
                var supplierBALObj = new SupplierBAL();
                var suppliers = supplierBALObj.GetAllSuppliers().Items;
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    suppliers = suppliers.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                                                  || x.SupplierName != null && x.SupplierName.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.SupplierCode != null && x.SupplierCode.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.Phone != null && x.Phone.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.Address != null && x.Address.ToLower().Contains(param.sSearch.ToLower())).ToList();
                }

                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        suppliers = sortDirection == "asc" ? suppliers.OrderBy(c => c.Id).ToList() : suppliers.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        suppliers = sortDirection == "asc" ? suppliers.OrderBy(c => c.SupplierName).ToList() : suppliers.OrderByDescending(c => c.SupplierName).ToList();
                        break;
                    case 2:
                        suppliers = sortDirection == "asc" ? suppliers.OrderBy(c => c.SupplierCode).ToList() : suppliers.OrderByDescending(c => c.SupplierCode).ToList();
                        break;
                    case 3:
                        suppliers = sortDirection == "asc" ? suppliers.OrderBy(c => c.Address).ToList() : suppliers.OrderByDescending(c => c.Address).ToList();
                        break;
                    case 4:
                        suppliers = sortDirection == "asc" ? suppliers.OrderBy(c => c.Phone).ToList() : suppliers.OrderByDescending(c => c.Phone).ToList();
                        break;
                    case 5:
                        suppliers = sortDirection == "asc" ? suppliers.OrderBy(c => c.StatusName).ToList() : suppliers.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = suppliers.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? suppliers.Count : param.iDisplayLength).ToList();

                var totalRecords = suppliers.Count;

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
                Log.Error("SupplierMasterController-GetSupplierJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            try
            {
                var supplierViewModel = new SupplierViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(supplierViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("SupplierMasterController-Index-Exception", ex);
                return null;
            }
        }
        [HttpPost]
        public ActionResult CreateOrUpdate(SupplierModel model)
        {
            try
            {
                var supplierBALObj = new SupplierBAL();
                if (supplierBALObj.GetSupplierById(model.Id) != null)
                {
                    supplierBALObj.UpdateSupplier(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    supplierBALObj.AddSupplier(model);
                    return Json(new { data = "Added" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("SupplierMasterController-CreateOrUpdate-Exception", ex);
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteSupplier(int Id)
        {
            try
            {
                var supplierBALObj = new SupplierBAL();
                supplierBALObj.DeleteSupplier(Id);

                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("SupplierMasterController-DeleteSupplier-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}