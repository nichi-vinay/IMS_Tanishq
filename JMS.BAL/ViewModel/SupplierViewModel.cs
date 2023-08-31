using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public class SupplierViewModel
    {
        public int Id { get; set; }  //edit
        public string SupplierName { get; set; } //create
        public string SupplierCode { get; set; } //create
        public string Address { get; set; }
        public string Phone { get; set; }
        public int SelectedStatus { get; set; }//create
        public bool Status { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public int SelectedSupplier { get; set; }
        public List<SupplierModel> Items { get; set; } //list
    }
    public class SupplierModel
    {
        public int Id { get; set; }
        public string SupplierName { get; set; }
        public string SupplierCode { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }
    }
}
