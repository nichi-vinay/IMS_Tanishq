using System;
using System.Collections.Generic;

namespace JMS.Models
{
    public class JqueryDatatableParam
    {
        public string sEcho { get; set; }
        public string sSearch { get; set; }
        public string categorySearch { get; set; }
        public string suplierSearch { get; set; }
        public string statusSearch { get; set; }
        public string inventorySearch { get; set; }
        public int iDisplayLength { get; set; }
        public int iDisplayStart { get; set; }
        public int iColumns { get; set; }
        public int iSortCol_0 { get; set; }
        public string sSortDir_0 { get; set; }
        public int iSortingCols { get; set; }
        public string sColumns { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string PhoneNumber { get; set; }
        public string statusSearchId { get; set; }
    }
}