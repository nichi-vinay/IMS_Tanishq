
using JMS.BAL.ViewModel;
using JMS.Common.Helper;
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
    public class UserBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserBAL));
        public UserViewModel GetAllUsers()
        {
            try
            {
                UserViewModel userViewModel = new UserViewModel();
                List<UsersModel> usersModels = new List<UsersModel>();
                var userDALObj = new UserDAL();
                var lstUsers = userDALObj.GetAllUsers();
                var roleDALObj = new RoleDAL();
                var roledetails = roleDALObj.GetAllRole();
                
                foreach (var item in lstUsers)
                {
                    var obj = new UsersModel()
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        RoleId = item.RoleId,
                        RoleName = roledetails.SingleOrDefault(roleid => roleid.Id == item.RoleId).RoleName,
                        Name = item.Name,
                        Phone = item.Phone,
                        UserName = item.UserName,
                        Password=item.Password,
                        Status=item.Status,
                        StatusName = item.Status == true ? "Active" : "In-Active",
               
                    };
                    usersModels.Add(obj);
                }
                userViewModel.Items = usersModels;
               
                return userViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("UserBAL-GetAllUsers-Exception", ex);
                return null;
            }
        }
        public void AddUser(UsersModel userModel)
        {
            try
            {
                var userDALObj = new UserDAL();
                userDALObj.AddUser(new User()
                {
                    
                    Name = userModel.Name,
                    Phone = userModel.Phone,
                    UserName = userModel.UserName,
                    UserId = userModel.UserId,
                    RoleId = userModel.RoleId,
                    Password= Helper.encrypt(userModel.Password),
                    Status = userModel.Status,
                });
            }
            catch (Exception ex)
            {
                Log.Error("UserBAL-AddUser-Exception", ex);
                throw;
            }
        }
        public UsersModel GetUserById(int UserId)
        {

            try
            {
                var userDALObj = new UserDAL();
                var userDetails = userDALObj.GetUserById(UserId);
                if (userDetails != null)
                {
                    UsersModel userModel = new UsersModel()
                    {
                        Id = userDetails.Id,
                        Name = userDetails.Name,
                        Phone = userDetails.Phone,
                        UserName = userDetails.UserName,
                        UserId = userDetails.UserId,
                        RoleId = userDetails.RoleId,
                        Status = userDetails.Status,
                    };

                    return userModel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserBAL-GetUserById-Exception", ex);
                return null;
            }
        }
        public void UpdateUser(UsersModel userModel)
        {
            try
            {
                var userDALObj = new UserDAL();
                userDALObj.UpdateUser(new User()
                {
                    Id = userModel.Id,
                    Name = userModel.Name,
                    Phone = userModel.Phone,
                    UserName = userModel.UserName,
                    UserId = userModel.UserId,
                    RoleId = userModel.RoleId,
                    Status = userModel.Status,
                });
            }
            catch (Exception ex)
            {
                Log.Error("UserBAL-UpdateUser-Exception", ex);
                throw;
            }
        }
        public void DeleteUser(int id)
        {
            try
            {
                var userDALObj = new UserDAL();
                userDALObj.DeleteUser(id);
            }
            catch (Exception ex)
            {
                Log.Error("UserBAL-DeleteUser-Exception", ex);
                throw;
            }
        }
        public void ChangeUserPassword(int id,string password)
        {
            try
            {
                var userDALObj = new UserDAL();
                userDALObj.ChangeUserPassword(id, Helper.encrypt(password));
            }
            catch (Exception ex)
            {
                Log.Error("UserBAL-ChangeUserPassword-Exception", ex);
                throw;
            }
        }

    }
}
