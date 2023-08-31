using System;
using System.Collections.Generic;
using System.Web.Mvc;
using JMS.BAL.ViewModel;
using JMS.BAL.BussinesLogic;
using System.Linq;
using JMS.Models;
using log4net;

namespace JMS.Controllers

{
    public class RoleMasterController : Controller
    {
        // GET: RoleMaster
        private static readonly ILog Log = LogManager.GetLogger(typeof(RoleMasterController));
        public ActionResult GetRoleJsonData(JqueryDatatableParam param)
        {
            try
            {
              
                var roleBALObj = new RoleBAL();
                var roles = roleBALObj.GetAllRoles().Items;
               
                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    roles = roles.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                      || x.RoleName != null && x.RoleName.ToLower().Contains(param.sSearch.ToLower())).ToList();

                }

                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        roles = sortDirection == "asc" ? roles.OrderBy(c => c.Id).ToList() : roles.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        roles = sortDirection == "asc" ? roles.OrderBy(c => c.RoleName).ToList() : roles.OrderByDescending(c => c.RoleName).ToList();
                        break;
                    case 2:
                        roles = sortDirection == "asc" ? roles.OrderBy(c => c.StatusName).ToList() : roles.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = roles.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? roles.Count : param.iDisplayLength).ToList();

                var totalRecords = roles.Count;

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
                Log.Error("RoleMasterController-GetRoleJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
               
            }
        }
        public ActionResult Index()
        {
            try
            {
                var roleVIewModel = new RoleViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(roleVIewModel);
            }
            catch (Exception ex)
            {
                Log.Error("RoleMasterController-Index-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(RoleModel model)
        {
            try
            {
                var roleBALObj = new RoleBAL();
                if (roleBALObj.GetRoleById(model.Id) != null)
                {
                    roleBALObj.UpdateRole(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    roleBALObj.AddRole(model);
                    return Json(new { data = "Added" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("RoleMasterController-CreateOrUpdate-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult DeleteRole(int Id)
        {
            try
            {
                var roleBALObj = new RoleBAL();
                roleBALObj.DeleteRole(Id);

                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("RoleMasterController-DeleteRole-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }


    }
}