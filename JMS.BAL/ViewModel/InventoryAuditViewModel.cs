using System;
using System.Collections.Generic;

namespace JMS.BAL.ViewModel
{
    public class InventoryAuditViewModel
    {
        public List<InventoryAuditModel> Items { get; set; } //list
    }

    public class InventoryAuditModel
    {
        public int Id { get; set; }
        public string AuditDate { get; set; }
        public int ItemsInShelves { get; set; }
        public int ItemsInInventory { get; set; }
        public int VarianceItems { get; set; }
        public int VarianceItemsInShelves { get; set; }
        public int VarianceItemsInInventory { get; set; }
        public string VarianceItemsIdsJson { get; set; }
     
        

    }
}
