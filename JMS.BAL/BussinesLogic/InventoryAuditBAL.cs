using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages;

namespace JMS.BAL.BussinesLogic
{
    public class InventoryAuditBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryAuditBAL));
        public InventoryAuditViewModel GetAllAuditData()
        {
            try
            {
                InventoryAuditViewModel inventoryAuditViewModel = new InventoryAuditViewModel();
                List<InventoryAuditModel> inventoryAuditModels = new List<InventoryAuditModel>();
                var inventoryAuditDALObj = new InventoryAuditDAL();
                var lstInventoryAudits = inventoryAuditDALObj.GetAllAuditData();
                foreach (var item in lstInventoryAudits)
                {
                    var obj = new InventoryAuditModel()
                    {
                        Id = item.Id,
                        AuditDate = item.AuditDate.ToString("MM-dd-yyyy"),
                        ItemsInShelves = item.ItemsInShelves,
                        ItemsInInventory = item.ItemsInInventory,
                        VarianceItems = item.VarianceItems,
                        VarianceItemsInShelves = item.VarianceItemsInShelves,
                        VarianceItemsInInventory = item.VarianceItemsInInventory,
                        VarianceItemsIdsJson = item.VarianceItemsIdsJson

                    };
                    inventoryAuditModels.Add(obj);
                }
                inventoryAuditViewModel.Items = inventoryAuditModels;
                return inventoryAuditViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryAuditBAL-GetAllAuditData-Exception", ex);
                return null;
            }
        }
        public void AddAuditData(InventoryAuditModel newInventoryAuditModel)
        {
            try
            {
                var inventoryAuditDALObj = new InventoryAuditDAL();
                inventoryAuditDALObj.AddAuditData(new InventoryAudit()
                {
                    AuditDate = newInventoryAuditModel.AuditDate.AsDateTime(),
                    ItemsInShelves = newInventoryAuditModel.ItemsInShelves,
                    ItemsInInventory = newInventoryAuditModel.ItemsInInventory,
                    VarianceItems = newInventoryAuditModel.VarianceItems,
                    VarianceItemsInShelves = newInventoryAuditModel.VarianceItemsInShelves,
                    VarianceItemsInInventory = newInventoryAuditModel.VarianceItemsInInventory,
                    VarianceItemsIdsJson = newInventoryAuditModel.VarianceItemsIdsJson
                });
            }
            catch (Exception ex)
            {
                Log.Error("InventoryAuditBAL-AddCategoryAddAuditData-Exception", ex);
                throw;
            }
        }
        public void UpdateAuditData(InventoryAuditModel newInventoryAuditModel)
        {
            try
            {
                var inventoryAuditDALObj = new InventoryAuditDAL();
                inventoryAuditDALObj.UpdateAuditData(new InventoryAudit()
                {
                    Id = newInventoryAuditModel.Id,
                    AuditDate = newInventoryAuditModel.AuditDate.AsDateTime(),
                    ItemsInShelves = newInventoryAuditModel.ItemsInShelves,
                    ItemsInInventory = newInventoryAuditModel.ItemsInInventory,
                    VarianceItems = newInventoryAuditModel.VarianceItems,
                    VarianceItemsInShelves = newInventoryAuditModel.VarianceItemsInShelves,
                    VarianceItemsInInventory = newInventoryAuditModel.VarianceItemsInInventory,
                    VarianceItemsIdsJson = newInventoryAuditModel.VarianceItemsIdsJson
                });
            }
            catch (Exception ex)
            {
                Log.Error("InventoryAuditBAL-UpdateAuditData-Exceptiopn", ex);
                throw;
            }
        }

    }
}
