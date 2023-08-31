using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.DAL.DataAccess
{    
    public class InventoryDAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryDAL));
        public List<InventoryModelDal> GetAllInventory()
        {
            try
            {
                List<InventoryModelDal> _Inventory = null;
                using (JMSEntities context = new JMSEntities())
                {
                     _Inventory = context.Inventories/*.Where(x=>x.InventoryStatusId==6)*/.Select(x => new InventoryModelDal
                    {
                        Id = x.Id,
                        CompanyId = x.CompanyId,
                        JewelTypeId = x.JewelTypeId,
                        CategoryId = x.CategoryId,
                        SupplierId = x.SupplierId==null?default(int):x.SupplierId.Value,
                        CaratWeight = x.CaratWeight,
                        GoldWeight = x.GoldWeight,
                        GoldContent = x.GoldContent,
                        Pieces = x.Pieces,
                        DiamondPieces = x.DiamondPieces,
                        DateReceived = x.DateReceived,
                        Price = x.Price,
                        InventoryStatusId = x.InventoryStatusId,
                        Description = x.Description,
                        Status = x.Status
                    }).OrderBy(c=>c.CategoryId).ToList();

                    //_Inventory = context.Inventories.Where(item=>item.JewelTypeId==1).ToList();
                }
                return _Inventory;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryDAL-GetAllInventory-Exception", ex);
                return null;
            }
        }

        public string AddInventory(Inventory inventory)
        {
            try
            {
                string insertedId = "";
                Inventory newInventory = new Inventory
                {
                    CompanyId= inventory.CompanyId,
                    JewelTypeId= inventory.JewelTypeId,
                    CategoryId=inventory.CategoryId,
                    SupplierId=inventory.SupplierId,
                    CaratWeight=inventory.CaratWeight,
                    GoldWeight=inventory.GoldWeight,
                    GoldContent=inventory.GoldContent,
                    Pieces=inventory.Pieces,
                    DiamondPieces=inventory.DiamondPieces,
                    DateReceived=inventory.DateReceived,
                    Price=inventory.Price,
                    InventoryStatusId=inventory.InventoryStatusId,
                    Image=inventory.Image,
                    Description=inventory.Description,
                    Status = inventory.Status,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = null
                };
                using (JMSEntities context = new JMSEntities())
                {
                    context.Inventories.Add(newInventory);
                    context.SaveChanges();
                    insertedId = newInventory.Id.ToString();
                }
                return insertedId;
            }
            catch (Exception ex)
            {
                
                Log.Error("InventoryDAL-AddInventory-Exception", ex);
                return "0";
            }
        }

        public Inventory GetInventoryById(int? inventoryId)
        {
            try
            {
                Inventory inventory = new Inventory();
                using (JMSEntities content = new JMSEntities())
                {
                    inventory = content.Inventories.FirstOrDefault((invent) => invent.Id == inventoryId);
                }

                return inventory;
            }
            catch (Exception ex)
            {
                Log.Error("InventoryDAL-GetInventoryById-Exception", ex);
                return new Inventory();
            }
        }

        public void UpdateInventory(Inventory newInventory)
        {
            try
            {
                using (JMSEntities content = new JMSEntities())
                {
                     var inventory = content.Inventories.FirstOrDefault((invent) => invent.Id == newInventory.Id);
                    inventory.CompanyId = newInventory.CompanyId;
                    inventory.JewelTypeId = newInventory.JewelTypeId;
                    inventory.CategoryId = newInventory.CategoryId;
                    inventory.SupplierId = newInventory.SupplierId;
                    inventory.CaratWeight = newInventory.CaratWeight;
                    inventory.GoldWeight = newInventory.GoldWeight;
                    inventory.GoldContent = newInventory.GoldContent;
                    inventory.Pieces = newInventory.Pieces;
                    inventory.DiamondPieces = newInventory.DiamondPieces;
                    inventory.DateReceived = newInventory.DateReceived;
                    inventory.Price = newInventory.Price;
                    inventory.InventoryStatusId = newInventory.InventoryStatusId;
                    inventory.Image = newInventory.Image;
                    inventory.Description = newInventory.Description;
                    inventory.Status = newInventory.Status;
                    inventory.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryDAL-UpdateInventory-Exception", ex);
                throw;
            }
        }
        public void DeleteInventory(int inventoryId)
        {
            try
            {
                Inventory inventory = new Inventory();
                using (JMSEntities content = new JMSEntities())
                {
                    inventory = content.Inventories.FirstOrDefault((invent) => invent.Id == inventoryId);
                    inventory.Status = false;
                    inventory.UpdatedAt = DateTime.Now;
                    content.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryDAL-DeleteInventory-Exception", ex);
                throw;
            }
        }

    }
}
