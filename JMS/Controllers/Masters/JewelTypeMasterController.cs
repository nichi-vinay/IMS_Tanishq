using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Models;
using log4net;
using System;
using System.Linq;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class JewelTypeMasterController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JewelTypeMasterController));
        public ActionResult Index()
        {
            try
            {
                var jeweTypeModel = new JewelTypeViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(jeweTypeModel);
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeMasterController-Index-Exception", ex);
                return null;
            }
        }
        public ActionResult GetJewelTypeJsonData(JqueryDatatableParam param)
        {
            try
            {
                var jewelTypeObj = new JewelTypeBAL();
                var jewelTypes = jewelTypeObj.GetAllJewelTypes().Items;
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    jewelTypes = jewelTypes.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || x.JewelTypeName != null && x.JewelTypeName.ToLower().Contains(param.sSearch.ToLower())).ToList();

                }

                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        jewelTypes = sortDirection == "asc" ? jewelTypes.OrderBy(c => c.Id).ToList() : jewelTypes.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        jewelTypes = sortDirection == "asc" ? jewelTypes.OrderBy(c => c.JewelTypeName).ToList() : jewelTypes.OrderByDescending(c => c.JewelTypeName).ToList();
                        break;
                    case 2:
                        jewelTypes = sortDirection == "asc" ? jewelTypes.OrderBy(c => c.StatusName).ToList() : jewelTypes.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = jewelTypes.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? jewelTypes.Count : param.iDisplayLength).ToList();

                var totalRecords = jewelTypes.Count;

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
                Log.Error("JewelTypeMasterController-GetJewelTypeJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(JewelTypeModel model)
         {
            try
            {
                var jewelTypeBALObj = new JewelTypeBAL();
                if (jewelTypeBALObj.GetJewelTypeById(model.Id) != null)
                {
                    jewelTypeBALObj.UpdateJewelType(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    jewelTypeBALObj.AddJewelType(model);
                    return Json(new { data = "Added" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeMasterController-CreateOrUpdate-Exception", ex);
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteJewelType(int Id)
        {
            try
            {
                var jewelTypeBALObj = new JewelTypeBAL();
                jewelTypeBALObj.DeleteJewelType(Id);

                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeMasterController-DeleteJewelType-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }
    }
}