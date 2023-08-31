using System;
using System.Collections.Generic;

namespace JMS.ViewModels
{
    public class CustomerViewModel
    {        
        public string CustomerName { get; set; } //create
        public string CustomerPhone { get; set; } //create
        public string Address { get; set; } //create
        public string DLNumber { get; set; } //create
        public string DOB { get; set; } //create
        public string ExpDate { get; set; } //create
        public string Email { get; set; }
        public int SelectedStatus { get; set; }
        public List<CustomerModel> CustomerItems { get; set; } //list
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
        public bool Status { get; set; }
        public string Statusname { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }
    }
}