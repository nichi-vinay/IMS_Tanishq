using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class InvoiceItemsDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceItemsDAL));
        public List<InvoiceItem> GetInvoiveItems()
        {

            try
            {
                List<InvoiceItem> _invoiceitems = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _invoiceitems = context.InvoiceItems.Where(x=>x.Status==true)
                    .ToList<InvoiceItem>();
                }
                return _invoiceitems;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsDAL-GetInvoiveItems-Exception", ex);
                return null;

            }
        }
        public InvoiceItem GetInvoiceItemById(int id)
        {
            try
            {
                InvoiceItem invoicesItems = new InvoiceItem();
                using (JMSEntities content = new JMSEntities())
                {
                    invoicesItems = content.InvoiceItems.Where(x=>x.Status==true).FirstOrDefault((invoiceitem) => invoiceitem.Id == id);
                }

                return invoicesItems;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsDAL-GetInvoiceItemById-Exception", ex);
                throw;
            }
        }
        //public List<InvoiceItem> GetInvoiceItemsByInvoiceId(int? invoiceId)
        //{
        //    try
        //    {
        //        List<InvoiceItem> invoiceItems = new List<InvoiceItem>();

        //        using (JMSEntities context = new JMSEntities())
        //        {
        //            invoiceItems = context.InvoiceItems.Where(x => x.Id == invoiceId).ToList<InvoiceItem>();
        //            //invoiceItems = context.InvoiceItems.Where(x => x.InvoiceId == invoiceId).Select(x => new InvoiceItemsCustomization
        //            //{
        //            //    Id = x.Id,
        //            //    Description = x.Inventory.Description,
        //            //    CaratWeight = x.Inventory.CaratWeight,
        //            //    GoldWeight = x.Inventory.GoldWeight,
        //            //    GoldContent = "22",
        //            //    Pieces = x.Inventory.Pieces,
        //            //    OtherStones = "0",
        //            //    Price = x.Inventory.Price,
        //            //    CategoryId = x.Inventory.CategoryId,
        //            //    JewelTypeId = x.Inventory.JewelTypeId,
        //            //    DiamondPieces = x.Inventory.DiamondPieces,
        //            //    InventoryId = x.InventoryId
        //            //}).ToList();
        //        }
        //        return invoiceItems;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public List<InvoiceItem> GetInvoiceItemsByInvoiceId(int? invoiceId)
        {
            try
            {
                List<InvoiceItem> invoiceItems = new List<InvoiceItem>();
                using (JMSEntities context = new JMSEntities())
                {
                    invoiceItems = context.InvoiceItems.Where(x => x.InvoiceId == invoiceId && x.Status==true).ToList();
                }
                return invoiceItems;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsDAL-GetInvoiceItemsByInvoiceId-Exception", ex);
                throw;
            }
        }
        public void AddInvoiceItem(InvoiceItem invoiceItem)
        {
            try
            {
                InvoiceItem newItem = new InvoiceItem
                {

                    InvoiceId=invoiceItem.InvoiceId,
                    InventoryId=invoiceItem.InventoryId,
                    Price=invoiceItem.Price,
                    Status=true,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    MISC1 = null,
                    MISC2 = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    invoiceItem.Status = true;
                    context.InvoiceItems.Add(invoiceItem);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsDAL-AddInvoiceItem-Exception", ex);
            }
        }
        public void UpdateInvoiceItem(InvoiceItem invoiceItem)
        {
            try
            {                
                InvoiceItem item = new InvoiceItem();
                using (JMSEntities context = new JMSEntities())
                {
                    item = context.InvoiceItems.FirstOrDefault((x) => x.Id == invoiceItem.Id);
                    item.InventoryId = invoiceItem.InventoryId;
                    item.InvoiceId = invoiceItem.InvoiceId;
                    item.Price = invoiceItem.Price;
                    item.Status = invoiceItem.Status;
                    item.UpdatedAt = DateTime.Now;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsDAL-UpdateInvoiceItem-Exception", ex);
            }
        }
    }
}
