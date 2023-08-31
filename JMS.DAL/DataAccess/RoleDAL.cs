using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class RoleDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(RoleDAL));
        public List<Role> GetAllRole()
        {
            try
            {
                // List<RoleModel> roleModels = new List<RoleModel>();
                List<Role> _role = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _role = context.Roles.ToList<Role>();

                }
                return _role;
            }
            catch (Exception ex)
            {
                Log.Error("RoleDAL-GetAllRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }
        public void AddRole(Role role)
        {
            try
            {
                Role newRole = new Role
                {

                    RoleName = role.RoleName,
                    Status = role.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Roles.Add(newRole);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("RoleDAL-AddRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        public Role GetRoleById(int id)
        {
            try
            {
                Role role = new Role();
                using (JMSEntities context = new JMSEntities())
                {
                    role = context.Roles.FirstOrDefault((roles) => roles.Id == id);
                }
                return role;
            }
            catch (Exception ex)
            {
                Log.Error("RoleDAL-GetRoleById-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        public void UpdateRole(Role role)
        {
            
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Roles.FirstOrDefault((e) => e.Id == role.Id);
                    selected.RoleName = role.RoleName;
                    selected.Status = role.Status;
                    selected.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("RoleDAL-UpdateRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

        public void DeleteRole(int id)
        {
            try
            {
                using (JMSEntities context = new JMSEntities())
                {
                    var selected = context.Roles.FirstOrDefault((e) => e.Id == id);
                    selected.Status = false;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("RoleDAL-DeleteRole-Exception", ex);
                throw new Exception(ex.Message);
            }
        }

    }
}