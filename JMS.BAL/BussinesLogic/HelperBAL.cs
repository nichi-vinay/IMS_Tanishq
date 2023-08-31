using JMS.BAL.ViewModel;
using JMS.DAL;
using JMS.DAL.DataAccess;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace JMS.BAL.BussinesLogic
{
    public static class HelperBAL
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HelperBAL));
        public static List<SelectListItem> StatusList = new List<SelectListItem>() {
            new SelectListItem {Text="Active",Value="true",Selected = true },
            new SelectListItem {Text="Inactive",Value="false" ,Selected = true}
        };
        public static List<SelectListItem> TaxTypeList = new List<SelectListItem>()
        {
            new SelectListItem{Text="INC",Value="INC",Selected=true},
            new SelectListItem{Text="EXEMPT",Value="EXEMPT",Selected=true},
            new SelectListItem{Text="SHIP",Value="SHIP",Selected=true}
        };
        public static List<SelectListItem> CompanyList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                foreach (var item in new CompanyBAL().GetAllCompany().Items.OrderBy(x=>x.CompanyName).Where(x=>x.StatusName=="Active"))
                {
                    listItems.Add(new SelectListItem { Text = item.CompanyName, Value = item.Id.ToString() });

                }
               
                return listItems;
            }
            catch (Exception ex )
            {
                Log.Error("HelperBAL-CompanyList-Exception", ex);
                throw;
            }
        }
     
        public static List<SelectListItem> EmployeeList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                foreach (var item in new UserBAL().GetAllUsers().Items.OrderBy(x=>x.UserName).Where(x=>x.StatusName=="Active"))
                {
                    listItems.Add(new SelectListItem { Text = item.Name, Value = item.Id.ToString() });

                }
                return listItems;
            }
            catch (Exception ex)
            {
                Log.Error("HelperBAL-EmployeeList-Exception", ex);
                throw;
            }
        }
        public static List<SelectListItem> JewelTypeList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                foreach (var item in new JewelTypeBAL().GetAllJewelTypes().Items.Where(x => x.StatusName == "Active").OrderBy(x=>x.JewelTypeName))
                {
                    listItems.Add(new SelectListItem { Text = item.JewelTypeName, Value = item.Id.ToString() });

                }
                return listItems;
            }
            catch (Exception ex)
            {
                Log.Error("HelperBAL-JewelTypeList-Exception", ex);
                throw;
            }
        }
        public static List<SelectListItem> CategoryList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                foreach (var item in new CategoryBAL().GetAllCategory().Items.OrderBy(x=>x.CategoryName).Where(x=>x.StatusName=="Active"))
                {
                    listItems.Add(new SelectListItem { Text = item.CategoryName, Value = item.Id.ToString() });

                }
                
                return listItems.OrderBy(x=>x.Text).ToList<SelectListItem>();
            }
            catch (Exception ex)
            {
                Log.Error("HelperBAL-CategoryList-Exception", ex);
                throw;
            }
        }
        public static List<SelectListItem> SupplierList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
               
                foreach (var item in new SupplierBAL().GetAllSuppliers().Items.OrderBy(x => x.SupplierName).Where(x => x.StatusName == "Active"))
                {
                    listItems.Add(new SelectListItem { Text = item.SupplierName + "(" + item.SupplierCode + ")", Value = item.Id.ToString() });

                }
                return listItems;
            }
            catch (Exception ex)
            {
                Log.Error("HelperBAL-SupplierList-Exception", ex);
                throw;
            }
        }
        public static List<SelectListItem> InventoryStatusList()
        {
            try
            {
                List<SelectListItem> listItems = new List<SelectListItem>();
                foreach (var item in new InventoryStatusBAL().GetAllInventoryStatus().Items.OrderBy(x=>x.InventoryStatusName).Where(x=>x.StatusName=="Active"))
                {
                    listItems.Add(new SelectListItem { Text = item.InventoryStatusName, Value = item.Id.ToString() });

                }
                return listItems;
            }
            catch (Exception ex)
            {
                Log.Error("HelperBAL-InventoryStatusList-Exception", ex);
                throw;
            }
        }


        public static List<SelectListItem> GetRolesList()
        {
            try
            {
                var roleBALObj = new RoleBAL();
                var lst = roleBALObj.GetAllRoles().Items.OrderBy(x=>x.RoleName).Where(x=>x.StatusName=="Active");
                List<SelectListItem> selectListItems = new List<SelectListItem>();
                foreach (var item in lst)
                {
                    selectListItems.Add(new SelectListItem { Text = item.RoleName, Value = item.Id.ToString() });
          
                }
                return selectListItems;
            }
            catch (Exception ex)
            {
                Log.Error("HelperBAL-GetRolesList-Exception", ex);
                throw;
            }
        }

        public static string GetUsersNameBYUserId(int userId)
        {
            UserDAL userDALObj = new UserDAL();
            var lstUsers = userDALObj.GetAllUsers().OrderBy(x => x.UserName).Where(x=>x.Status==true);
            string name = lstUsers.FirstOrDefault(x => x.Id == userId).Name;
            return name;

        }
        public static string GetCustomerNameByCustomerId(int? customerId)
        {
            CustomerDAL customerDALObj = new CustomerDAL();
            var lstCustomers = customerDALObj.GetAllCustomers().OrderBy(x=>x.CustomerName);
            string customerName = lstCustomers.FirstOrDefault(x => x.Id == customerId).CustomerName;
            return customerName;
        }
        public static string GetStatusByInventoryStatusId(int inventoryStatusId)
        {
            InventoryStatusDAL inventoryStatusDALObj = new InventoryStatusDAL();
            var lstInventoryStatus = inventoryStatusDALObj.GetAllInventoryStatus().OrderBy(x=>x.InventoryStatusName);
            string status = lstInventoryStatus.FirstOrDefault(x => x.Id == inventoryStatusId).InventoryStatusName;
            return status;
        }
        public static int? GetInventoryIdByinvoiceId(int? invoiceId)
        {
            InvoiceItemsBAL invoiceItemsBALObj = new InvoiceItemsBAL();
            var lstinvoiceItems = invoiceItemsBALObj.GetAllInvoiceItemsList();
          
                var inventoryId = lstinvoiceItems.FirstOrDefault(x => x.InvoiceId == invoiceId).InventoryId;
            return inventoryId;
        }
        //public static List<InvoiceItemsViewModel> GetInvoiceItemsByInvoiceId(int? invoiceId)
        //{
        //     InvoiceItemsBAL invoiceItemsBALObj = new InvoiceItemsBAL();
        //    var lstinvoiceItems = invoiceItemsBALObj.GetAllInvoiceItemsList();
        //    InventoryBAL inventoryBALObj = new InventoryBAL();
        //    var lstInvoiceItems = new List<InvoiceItemsViewModel>();

        //    if (invoiceId != null)
        //    {
        //        foreach (var item in lstInvoiceItems)
        //        {
        //            if (item.InventoryId != null)
        //            {
        //                int? inventoryId = lstinvoiceItems.FirstOrDefault(x => x.InvoiceId == invoiceId).InventoryId;


        //                var price = lstinvoiceItems.FirstOrDefault(x => x.InvoiceId == invoiceId).Price;
        //                if (inventoryId != null)
        //                {
        //                    var lstInventoryitemsGotByInvoiceId = new InvoiceItemsViewModel
        //                    {
        //                        InventoryId = inventoryId,
        //                        Description = inventoryBALObj.GetInventoryById(inventoryId).Description,
        //                        GoldWeight = inventoryBALObj.GetInventoryById(inventoryId).GoldWeight,
        //                        CaratWeight = inventoryBALObj.GetInventoryById(inventoryId).CaratWeight,
        //                        Pieces = inventoryBALObj.GetInventoryById(inventoryId).Pieces,
        //                        Price = price

        //                    };
        //                    lstInvoiceItems.Add(lstInventoryitemsGotByInvoiceId);
        //                }
        //            }
        //        }
        //    }
        //    return lstInvoiceItems;


        //}
        //public static List<Inventory> GetInventory()
        //{
        //    InventoryDAL inventoryDALObj = new InventoryDAL();
        //   var lstInventory= inventoryDALObj.GetInventory();
        //    return lstInventory;
        //}

    }
}
