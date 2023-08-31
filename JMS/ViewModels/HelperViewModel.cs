using JMS.BAL.BussinesLogic;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public  class HelperViewModel
    {
      
        public static List<SelectListItem> StatusList = new List<SelectListItem>() {
            new SelectListItem {Text="Active",Value="1",Selected=true },
            new SelectListItem {Text="Inactive",Value="0" },
        };
        public static List<SelectListItem> roleNamelist = new List<SelectListItem>() {
            new SelectListItem {Text="Admin",Value="1",Selected=true },
            new SelectListItem {Text="Standard",Value="0" },
        };
        public static List<SelectListItem> categorylist = new List<SelectListItem>() {
            new SelectListItem {Text="Gold Neclace",Value="1",Selected=true },
            new SelectListItem {Text="Diamond Bracelet"},
            new SelectListItem {Text="Silver items"},
            new SelectListItem {Text="Gold chains"},
        };

        public static List<SelectListItem> companylist = new List<SelectListItem>() {
            new SelectListItem {Text="KV Diamonds",Value="1",Selected=true },
            new SelectListItem {Text="kirti Jewellery" },
        };
        public static List<SelectListItem> jeweltypelist = new List<SelectListItem>() {
            new SelectListItem {Text="Gold",Value="1",Selected=true },
            new SelectListItem {Text="Diamond" },
            new SelectListItem{Text="Silver"},
        };
        public static List<SelectListItem> supplierlist = new List<SelectListItem>() {
            new SelectListItem {Text="kV Diamonds",Value="1",Selected=true },
            new SelectListItem {Text="Kirti Jewellers" },
            new SelectListItem{Text="Surya International"},
            new SelectListItem{Text="JA"},
        };
        public static List<SelectListItem> inventorystatuslist = new List<SelectListItem>() {
            new SelectListItem {Text="LayAway",Selected=true },
            new SelectListItem {Text="Instock",Value="1"},
            new SelectListItem{Text="Memo"},
             new SelectListItem{Text="Sold"},
            new SelectListItem{Text="Melt"},
        };
        public static List<SelectListItem> employeeList = new List<SelectListItem>() {
            new SelectListItem {Text="Suresh",Value="0",Selected=true },
            new SelectListItem {Text="Soma",Value="1"},
            new SelectListItem{Text="Yeshwanth",Value="2"}
        };
        public static List<SelectListItem> companyList = new List<SelectListItem>() {
            new SelectListItem {Text="kV Diamonds",Value="0",Selected=true },
            new SelectListItem {Text="Kirti Jewellers",Value="1" }
		};
        public static List<SelectListItem> userslist = new List<SelectListItem>() {
            new SelectListItem {Text="Suresh",Selected=true },
            new SelectListItem {Text="Sachin",Value="1"},
            new SelectListItem{Text="Ajay"},
             new SelectListItem{Text="Chethan"},
            new SelectListItem{Text="Manoj"},
        };
      
    }
   

}