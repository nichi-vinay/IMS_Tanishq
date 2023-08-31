using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class InvoiceListViewModel
    {
        public string Date { get; set; }
        public int InvoiceId { get; set; }
        public string Customer { get; set; }
        public double Total { get; set; }
        public string Employee { get; set; }
        public string Status { get; set; }
        public double Balance { get; set; }

        public List<InvoiceItems> Items {get;set;}
    }
    public class InvoiceItems
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double CaratWeight { get; set; }
        public double GoldWeight { get; set; }
        public double GoldContent { get; set; }
        public int Pieces { get; set; }
        public string OtherStones { get; set; }
        public double Price { get; set; }
    }
}