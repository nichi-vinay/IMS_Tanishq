using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }  //edit
        public string Supplier { get; set; } //create
        public int Address { get; set; }
        public int Phone { get; set; }
        public int Status { get; set; }
        public int SelectedStatus { get; set; }//create
        public int SelectedSupplier { get; set; }
        public List<SupplierModel> Items { get; set; } //list
    }
    public class SupplierModel
    {
        public int Id { get; set; }
        public string Supplier { get; set; }
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