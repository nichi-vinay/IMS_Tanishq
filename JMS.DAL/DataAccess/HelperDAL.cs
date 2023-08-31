using System;

namespace JMS.DAL.DataAccess
{
    public class InventoryModelDal
    {
        public int Id { get; set; }
        public int CompanyId { get; set; }
        public int JewelTypeId { get; set; }
        public int CategoryId { get; set; }
        public int SupplierId { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string GoldContent { get; set; }
        public string Pieces { get; set; }
        public string DiamondPieces { get; set; }
        public string DateReceived { get; set; }
        public string Price { get; set; }
        public int InventoryStatusId { get; set; }
        public string ImageSrc { get; set; }
        public string Description { get; set; }
        public bool Status { get; set; }
    }
    //    { "data": "Date" },
    //{ "data": "InvoiceId" },
    //{ "data": "Customer" },
    //{ "data": "Total" },
    //{ "data": "Employee" },
    //{ "data": "Status" },
    //{ "data": "Balance" },
    public class InvoiceCustomization
    {
        public int Id { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Customer { get; set; }
        public string SubTotal { get; set; }
        public string Cheque { get; set; }
        public string CreditCard { get; set; }
        public string Cash { get; set; }
        public string TotalAmount { get; set; }
        public string Balance { get; set; }
        public int UserId { get; set; }
        public string Employee { get; set; }
        public int InventoryStatusId { get; set; }
        public string InventoryStatusName { get; set; }
        public string TaxType { get; set; }
        public string LayAwayInvoiceId { get; set; }
        public Nullable<int> CompanyId { get; set; }
        public Nullable<bool> Status { get; set; }
        public System.DateTime CreatedAt { get; set; }
        public Nullable<System.DateTime> UpdatedAt { get; set; }
    }
    public class InvoiceItemsCustomization
    {
        //'<td>' + itemGot.Id + '</td>' +
        //  '<td>' + itemGot.Description + '</td>' +
        //  '<td>' + itemGot.CaratWeight + '</td>' +
        //  '<td>' + itemGot.GoldWeight + '</td>' +
        //  '<td>' + itemGot.GoldContent + '</td>' +
        //  '<td>' + itemGot.Pieces + '</td>' +
        //  '<td>' + itemGot.OtherStones + '</td>' +
        //  '<td>' + itemGot.Price + '</td>' +
        public int Id { get; set; }
        public string Description { get; set; }
        public string CaratWeight { get; set; }
        public string GoldWeight { get; set; }
        public string GoldContent { get; set; }
        public string Pieces { get; set; }
        public string OtherStones { get; set; }
        public string Price { get; set; }
        public string JewelType { get; set; }
        public int JewelTypeId { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public string DiamondPieces { get; set; }
        public int? InventoryId { get; set; }
    }
}