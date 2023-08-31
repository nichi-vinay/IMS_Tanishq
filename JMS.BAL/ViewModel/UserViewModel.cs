using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
   public  class UserViewModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public int SelectedStatus { get; set; }
        public int SelectedRoleName { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<SelectListItem> RolesList { get; set; }
        public int SelectedUser { get; set; }
        public string Password { get; set; }
        public string RoleName { get; set; }
        public List<UsersModel> Items { get; set; }
    }
    public class UsersModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int RoleId { get; set; }
       
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }


    }


}
