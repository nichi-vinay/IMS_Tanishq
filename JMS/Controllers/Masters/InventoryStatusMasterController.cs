using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Models;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class InventoryStatusMasterController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryStatusMasterController));
  
        public ActionResult Index()
        {
            try
            {
                InventoryStatusViewModel inventoryStatusViewModel = new InventoryStatusViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(inventoryStatusViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusMasterController-Index-Exception",ex);
                return null;
            }
        }

        public ActionResult GetInventoryStatusJsonData(JqueryDatatableParam param)
        {
            try
            {
                var inventoryStatusBALObj = new InventoryStatusBAL();
                var inventoryStatus = inventoryStatusBALObj.GetAllInventoryStatus().Items;

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    inventoryStatus = inventoryStatus.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                                || x.InventoryStatusName != null && x.InventoryStatusName.ToLower().Contains(param.sSearch.ToLower())).ToList();

                }
                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        inventoryStatus = sortDirection == "asc" ? inventoryStatus.OrderBy(c => c.Id).ToList() : inventoryStatus.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        inventoryStatus = sortDirection == "asc" ? inventoryStatus.OrderBy(c => c.InventoryStatusName).ToList() : inventoryStatus.OrderByDescending(c => c.InventoryStatusName).ToList();
                        break;
                    case 2:
                        inventoryStatus = sortDirection == "asc" ? inventoryStatus.OrderBy(c => c.StatusName).ToList() : inventoryStatus.OrderByDescending(c => c.StatusName).ToList();
                        break;                    
                }

                var displayResult = inventoryStatus.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? inventoryStatus.Count : param.iDisplayLength).ToList();

                var totalRecords = inventoryStatus.Count;

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
                Log.Error("InventoryStatusMasterController-GetInventoryStatusJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(InventoryStatusModel model)
        {
            try
            {
                var inventoryStatusBALObj = new InventoryStatusBAL();
                if (inventoryStatusBALObj.GetInventoryStatusById(model.Id) != null)
                {
                    inventoryStatusBALObj.UpdateInventoryStatus(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    inventoryStatusBALObj.AddInventoryStatus(model);
                    return Json(new { data = "Created" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusMasterController-CreateOrUpdate-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteInventoryStatus(int Id)
        {
            try
            {
                var inventoryStatusBALObj = new InventoryStatusBAL();
                inventoryStatusBALObj.DeleteInventoryStatus(Id);
                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusMasterController-DeleteInventoryStatus-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}