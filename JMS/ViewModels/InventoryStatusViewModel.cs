using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class InventoryStatusViewModel
    {
        public int Id { get; set; }  //edit
        public string InventoryStatusName { get; set; } //create
        public int SelectedStatus { get; set; } //create
        public int SelectedInventoryStatuslist { get; set; }
        public List<InventoryStatusModel> Items { get; set; } //list
    }

    public class InventoryStatusModel
    {
        public int Id { get; set; }
        public string InventoryStatusName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
}