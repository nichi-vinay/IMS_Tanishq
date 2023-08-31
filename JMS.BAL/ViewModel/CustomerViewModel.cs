using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public class CustomerViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } //create
        public string CustomerPhone { get; set; } //create
        public string Address { get; set; } //create
        public string DLNumber { get; set; } //create
        public string DOB { get; set; } //create
        public string ExpDate { get; set; } //create
        public string Email { get; set; }
        public string City { get; set; } //create
        public string State { get; set; } //create
        public string Zip { get; set; }
        public bool Status { get; set; }
        public List<SelectListItem> StatusList { get; set; }
        public List<CustomerModel> Items { get; set; } //list
        public string FirstorDefault { get; set; }
    }
    
    public class CustomerModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string Address { get; set; }
        public string DLNumber { get; set; }
        public string ExpDate { get; set; }
        public string DOB { get; set; }
        public string Email { get; set; }
        public string City { get; set; } //create
        public string State { get; set; } //create
        public string Zip { get; set; }
        public bool? Status { get; set; }
        public string StatusName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }
    }
}