using System;
using System.Collections.Generic;

namespace JMS.ViewModels
{
    public class CompanyViewModel
    {
        public int Id { get; set; }  //edit
        public string CompanyName { get; set; } //create
        public int SelectedStatus { get; set; }
        public int SelectedCompany { get; set; }//create
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