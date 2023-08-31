using System;
using System.Collections.Generic;

namespace JMS.ViewModels
{
    public class RoleViewModel
    {
        public int Id { get; set; }  //edit
        public string RoleName { get; set; } //create
        public int SelectedStatus { get; set; }
        public int SelectedRoleName { get; set; } //create
        public List<RoleModel> Items { get; set; } //list
    }

    public class RoleModel
    {
        public int Id { get; set; }
        public string RoleName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
}