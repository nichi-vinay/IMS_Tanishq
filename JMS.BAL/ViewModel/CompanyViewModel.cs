using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public class CompanyViewModel
    {
        public int Id { get; set; }  //edit
        public string CompanyName { get; set; } //create
        public bool Status { get; set; }
        public int SelectedCompany { get; set; }//create
        public List<SelectListItem> StatusList { get; set; }
        public List<CompanyModel> Items { get; set; } //list
    }

    public class CompanyModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
}
