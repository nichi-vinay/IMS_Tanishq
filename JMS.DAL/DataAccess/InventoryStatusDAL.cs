using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{
    public class InventoryStatusDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryStatusDAL));
        public List<InventoryStatu> GetAllInventoryStatus()
        {
            try
            {
                List<InventoryStatu> _inventoryStatus = null;
                using (JMSEntities context = new JMSEntities())
                {
                    _inventoryStatus = context.InventoryStatus
                    .ToList<InventoryStatu>();
                }
                return _inventoryStatus;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusDAL-GetAllInventoryStatus-Exception",ex);
                return new List<InventoryStatu>();
            }
        }
        public void AddInventoryStatus(InventoryStatu inventoryStatus)
        {
            try
            {
                var a = inventoryStatus;
                InventoryStatu newInventoryStatus = new InventoryStatu
                {
                    InventoryStatusName = inventoryStatus.InventoryStatusName,
                    Status = inventoryStatus.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null,
                    MISC1 = null,
                    MISC2 = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.InventoryStatus.Add(newInventoryStatus);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusDAL-AddInventoryStatus-Exception", ex);
                throw;
            }
        }

        public InventoryStatu GetInventoryStatusById(int inventoryStatusId)
        {
            try
            {
                InventoryStatu inventoryStatus = new InventoryStatu();
                using (JMSEntities content = new JMSEntities())
                {
                    inventoryStatus = content.InventoryStatus.FirstOrDefault((status) => status.Id == inventoryStatusId);
                }

                return inventoryStatus;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusDAL-GetInventoryStatusById-Exception", ex);
                return new InventoryStatu();
            }
        }

        public void UpdateInventoryStatus(InventoryStatu newInventoryStatus)
        {
            try
            {
                InventoryStatu inventoryStatus = new InventoryStatu();
                using (JMSEntities content = new JMSEntities())
                {
                    inventoryStatus = content.InventoryStatus.FirstOrDefault((status) => status.Id == newInventoryStatus.Id);
                    inventoryStatus.InventoryStatusName = newInventoryStatus.InventoryStatusName;
                    inventoryStatus.Status = newInventoryStatus.Status;
                    inventoryStatus.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusDAL-UpdateInventoryStatus-Exception", ex);
                throw;
            }
        }
        public void DeleteInventoryStatus(int statusId)
        {
            try
            {
                InventoryStatu inventoryStatus = new InventoryStatu();
                using (JMSEntities content = new JMSEntities())
                {
                    inventoryStatus = content.InventoryStatus.FirstOrDefault((status) => status.Id == statusId);
                    inventoryStatus.Status = false;
                    inventoryStatus.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusDAL-DeleteInventoryStatus-Exception", ex);
                throw;
            }
        }
    }
}
