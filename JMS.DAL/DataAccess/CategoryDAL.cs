using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class CategoryDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryDAL));
        public List<Category> GetAllCategory()
        {
            try
            {
                List<Category> _category = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _category = context.Categories
                    .ToList<Category>();
                }
                return _category;
            }
            catch (Exception ex)
            {
                Log.Error("CategoryDAL-GetAllCategory-Exception", ex);
                return new List<Category>();
            }
        }
        public void AddCategory(Category category)
        {
            try
            {
                Category newCategory = new Category
                {

                    CategoryName = category.CategoryName,
                    CategoryNumber=category.CategoryNumber,
                    Status = true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    MISC1 = null,
                    MISC2 = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Categories.Add(newCategory);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CategoryDAL-AddCategory-Exception", ex);
                throw;
            }
        }

        public Category GetCategoryById(int categoryId)
        {
            try
            {
                Category category = new Category();
                using (JMSEntities content = new JMSEntities())
                {
                    category = content.Categories.FirstOrDefault((categ) => categ.Id == categoryId);
                }

                return category;
            }
            catch (Exception ex)
            {
                Log.Error("CategoryDAL-GetCategoryById-Exception", ex);
                return new Category();
            }
        }

        public void UpdateCategory(Category newCategory)
        {
            try
            {
                Category category = new Category();
                using (JMSEntities content = new JMSEntities())
                {
                    category = content.Categories.FirstOrDefault((categ) => categ.Id == newCategory.Id);
                    category.CategoryName = newCategory.CategoryName;
                    category.CategoryNumber = newCategory.CategoryNumber;
                    category.Status = newCategory.Status;
                    category.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CategoryDAL-UpdateCategory-Exception", ex);
                throw;
            }
        }
        public void DeleteCategory(int id)
        {
            try
            {
                Category category = new Category();
                using (JMSEntities content = new JMSEntities())
                {
                    category = content.Categories.FirstOrDefault((categ) => categ.Id == id);
                    category.Status = false;
                    category.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("CategoryDAL-DeleteCategory-Exception", ex);
                throw;
            }
        }
    }
}
