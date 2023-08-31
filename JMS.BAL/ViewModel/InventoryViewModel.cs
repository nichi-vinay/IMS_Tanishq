using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.ViewModel
{
    public class InventoryViewModel
    {
        public int Id { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string GoldContent{ get; set; }
        public string Pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string DateReceived { get; set; }
        public string Price { get; set; }
        public int InventoryStatusId { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public string StatusName { get; set; }
        public bool Status { get; set; }
        public List<SelectListItem> Company { get; set; }
        public List<SelectListItem> JewelType { get; set; }
        public List<SelectListItem> Category { get; set; }
        public List<SelectListItem> Supplier { get; set; }
        public List<SelectListItem> InventoryStatus { get; set; }
        public List<SelectListItem> StatusList { get; set; }   
        public List<InventoryModel> Items { get; set; }
    }

    public class InventoryModel
    {
        public int Id { get; set; }
        public string Company { get; set; }
        public int CompanyId { get; set; }
        public string JewelType { get; set; }
        public int JewelTypeId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string Supplier { get; set; }
        public int? SupplierId { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string GoldContent { get; set; }
        public string Pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string DateReceived { get; set; }
        public string Price { get; set; }
        public string InventoryStatus { get; set; }
        public int InventoryStatusId { get; set; }
        public byte[] Image { get; set; }
        public string ImageSrc { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
    }
}
