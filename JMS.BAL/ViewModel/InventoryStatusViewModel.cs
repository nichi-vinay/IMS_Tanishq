using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public class InventoryStatusViewModel
    {
        public int Id { get; set; }
        public string InventoryStatusName { get; set; }
        public bool Status { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<InventoryStatusModel> Items { get; set; } //list
    }

    public class InventoryStatusModel
    {
        public int Id { get; set; }
        public string InventoryStatusName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
    }
}
