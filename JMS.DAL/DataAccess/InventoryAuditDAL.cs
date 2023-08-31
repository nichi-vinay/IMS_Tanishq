using log4net;
using System;
using System.Collections.Generic;
using System.Linq;

namespace JMS.DAL.DataAccess
{
    public class InventoryAuditDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryAuditDAL));
        public List<InventoryAudit> GetAllAuditData()
        {
            try
            {
                List<InventoryAudit> _auditData = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _auditData = context.InventoryAudits.OrderByDescending(x=>x.Id).Take(5).ToList<InventoryAudit>();

                }
                return _auditData;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryAuditDAL-Exception-GetAllAuditData", ex);
                return null;
            }
        }
        public void AddAuditData(InventoryAudit inventoryAudit)
        {
            try
            {
                InventoryAudit newInventoryAudit = new InventoryAudit
                {
                    AuditDate= inventoryAudit.AuditDate,
                    ItemsInShelves = inventoryAudit.ItemsInShelves,
                    ItemsInInventory = inventoryAudit.ItemsInInventory,
                    VarianceItems = inventoryAudit.VarianceItems,
                    VarianceItemsInShelves = inventoryAudit.VarianceItemsInShelves,
                    VarianceItemsInInventory = inventoryAudit.VarianceItemsInInventory,
                    VarianceItemsIdsJson = inventoryAudit.VarianceItemsIdsJson
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.InventoryAudits.Add(newInventoryAudit);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryAuditDAL-AddAuditData-Exception-", ex);
                throw;
            }
        }
        public void UpdateAuditData(InventoryAudit newInventoryAudit)
        {
            try
            {
                InventoryAudit inventoryAudit = new InventoryAudit();
                using (JMSEntities content = new JMSEntities())
                {
                    inventoryAudit = content.InventoryAudits.FirstOrDefault((x) => x.Id == newInventoryAudit.Id);
                    inventoryAudit.AuditDate = newInventoryAudit.AuditDate;
                    inventoryAudit.ItemsInShelves = newInventoryAudit.ItemsInShelves;
                    inventoryAudit.ItemsInInventory = newInventoryAudit.ItemsInInventory;
                    inventoryAudit.VarianceItems = newInventoryAudit.VarianceItems;
                    inventoryAudit.VarianceItemsInShelves = newInventoryAudit.VarianceItemsInShelves;
                    inventoryAudit.VarianceItemsInInventory = newInventoryAudit.VarianceItemsInInventory;
                    inventoryAudit.VarianceItemsIdsJson = newInventoryAudit.VarianceItemsIdsJson;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryAuditDAL-UpdateAuditData-Exception", ex);
                throw;
            }
        }
    }
}
