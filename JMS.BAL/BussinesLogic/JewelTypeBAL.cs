
using JMS.BAL.ViewModel;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.BussinesLogic
{
    public class JewelTypeBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(JewelTypeBAL));
        public JewelTypeViewModel GetAllJewelTypes()
        {
            try
            {
                JewelTypeViewModel jewelTypeViewModel = new JewelTypeViewModel();
                List<JewelTypeModel> jewelModels = new List<JewelTypeModel>();
                var JewelTypeDalObj = new JewelTypeDAL();
                var lstRoles = JewelTypeDalObj.GetAllJewelTypes();
                foreach (var item in lstRoles)
                {
                    var obj = new JewelTypeModel()
                    {
                        Id = item.Id,
                        JewelTypeName = item.JewelTypeName,
                        StatusName = item.Status == true ? "Active" : "In-Active",
                    };
                    jewelModels.Add(obj);
                }
                jewelTypeViewModel.Items = jewelModels;
                return jewelTypeViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeBAL-GetAllJewelTypes-Exception", ex);
                return null;
            }
        }
        public void AddJewelType(JewelTypeModel jewelTypeModel)
        {
            try
            {
                var JewelTypeDALobj = new JewelTypeDAL();
                JewelTypeDALobj.AddJewelType(new JewelType()
                {
                    JewelTypeName = jewelTypeModel.JewelTypeName,
                    Status = jewelTypeModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeBAL-AddJewelType-Exception", ex);
                throw;
            }
        }
        public JewelTypeModel GetJewelTypeById(int jewelTypeId)
        {

            try
            {
                var JewelTypeDALObj = new JewelTypeDAL();
                var jewelTypeDetails = JewelTypeDALObj.GetJewelTypeById(jewelTypeId);
                if (jewelTypeDetails != null)
                {
                    JewelTypeModel jewelTypeModel = new JewelTypeModel()
                    {
                        Id = jewelTypeDetails.Id,
                        JewelTypeName = jewelTypeDetails.JewelTypeName,
                        Status = jewelTypeDetails.Status
                    };

                    return jewelTypeModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeBAL-FindJewelTypeById-Exception", ex);
                return null;
            }
        }
        public void UpdateJewelType(JewelTypeModel jewelTypeModel)
        {
            try
            {
                var jewelTypeDALObj = new JewelTypeDAL();
                jewelTypeDALObj.UpdateJewelType(new JewelType()
                {
                    Id = jewelTypeModel.Id,
                    JewelTypeName = jewelTypeModel.JewelTypeName,
                    Status = jewelTypeModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeBAL-UpdateJewelType-Exception", ex);
                throw;
            }
        }
        public void DeleteJewelType(int id)
        {
            try
            {
                var jewelTypeDALObj = new JewelTypeDAL();
                jewelTypeDALObj.DeleteJewelType(id);
            }
            catch (Exception ex)
            {
                Log.Error("JewelTypeBAL-DeleteJewelType-Exception", ex);
                throw;
            }
        }
    }
}
