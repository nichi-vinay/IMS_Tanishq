using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.Models
{
    public class InventoryModel
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int JewelTypeId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string Pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string DateReceived { get; set; }
        public string Price { get; set; }
        public int InventoryStatusId { get; set; }
        public byte[] Image { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
