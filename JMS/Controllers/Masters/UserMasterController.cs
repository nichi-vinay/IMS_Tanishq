using JMS.BAL.BussinesLogic;

using JMS.BAL.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using JMS.Models;
using System.Collections.Specialized;
using log4net;

namespace JMS.Controllers
{
    public class UserMasterController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserMasterController));
        public ActionResult GetUserJsonData(JqueryDatatableParam param)
        {
            try
            {
                var userBALObj = new UserBAL();
                var users = userBALObj.GetAllUsers().Items;

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    users = users.Where(x =>x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                                                  ||x.Name != null && x.Name.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.RoleName != null && x.RoleName.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.Phone != null && x.Phone.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.UserName != null && x.UserName.ToLower().Contains(param.sSearch.ToLower())).ToList();
                }

                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.Id).ToList() : users.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.UserId).ToList() : users.OrderByDescending(c => c.UserId).ToList();
                        break;
                    case 2:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.Name).ToList() : users.OrderByDescending(c => c.Name).ToList();
                        break;
                    case 3:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.RoleName).ToList() : users.OrderByDescending(c => c.RoleName).ToList();
                        break;
                    case 4:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.Phone).ToList() : users.OrderByDescending(c => c.Phone).ToList();
                        break;
                    case 5:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.UserName).ToList() : users.OrderByDescending(c => c.UserName).ToList();
                        break;
                    case 6:
                        users = sortDirection == "asc" ? users.OrderBy(c => c.StatusName).ToList() : users.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = users.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? users.Count : param.iDisplayLength).ToList();

                var totalRecords = users.Count;

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
                Log.Error("UserMasterController-GetUserJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Index()
        {
            try
            {

                var userViewModel = new UserViewModel
                {
                    StatusList = HelperBAL.StatusList,
                    RolesList = HelperBAL.GetRolesList()
                };

                return View(userViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("UserMasterController-Index-Exception", ex);
                return null;
            }
        }

        [HttpPost]
        public ActionResult UserChangePassword(UsersModel model)
        {
            try
            {
                var userBALObj = new UserBAL();
                userBALObj.ChangeUserPassword(model.Id, model.Password);

                return Json(new { data = "Password Changed" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("UserMasterController-UserChangePassword-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult DeleteUser(int Id)
        {
            try
            {
                var userBALObj = new UserBAL();
                userBALObj.DeleteUser(Id);

                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("UserMasterController-DeleteUser-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);

            }
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(UsersModel model)
        {
            var userBALObj = new UserBAL();
            try
            {
                if (userBALObj.GetUserById(model.Id) != null)
                {
                    userBALObj.UpdateUser(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    userBALObj.AddUser(model);
                    return Json(new { data = "Added" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserMasterController-CreateOrUpdate-Exception", ex);
                throw;
            }
        }
    }
}