using JMS.BAL.ViewModel;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.BussinesLogic
{
    public class CategoryBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryBAL));
        public CategoryViewModel GetAllCategory()
        {
            try
            {
                CategoryViewModel categoryViewModels = new CategoryViewModel();
                var categoryDalObj = new CategoryDAL();
                List<CategoryModel> categoryModels = new List<CategoryModel>();
                var lstOfCategory = categoryDalObj.GetAllCategory();
                foreach (var item in lstOfCategory)
                {
                    var obj = new CategoryModel()
                    {
                        Id = item.Id,
                        CategoryName = item.CategoryName,
                        CategoryNumber = item.CategoryNumber,
                        Status = item.Status,
                        StatusName = item.Status == true ? "Active" : "In-Active"
                    };
                    categoryModels.Add(obj);
                }
                categoryViewModels.Items = categoryModels;
                return categoryViewModels;
            }
            catch (Exception ex)
            {
                Log.Error("CategoryBAL-GetAllCategory-Exception", ex);
                return null;
            }
        }

        public void AddCategory(CategoryModel newCategory)
        {
            try
            {
                var categoryDalObj = new CategoryDAL();
                categoryDalObj.AddCategory(new Category()
                {
                    Id = newCategory.Id,
                    CategoryName = newCategory.CategoryName,
                    CategoryNumber = newCategory.CategoryNumber,
                    Status = newCategory.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("CategoryBAL-AddCategory-Exception", ex);
                throw;
            }
        }
        public void UpdateCategory(CategoryModel newCategory)
        {
            try
            {
                var categoryDalObj = new CategoryDAL();
                categoryDalObj.UpdateCategory(new Category()
                {
                    Id = newCategory.Id,
                    CategoryName = newCategory.CategoryName,
                    CategoryNumber = newCategory.CategoryNumber,
                    Status = newCategory.Status
                });
            }
            catch (Exception  ex)
            {
                Log.Error("CategoryBAL-UpdateCategory-Exception", ex);
                throw;
            }
        }
        public void DeleteCategory(int categoryId)
        {
            try
            {
                var categoryDalObj = new CategoryDAL();
                categoryDalObj.DeleteCategory(categoryId);
            }
            catch (Exception ex)
            {
                Log.Error("CategoryBAL-DeleteCategory-Exception", ex);
                throw;
            }
        }
        public CategoryModel GetCategoryById(int categoryId)
        {
            try
            {
                var categoryDalObj = new CategoryDAL();
                var dalCategory = categoryDalObj.GetCategoryById(categoryId);
                if (dalCategory != null)
                {
                    return new CategoryModel()
                    {
                        Id = dalCategory.Id,
                        CategoryName = dalCategory.CategoryName,
                        CategoryNumber = dalCategory.CategoryNumber,
                        Status = dalCategory.Status
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                Log.Error("CategoryBAL-GetCategoryById-Exception", ex);
                return null;
            }
        }
    }
}
