using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string JewelType { get; set; }
        public string CategoryName { get; set; }
        public string Supplier { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string Pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string DateReceived { get; set; }
        public string Price { get; set; }
        public int InventoryStatus { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public int SelectedStatus { get; set; }
        public int SelectedCategory { get; set; }
        public int SelectedCompany { get; set; }
        public int SelectedJewelType { get; set; }
        public int SelectedSupplier { get; set; }

        public int SelectedInventoryStatus { get; set; }
        public List<InventoryModel> Items { get; set; } //list
    }
    public class InventoryModel
    {
        public int Id { get; set; }
        public string CompanyName { get; set; }
        public string JewelType { get; set; }
        public string CategoryName { get; set; }
        public string Supplier { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string Pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string DateReceived { get; set; }
        public string Price { get; set; }
        public string InventoryStatus { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int MISC1 { get; set; }
        public string MISC2 { get; set; }
    }
}