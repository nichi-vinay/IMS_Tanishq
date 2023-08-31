using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Models;
using log4net;
using System;
using System.Linq;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class CategoryMasterController : Controller
    {
        // GET: CategoryController 
        private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryMasterController));
        
        public ActionResult Index()
        {
            try
            {
                
                var categoryViewModel = new CategoryViewModel
                {
                    StatusList = HelperBAL.StatusList
                };
                return View(categoryViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("CategoryMasterController-Index-Exception", ex);
                return null;
            }
        }
        public ActionResult GetCategoryJsonData(JqueryDatatableParam param)
        {
            try
            {
                var categoryBalObj = new CategoryBAL();
                var categories = categoryBalObj.GetAllCategory().Items;

                if (!string.IsNullOrEmpty(param.sSearch))
                {

                    categories = categories.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())
                                                  || x.CategoryName != null && x.CategoryName.ToLower().Contains(param.sSearch.ToLower())
                                                  || x.CategoryNumber != null && x.CategoryNumber.ToLower().Contains(param.sSearch.ToLower())).ToList();
                }
                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                switch (sortColumnIndex)
                {
                    case 0:
                        categories = sortDirection == "asc" ? categories.OrderBy(c => c.Id).ToList() : categories.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        categories = sortDirection == "asc" ? categories.OrderBy(c => c.CategoryName).ToList() : categories.OrderByDescending(c => c.CategoryName).ToList();
                        break;
                    case 2:
                        categories = sortDirection == "asc" ? categories.OrderBy(c => c.CategoryNumber).ToList() : categories.OrderByDescending(c => c.CategoryNumber).ToList();
                        break;
                    case 3:
                        categories = sortDirection == "asc" ? categories.OrderBy(c => c.StatusName).ToList() : categories.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = categories.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? categories.Count : param.iDisplayLength).ToList();

                var totalRecords = categories.Count;

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
                Log.Error("CategoryMasterController-GetCategoryJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult Index(CategoryViewModel model)
        {

            return Index();
        }

        [HttpPost]
        public ActionResult CreateOrUpdate(CategoryModel model)
        {
            try
            {
                var categoryBalObj = new CategoryBAL();
                if (categoryBalObj.GetCategoryById(model.Id) != null)
                {
                    categoryBalObj.UpdateCategory(model);
                    return Json(new { data = "Updated" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    categoryBalObj.AddCategory(model);
                    return Json(new { data = "Created" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("CategoryMasterController-Exception-CreateOrUpdate", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult DeleteCategory(int Id)
        {
            try
            {
                var categoryBalObj = new CategoryBAL();
                categoryBalObj.DeleteCategory(Id);
                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("CategoryMasterController-DeleteCategory-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
