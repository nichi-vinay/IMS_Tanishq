using JMS.BAL.ViewModel;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.BussinesLogic
{
   public class InvoiceItemsBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InvoiceItemsBAL));
        public List<InvoiceItemsViewModel> GetAllInvoiceItemsList()
        {
            try
            {
                List<InvoiceItemsViewModel> invoiceItemsModel = new List<InvoiceItemsViewModel>();
                var invoiceItemsDALObj = new InvoiceItemsDAL();
              
                var lstInvoiceItems = invoiceItemsDALObj.GetInvoiveItems();
                foreach (var item in lstInvoiceItems)
                {
                    var obj = new InvoiceItemsViewModel()
                    {
                        Id = item.Id,
                        InvoiceId = item.InvoiceId,
                        Price=item.Price,
                        InventoryId=item.InventoryId

                    };

                    invoiceItemsModel.Add(obj);
                }

                return invoiceItemsModel;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsBAL-GetAllInvoiceItemsList-Exception", ex);
                return null;
            }
        }
        public void UpdateInvoiceItems(InvoiceItemsViewModel invoiceItem)
        {
            try
            {
                var invoiceItemDALObj = new InvoiceItemsDAL();
                invoiceItemDALObj.UpdateInvoiceItem(new InvoiceItem
                {
                    Id=invoiceItem.Id,
                    InvoiceId = invoiceItem.InvoiceId,
                    InventoryId = invoiceItem.InventoryId,
                    Status=invoiceItem.Status,
                    Price = invoiceItem.Price
                });
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsBAL-UpdateInvoiceItems-Exception", ex);
            }
        }
        public InvoiceItemsViewModel GetInvoicesItemsById(int id)
        {
            try
            {
                InvoiceItemsDAL invoiceItemsDALObj = new InvoiceItemsDAL();
                var invoicedetails = invoiceItemsDALObj.GetInvoiceItemById(id);
                if (invoicedetails != null)
                {
                    return new InvoiceItemsViewModel()
                    {
                        Id = invoicedetails.Id,
                        InvoiceId = invoicedetails.InvoiceId,
                        InventoryId = invoicedetails.InventoryId,
                        Price = invoicedetails.Price,
                        Status=invoicedetails.Status
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsBAL-GetInvoicesItemsById-Exception", ex);
                throw;
            }

        }

        public List<InvoiceItemsViewModel> GetInvoiceItemsByInvoiceId(int? invoiceId)
        {
            try
            {
                InvoiceItemsDAL invoiceItemsDALObj = new InvoiceItemsDAL();
                var invoiceItems = invoiceItemsDALObj.GetInvoiceItemsByInvoiceId(invoiceId);
                var lstitems = new List<InvoiceItemsViewModel>();
                foreach (var item in invoiceItems)
                {
                    var Obj = new InvoiceItemsViewModel
                    {
                        Id = item.Id,
                        InvoiceId = item.InvoiceId,
                        InventoryId = item.InventoryId,
                        Price = item.Price,
                        Status=item.Status
                    };
                    lstitems.Add(Obj);
                }
                return lstitems;
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsBAL-GetInvoiceItemsByInvoiceId-Exception", ex);
                return new List<InvoiceItemsViewModel>();
            }
        }
        public void AddInvoiceItem(InvoiceItemsViewModel invoiceItem)
        {
            try
            {
                var invoiceItemDALObj = new InvoiceItemsDAL();
                invoiceItemDALObj.AddInvoiceItem(new InvoiceItem { 
                    InvoiceId=invoiceItem.InvoiceId,
                    InventoryId=invoiceItem.InventoryId,
                    Price=invoiceItem.Price,
                    CreatedAt=DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Log.Error("InvoiceItemsBAL-AddInvoiceItem-Exception", ex);
                throw;
            }
        }
    }
}
