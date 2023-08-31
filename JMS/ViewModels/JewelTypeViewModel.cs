using System;
using System.Collections.Generic;

namespace JMS.ViewModels
{
    public class JewelTypeViewModel
    {
        public int Id { get; set; }  //edit
        public string JewelTypeName { get; set; } //create
        public int SelectedStatus { get; set; }
        public int SelectedJewelType { get; set; }//create
        public List<JewelTypeModel> Items { get; set; } //list
    }

    public class JewelTypeModel
    {
        public int Id { get; set; }
        public string JewelTypeName { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CratedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
}