using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class AdhocViewModel
    {
            public string Company { get; set; }
            public string JewelType { get; set; }
            public string Category { get; set; }
            public int SelectedCompany { get; set; }
            public int SelectedJewelType { get; set; }
            public int SelectedCategory { get; set; }
            public double CaratWeight { get; set; }
            public double GoldWeight { get; set; }
            public double GoldContent { get; set; }
            public int NumberOfPieces { get; set; }
            public double OtherStones { get; set; }
            public string Description { get; set; }
            public double Price { get; set; }       
    }
}