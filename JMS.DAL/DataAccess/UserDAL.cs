using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class UserDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(UserDAL));
        public List<User> GetAllUsers()
        {
            try
            {
                List<User> _users = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _users = context.Users.ToList();
                    _users = context.Users.ToList<User>();
                }
                return _users;
            }
            catch (Exception ex)
            {
                Log.Error("UserDAL-GetAllUsers-Exception", ex);
                return null;
            }
        }

        public void AddUser(User user)
        {
            try
            {
                User newUser = new User
                {

                    UserId = user.UserId,
                    RoleId = user.RoleId,
                    Name = user.Name,
                    Phone = user.Phone,
                    UserName = user.UserName,
                    Password = user.Password,
                    Status = user.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Users.Add(newUser);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserDAL-AddUser-Exception", ex);
                throw;
            }
        }

        public User GetUserById(int id)
        {
            try
            {
                User users = new User();
                using (JMSEntities context = new JMSEntities())
                {
                    users = context.Users.FirstOrDefault((user) => user.Id == id);
                }
                return users;
            }
            catch (Exception ex)
            {
                Log.Error("UserDAL-GetUserById-Exception", ex);
                return null;
            }
        }

        public void UpdateUser(User newUser)
        {
            try
            {

                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Users.FirstOrDefault((users) => users.Id == newUser.Id);
                    selected.Name = newUser.Name;
                    selected.Phone = newUser.Phone;
                    selected.UserName = newUser.UserName;
                    selected.UserId = newUser.UserId;
                    selected.RoleId = newUser.RoleId;
                    selected.Status = newUser.Status;
                    selected.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserDAL-UpdateUser-Exception", ex);
                throw;
            }
        }

        public void DeleteUser(int id)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Users.FirstOrDefault((user) => user.Id == id);
                    selected.Status = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserDAL-DeleteUser-Exception", ex);
                throw;
            }
        }
        public void ChangeUserPassword(int id, string password)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Users.FirstOrDefault((user) => user.Id == id);
                    selected.Password = password;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("UserDAL-ChangeUserPassword-Exception", ex);
                throw;
            }
        }
    }
}

