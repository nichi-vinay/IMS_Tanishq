using JMS.BAL.ViewModel;
using JMS.DAL.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JMS.DAL;
using log4net;

namespace JMS.BAL.BussinesLogic
{
    public class RoleBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RoleBAL));
        public RoleViewModel GetAllRoles()
        {
            try
            {
                RoleViewModel roleViewModel = new RoleViewModel();
                List<RoleModel> roleModels = new List<RoleModel>();
                var roleDalObj = new RoleDAL();
                var lstRoles = roleDalObj.GetAllRole();
            
                foreach (var item in lstRoles)
                {
                    var obj = new RoleModel()
                    {
                        Id = item.Id,
                        RoleName = item.RoleName,
                        Status = item.Status,
                        StatusName = item.Status == true ? "Active" : "In-Active",
                    };
                    roleModels.Add(obj);
                }
                roleViewModel.Items = roleModels;
                return roleViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("RoleBAL-GetAllRoles-Exception", ex);
                throw new Exception(ex.Message);
            }
        }
        public void AddRole(RoleModel roleModel)
        {
            try
            {
                var userRoleObj = new RoleDAL();
                userRoleObj.AddRole(new Role()
                {
                    RoleName = roleModel.RoleName,
                    Status = roleModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("RoleBAL-AddRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }
        public void UpdateRole(RoleModel roleModel)
        {
            try
            {
                var userRoleObj = new RoleDAL();
                userRoleObj.UpdateRole(new Role()
                {
                    Id = roleModel.Id,
                    RoleName = roleModel.RoleName,
                    Status = roleModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("RoleBAL-UpdateRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }
        public RoleModel GetRoleById(int roleId)
        {
            try
            {
                var roleDALObj = new RoleDAL();
                var roleDetails = roleDALObj.GetRoleById(roleId);
                if (roleDetails != null)
                {
                   RoleModel roleModel = new RoleModel()
                    {
                        Id = roleDetails.Id,
                        RoleName = roleDetails.RoleName,
                        Status = roleDetails.Status
                    };

                    return roleModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("RoleBAL-GetRoleById-Exception", ex);
                throw new Exception(ex.Message);
            }
        }
        public void DeleteRole(int id)
        {
            try
            {
                var roleDALObj = new RoleDAL();
                roleDALObj.DeleteRole(id);
            }
            catch (Exception ex)
            {
                Log.Error("RoleBAL-DeleteRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

    }
}
