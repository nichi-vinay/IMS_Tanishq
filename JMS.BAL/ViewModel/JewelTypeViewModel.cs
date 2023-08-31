using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
   public  class JewelTypeViewModel
    {
        public int Id { get; set; }  //edit
        public string JewelTypeName { get; set; } //create
        public int SelectedStatus { get; set; }
        public int SelectedJewelType { get; set; }//create
        public bool Status { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<JewelTypeModel> Items { get; set; }
    }
    public class JewelTypeModel
    {
        public int Id { get; set; }
        public string JewelTypeName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
}
