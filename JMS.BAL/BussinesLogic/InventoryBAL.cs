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
    public class InventoryBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryBAL));
        public InventoryViewModel GetAllInventory()
        {
            try
            {
                InventoryViewModel inventoryViewModel = new InventoryViewModel();
                var inventoryDALObj = new InventoryDAL();
                List<InventoryModel> inventoryModels = new List<InventoryModel>();
                var lstInventory = inventoryDALObj.GetAllInventory();                
                foreach (var item in lstInventory)
                {
                    var obj = new InventoryModel()
                    {
                        Id = item.Id,
                        CompanyId = item.CompanyId,
                        JewelTypeId = item.JewelTypeId,
                        CategoryId = item.CategoryId,
                        SupplierId = item.SupplierId,
                        CaratWeight = item.CaratWeight,
                        GoldWeight = item.GoldWeight,
                        GoldContent = item.GoldContent,
                        Pieces = item.Pieces,
                        DiamondPieces = item.DiamondPieces,
                        DateReceived = item.DateReceived,
                        Price = item.Price,
                        Status=item.Status,
                        InventoryStatusId = item.InventoryStatusId,
                        Description = item.Description,
                        
                        StatusName = item.Status == true ? "Active" : "In-active"
                    };
                    inventoryModels.Add(obj);
                }
                inventoryViewModel.Items = inventoryModels;
                return inventoryViewModel;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryBAL-GetAllInventory-Exception", ex);
                return null;
            }
        }

        public void UpdateInventory(InventoryModel inventoryModel)
        {
            try
            {
                var inventoryDALObj = new InventoryDAL();
                inventoryDALObj.UpdateInventory(new Inventory()
                {
                    Id = inventoryModel.Id,
                    CompanyId = inventoryModel.CompanyId,
                    JewelTypeId = inventoryModel.JewelTypeId,
                    CategoryId = inventoryModel.CategoryId,
                    SupplierId = inventoryModel.SupplierId,
                    CaratWeight = inventoryModel.CaratWeight,
                    GoldWeight = inventoryModel.GoldWeight,
                    GoldContent= inventoryModel.GoldContent,
                    Pieces = inventoryModel.Pieces,
                    DiamondPieces = inventoryModel.DiamondPieces,
                    DateReceived = inventoryModel.DateReceived,
                    Price = inventoryModel.Price,
                    InventoryStatusId = inventoryModel.InventoryStatusId,
                    Image = inventoryModel.Image,
                    Description = inventoryModel.Description,
                    Status = inventoryModel.Status
                });
            }
            catch (Exception ex)
            {
                Log.Error("InventoryBAL-UpdateInventory-Exception", ex);
                throw;
            }
        }
        public string AddInventory(InventoryModel newInventory)
        {
            try
            {
                var inventoryDALObj = new InventoryDAL();
                string insertedId= inventoryDALObj.AddInventory(new Inventory {
                    CompanyId = newInventory.CompanyId,
                    JewelTypeId = newInventory.JewelTypeId,
                    CategoryId = newInventory.CategoryId,
                    SupplierId = newInventory.SupplierId,
                    CaratWeight = newInventory.CaratWeight,
                    GoldWeight = newInventory.GoldWeight,
                    GoldContent= newInventory.GoldContent,
                    Pieces = newInventory.Pieces,
                    DiamondPieces = newInventory.DiamondPieces,
                    DateReceived = newInventory.DateReceived,
                    Price = newInventory.Price,
                    InventoryStatusId = newInventory.InventoryStatusId,
                    Image = newInventory.Image,
                    Description = newInventory.Description,
                    Status = newInventory.Status
                });
                return insertedId;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryBAL-AddInventory-Exception", ex);
                return "0";
            }
        }
        public void DeleteInventory(int InventoryId)
        {
            try
            {
                var inventoryDALObj = new InventoryDAL();
                inventoryDALObj.DeleteInventory(InventoryId);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryBAL-DeleteInventory-Exception", ex);
                throw;
            }
        }
        public InventoryModel GetInventoryById(int? inventoryId)
        {
            try
            {
                var inventoryDALObj = new InventoryDAL();
                var dalInventory = inventoryDALObj.GetInventoryById(inventoryId);
                var companyDal = new CompanyDAL();
                var jewelTypeDal = new JewelTypeDAL();
                var categorryDal = new CategoryDAL();
                var supplierDal = new SupplierDAL();
                var inventoryStatusDal = new InventoryStatusDAL();
                if (dalInventory != null)
                {

                    return new InventoryModel()
                    {
                        Id = dalInventory.Id,
                        GoldContent=dalInventory.GoldContent,
                        Company = companyDal.GetCompanyById(dalInventory.CompanyId).CompanyName,
                        CompanyId=dalInventory.CompanyId,
                        JewelTypeId=dalInventory.JewelTypeId,
                        JewelType = jewelTypeDal.GetJewelTypeById(dalInventory.JewelTypeId).JewelTypeName,
                        Category = categorryDal.GetCategoryById(dalInventory.CategoryId).CategoryName,
                        CategoryId=dalInventory.CategoryId,
                        Supplier = dalInventory.SupplierId == null ? null : (supplierDal.GetSupplierById(dalInventory.SupplierId).SupplierName),
                        SupplierId=dalInventory.SupplierId,
                        CaratWeight = dalInventory.CaratWeight,
                        GoldWeight = dalInventory.GoldWeight,
                        Pieces = dalInventory.Pieces,
                        DiamondPieces = dalInventory.DiamondPieces,
                        DateReceived = dalInventory.DateReceived,
                        Price = dalInventory.Price,
                        InventoryStatus = inventoryStatusDal.GetInventoryStatusById(dalInventory.InventoryStatusId).InventoryStatusName,
                        Image = dalInventory.Image,
                        InventoryStatusId=dalInventory.InventoryStatusId,                        
                        Description = dalInventory.Description,
                        Status=dalInventory.Status,
                        StatusName = dalInventory.Status == true ? "Active" : "In-active"
                    };
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryBAL-GetInventoryById-Exception", ex);
                return null;
            }
        }
    }
}
