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
    public class InventoryStatusBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryStatusBAL));
        public InventoryStatusViewModel GetAllInventoryStatus()
        {
            try
            {
                InventoryStatusViewModel inventoryStatusViewModel = new InventoryStatusViewModel();
                var inventoryStatusDALObj = new InventoryStatusDAL();
                List<InventoryStatusModel> inventoryStatusModels = new List<InventoryStatusModel>();
                var lstOfInventoryStatus = inventoryStatusDALObj.GetAllInventoryStatus();
                foreach (var item in lstOfInventoryStatus)
                {
                    var obj = new InventoryStatusModel()
                    {
                        Id = item.Id,
                        InventoryStatusName = item.InventoryStatusName,
                        StatusName = item.Status == true ? "Active" : "In-Active"
                    };
                    inventoryStatusModels.Add(obj);
                }
                inventoryStatusViewModel.Items = inventoryStatusModels;
                return inventoryStatusViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusBAL-GetAllInventoryStatus-Exception", ex);
                return new InventoryStatusViewModel();
            }
        }

        public void UpdateInventoryStatus(InventoryStatusModel inventoryStatusModel)
        {
            try
            {
                var inventoryStatusDALObj = new InventoryStatusDAL();
                inventoryStatusDALObj.UpdateInventoryStatus(new InventoryStatu()
                {
                    Id = inventoryStatusModel.Id,
                    InventoryStatusName = inventoryStatusModel.InventoryStatusName,
                    Status = inventoryStatusModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusBAL-UpdateInventoryStatus-Exception", ex);
                throw;
            }
        }
        public void AddInventoryStatus(InventoryStatusModel inventoryStatus)
        {
            try
            {
                var inventoryStatusDALObj = new InventoryStatusDAL();
                inventoryStatusDALObj.AddInventoryStatus(new InventoryStatu
                {
                    Status = inventoryStatus.Status,
                    InventoryStatusName = inventoryStatus.InventoryStatusName

                });
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusBAL-AddInventoryStatus-Exception", ex);
                throw;
            }
        }
        public void DeleteInventoryStatus(int statusId)
        {
            try
            {
                var inventoryStatusDALObj = new InventoryStatusDAL();
                inventoryStatusDALObj.DeleteInventoryStatus(statusId);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusBAL-DeleteInventoryStatus-Exception", ex);
                throw;
            }
        }
        public InventoryStatusModel GetInventoryStatusById(int statusId)
        {
            try
            {
                var inventoryStatusDALObj = new InventoryStatusDAL();
                var dalInventoryStatus = inventoryStatusDALObj.GetInventoryStatusById(statusId);
                if (dalInventoryStatus != null)
                {
                    return new InventoryStatusModel()
                    {
                        Id = dalInventoryStatus.Id,
                        InventoryStatusName = dalInventoryStatus.InventoryStatusName,
                        StatusName = dalInventoryStatus.Status == true ? "Active" : "In-active"
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryStatusBAL-GetInventoryStatusById-Exception", ex);
                return null;
            }
        }
    }
}
