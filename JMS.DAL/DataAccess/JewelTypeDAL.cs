using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class JewelTypeDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JewelTypeDAL));
        public List<JewelType> GetAllJewelTypes()
        {
            try
            {
                // List<RoleModel> roleModels = new List<RoleModel>();
                List<JewelType> _jewelType = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _jewelType = context.JewelTypes.ToList<JewelType>();

                }
                return _jewelType;
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeDAL-GetAllJewelTypes-Exception", ex);
                return null;
            }
        }
        public void AddJewelType(JewelType jewelType)
        {
            try
            {
                JewelType newjewelType= new JewelType
                {

                    JewelTypeName = jewelType.JewelTypeName,
                    Status = jewelType.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.JewelTypes.Add(newjewelType);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeDAL-AddJewelType-Exception", ex);
                return ;
            }
        }

        public JewelType GetJewelTypeById(int id)
        {
            try
            {
                JewelType jewelTypes = new JewelType();
                using (JMSEntities context = new JMSEntities())
                {
                    jewelTypes = context.JewelTypes.FirstOrDefault((jeweltype) => jeweltype.Id == id);
                }
                return jewelTypes;
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeDAL-GetJewelTypeById-Exception", ex);
                return null;
            }
        }
        public void UpdateJewelType(JewelType jewelType)
        {

            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.JewelTypes.FirstOrDefault((e) => e.Id == jewelType.Id);
                    selected.JewelTypeName = jewelType.JewelTypeName;
                    selected.Status = jewelType.Status;
                    selected.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeDAL-UpdateJewelType-Exception", ex);
                new Exception(ex.Message);
            }
        }
        public void DeleteJewelType(int id)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.JewelTypes.FirstOrDefault((e) => e.Id == id);
                    selected.Status = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeDAL-DeleteJewelType-Exception", ex);
                throw;
            }
        }
    }
}
