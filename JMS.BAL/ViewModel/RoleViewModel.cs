using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
 public class RoleViewModel
    {
        public int Id { get; set; }  //edit
        public string RoleName { get; set; } //create
        public int SelectedStatus { get; set; }
        public int SelectedRoleName { get; set; }
        public bool Status { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<RoleModel> Items { get; set; } //list
    }

    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
}
