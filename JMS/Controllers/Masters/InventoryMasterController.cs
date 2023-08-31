using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Jewel.Libraries.Barcode;
using System.Drawing;
using JMS.Models;
using log4net;
using System.Text.RegularExpressions;

namespace JMS.Controllers
{
    public class InventoryMasterController : Controller
    {
        // GET: InventoryMaster
        private static readonly ILog Log = LogManager.GetLogger(typeof(InventoryMasterController));

        public ActionResult Index()
        {
            try
            {
                var inventoryViewModel = new InventoryViewModel
                {
                    StatusList = HelperBAL.StatusList,
                    Company = HelperBAL.CompanyList(),
                    JewelType = HelperBAL.JewelTypeList(),
                    Category = HelperBAL.CategoryList(),
                    Supplier = HelperBAL.SupplierList(),
                    InventoryStatus = HelperBAL.InventoryStatusList()
                };
                return View(inventoryViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-Index-Exception", ex);
                return null;//To-do: Redirect to Error Page 
            }
        }
        public ActionResult GetInventoryJsonData(JqueryDatatableParam param)
        {
            try
            {
                var inventoryBalObj = new InventoryBAL();
                var inventories = inventoryBalObj.GetAllInventory().Items;

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    inventories = inventories.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())).ToList();
                }
                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                if (!string.IsNullOrEmpty(param.categorySearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.CategoryId.ToString()) && x.CategoryId.ToString().Equals(param.categorySearch)).ToList();
                }
                if (!string.IsNullOrEmpty(param.suplierSearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.SupplierId.ToString()) && x.SupplierId.ToString().Equals(param.suplierSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(param.statusSearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.Status.ToString()) && x.Status.ToString().ToLower().Equals(param.statusSearch.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(param.inventorySearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.InventoryStatusId.ToString()) && x.InventoryStatusId.ToString().ToLower().Equals(param.inventorySearch.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(param.statusSearchId))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.Id.ToString()) && x.Id.ToString().ToLower().Equals(param.statusSearchId.ToLower())).ToList();
                }
                switch (sortColumnIndex)
                {
                    case 0:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Id).ToList() : inventories.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.JewelType).ToList() : inventories.OrderByDescending(c => c.JewelType).ToList();
                        break;
                    case 2:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.CaratWeight).ToList() : inventories.OrderByDescending(c => c.CaratWeight).ToList();
                        break;
                    case 3:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.GoldWeight).ToList() : inventories.OrderByDescending(c => c.GoldWeight).ToList();
                        break;
                    case 4:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Pieces).ToList() : inventories.OrderByDescending(c => c.Pieces).ToList();
                        break;
                    case 5:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Description).ToList() : inventories.OrderByDescending(c => c.Description).ToList();
                        break;
                    case 6:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Company).ToList() : inventories.OrderByDescending(c => c.Company).ToList();
                        break;
                    case 7:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Supplier).ToList() : inventories.OrderByDescending(c => c.Supplier).ToList();
                        break;
                    case 8:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Price).ToList() : inventories.OrderByDescending(c => c.Price).ToList();
                        break;
                    case 9:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.DateReceived).ToList() : inventories.OrderByDescending(c => c.DateReceived).ToList();
                        break;
                    case 10:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.InventoryStatus).ToList() : inventories.OrderByDescending(c => c.InventoryStatus).ToList();
                        break;
                    case 11:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.StatusName).ToList() : inventories.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = inventories.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? inventories.Count : param.iDisplayLength).ToList();

                var jewelTypeBAL = new JewelTypeBAL();
                var companyBAL = new CompanyBAL();
                var supplierBAL = new SupplierBAL();
                var categoryBAL = new CategoryBAL();
                var inventoryStatusBAL = new InventoryStatusBAL();
                if (displayResult != null)
                {
                    foreach (var item in displayResult)
                    {
                        item.Price = "$" + Regex.Match(item.Price, @"([-+]?[0-9]*\.?[0-9]+)").Value;

                        item.JewelType = jewelTypeBAL.GetJewelTypeById(item.JewelTypeId).JewelTypeName;
                        item.Company = companyBAL.GetCompanyById(item.CompanyId).CompanyName;
                        item.Category = categoryBAL.GetCategoryById(item.CategoryId).CategoryName;
                        if (!(item.SupplierId == null || item.SupplierId == 0))
                        {
                            item.Supplier = supplierBAL.GetSupplierById(item.SupplierId).SupplierName;
                        }
                        else
                        {
                            item.Supplier = "";
                        }

                        item.InventoryStatus = inventoryStatusBAL.GetInventoryStatusById(item.InventoryStatusId).InventoryStatusName;
                        var a = inventoryBalObj.GetInventoryById(item.Id);
                        byte[] image;
                        if (a.Image != null)
                        {
                            image = a.Image;
                        }
                        else
                        {
                            string imagePath = AppDomain.CurrentDomain.BaseDirectory + "DefaultImages\\No_image.png";
                            image = System.IO.File.ReadAllBytes(imagePath); ;
                        }
                        string imreBase64Data = Convert.ToBase64String(image);
                        item.ImageSrc = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);
                    }
                }

                var totalRecords = inventories.Count;

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = displayResult
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-GetInventoryJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetInventoryInstockJsonData(JqueryDatatableParam param)
        {
            try
            {
                var inventoryBalObj = new InventoryBAL();
                var inventories = inventoryBalObj.GetAllInventory().Items.Where(x => x.InventoryStatusId == 6).ToList();

                if (!string.IsNullOrEmpty(param.sSearch))
                {
                    inventories = inventories.Where(x => x.Id.ToString().ToLower() != null && x.Id.ToString().ToLower().Contains(param.sSearch.ToLower())).ToList();
                }
                var sortColumnIndex = Convert.ToInt32(HttpContext.Request.QueryString["iSortCol_0"]);
                var sortDirection = HttpContext.Request.QueryString["sSortDir_0"];
                if (!string.IsNullOrEmpty(param.categorySearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.CategoryId.ToString()) && x.CategoryId.ToString().Equals(param.categorySearch)).ToList();
                }
                if (!string.IsNullOrEmpty(param.suplierSearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.SupplierId.ToString()) && x.SupplierId.ToString().Equals(param.suplierSearch)).ToList();
                }
                if (!string.IsNullOrEmpty(param.statusSearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.Status.ToString()) && x.Status.ToString().ToLower().Equals(param.statusSearch.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(param.inventorySearch))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.InventoryStatusId.ToString()) && x.InventoryStatusId.ToString().ToLower().Equals(param.inventorySearch.ToLower())).ToList();
                }
                if (!string.IsNullOrEmpty(param.statusSearchId))
                {
                    inventories = inventories.Where(x => !string.IsNullOrEmpty(x.Id.ToString()) && x.Id.ToString().ToLower().Equals(param.statusSearchId.ToLower())).ToList();
                }
                switch (sortColumnIndex)
                {
                    case 0:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Id).ToList() : inventories.OrderByDescending(c => c.Id).ToList();
                        break;
                    case 1:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.JewelType).ToList() : inventories.OrderByDescending(c => c.JewelType).ToList();
                        break;
                    case 2:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.CaratWeight).ToList() : inventories.OrderByDescending(c => c.CaratWeight).ToList();
                        break;
                    case 3:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.GoldWeight).ToList() : inventories.OrderByDescending(c => c.GoldWeight).ToList();
                        break;
                    case 4:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Pieces).ToList() : inventories.OrderByDescending(c => c.Pieces).ToList();
                        break;
                    case 5:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Description).ToList() : inventories.OrderByDescending(c => c.Description).ToList();
                        break;
                    case 6:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Company).ToList() : inventories.OrderByDescending(c => c.Company).ToList();
                        break;
                    case 7:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Supplier).ToList() : inventories.OrderByDescending(c => c.Supplier).ToList();
                        break;
                    case 8:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.Price).ToList() : inventories.OrderByDescending(c => c.Price).ToList();
                        break;
                    case 9:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.DateReceived).ToList() : inventories.OrderByDescending(c => c.DateReceived).ToList();
                        break;
                    case 10:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.InventoryStatus).ToList() : inventories.OrderByDescending(c => c.InventoryStatus).ToList();
                        break;
                    case 11:
                        inventories = sortDirection == "asc" ? inventories.OrderBy(c => c.StatusName).ToList() : inventories.OrderByDescending(c => c.StatusName).ToList();
                        break;
                }

                var displayResult = inventories.Skip(param.iDisplayStart)
                                         .Take(param.iDisplayLength == -1 ? inventories.Count : param.iDisplayLength).ToList();

                var jewelTypeBAL = new JewelTypeBAL();
                var companyBAL = new CompanyBAL();
                var supplierBAL = new SupplierBAL();
                var categoryBAL = new CategoryBAL();
                var inventoryStatusBAL = new InventoryStatusBAL();
                if (displayResult != null)
                {
                    foreach (var item in displayResult)
                    {
                        item.Price = "$" + Regex.Match(item.Price, @"([-+]?[0-9]*\.?[0-9]+)").Value;

                        item.JewelType = jewelTypeBAL.GetJewelTypeById(item.JewelTypeId).JewelTypeName;
                        item.Company = companyBAL.GetCompanyById(item.CompanyId).CompanyName;
                        item.Category = categoryBAL.GetCategoryById(item.CategoryId).CategoryName;
                        if (!(item.SupplierId == null || item.SupplierId == 0))
                        {
                            item.Supplier = supplierBAL.GetSupplierById(item.SupplierId).SupplierName;
                        }
                        else
                        {
                            item.Supplier = "";
                        }

                        item.InventoryStatus = inventoryStatusBAL.GetInventoryStatusById(item.InventoryStatusId).InventoryStatusName;
                        var a = inventoryBalObj.GetInventoryById(item.Id);
                        byte[] image;
                        if (a.Image != null)
                        {
                            image = a.Image;
                        }
                        else
                        {
                            string imagePath = AppDomain.CurrentDomain.BaseDirectory + "DefaultImages\\No_image.png";
                            image = System.IO.File.ReadAllBytes(imagePath); ;
                        }
                        string imreBase64Data = Convert.ToBase64String(image);
                        item.ImageSrc = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);
                    }
                }

                var totalRecords = inventories.Count;

                return Json(new
                {
                    param.sEcho,
                    iTotalRecords = totalRecords,
                    iTotalDisplayRecords = totalRecords,
                    aaData = displayResult
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-GetInventoryJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult FindInventoryImage(int Id)
        {
            try
            {

                var inventory = new InventoryBAL().GetInventoryById(Id);
                byte[] image = inventory.Image;
                string imgDataURL = "";
                string noImageURl = "";
                if (inventory != null)
                {
                    if (image != null)
                    {
                        string imreBase64Data = Convert.ToBase64String(image);
                        imgDataURL = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);
                    }
                    else
                    {
                        string imagePath = AppDomain.CurrentDomain.BaseDirectory + "DefaultImages\\No_image.png";
                        image = System.IO.File.ReadAllBytes(imagePath);
                        string imreBase64Data = Convert.ToBase64String(image);
                        noImageURl = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);
                    }
                }

                //ViewBag.Image = imgDataURL;
                return Json(new { data = imgDataURL, values = inventory, noImage = noImageURl }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-FindInventoryImage-Exception", ex);
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult DeleteInventory(int Id)
        {
            try
            {
                new InventoryBAL().DeleteInventory(Id);
                return Json(new { data = "deleted" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-DeleteInventory-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult CreateOrUpdate(InventoryModel model)
        {
            try
            {
                if (!model.ImageSrc.Contains("noImageAttached"))
                {
                    model.Image = Convert.FromBase64String(model.ImageSrc.Split(',')[1]);
                }

                var inventoryBalObj = new InventoryBAL();
                if (inventoryBalObj.GetInventoryById(model.Id) != null)
                {
                    inventoryBalObj.UpdateInventory(model);
                    return Json(new { data = "Updated", idValue = model.Id }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    string insertedID = inventoryBalObj.AddInventory(model);
                    return Json(new { data = "Created", idValue = insertedID }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-CreateOrUpdate-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GenerateBarcode(string Id)
        {
            Barcoder barcoder = new Barcoder();
            TYPE type = TYPE.CODE128C;

            int width = 190;
            int height = 21;

            barcoder.Alignment = AlignmentPositions.LEFT;
            type = TYPE.CODE128C;

            //barcoder.IncludeLabel = true;
            //value = "100000";

            try
            {
                string leftText1 = "";
                string line2 = "";
                string rightText = "";
                string text3 = "";
                decimal price = Convert.ToDecimal("0.00");
                var inventory = new InventoryBAL().GetInventoryById(int.Parse(Id));
                var supplier = inventory.SupplierId == null ? "" : new SupplierBAL().GetSupplierById(inventory.SupplierId).SupplierName;
                var companyName = inventory.CompanyId == 0 ? "" : new CompanyBAL().GetCompanyById(inventory.CompanyId).CompanyName;
                var categoryName = inventory.CategoryId == 0 ? "" : new CategoryBAL().GetCategoryById(inventory.CategoryId).CategoryName;
                var categoryNumber = inventory.CategoryId == 0 ? "" : new CategoryBAL().GetCategoryById(inventory.CategoryId).CategoryNumber;
                var dateReceived = inventory.DateReceived == null ? "" : inventory.DateReceived.Replace("/", "");

                if (inventory != null || inventory.Id != 0)
                {
                    if (inventory.CompanyId == 1)
                    {
                        leftText1 = "KJ-" + inventory.Id + "/" + supplier;
                        for (int i = 0; i < categoryName.Length; i++)
                        {
                            if (i == 0)
                            {
                                line2 = line2 + categoryName[i];
                            }
                            else if (categoryName[i - 1] == ' ')
                            {
                                line2 = line2 + categoryName[i];
                            }

                        }

                        rightText = "KJ" + dateReceived;
                        price = Convert.ToDecimal("0.00");
                    }
                    else
                    {
                        leftText1 = "KV - " + Id + "/" + supplier;

                        for (int i = 0; i < categoryName.Length; i++)
                        {
                            if (i == 0)
                            {
                                line2 = line2 + categoryName[i];
                            }
                            else if (categoryName[i - 1] == ' ')
                            {
                                line2 = line2 + categoryName[i];
                            }

                        }
                        price = Convert.ToDecimal(inventory.Price);
                        if (price <= 0)
                        {
                            text3 = "KV/000/" + dateReceived;
                        }
                        else
                        {
                            text3 = "KV/" + string.Format("{0:#.00}", price).Replace(".", "") + "/" + dateReceived;
                        }
                    }
                }
                var image = barcoder.Encode(type, Id, Color.Black, Color.White, width, height);
                ImageConverter _imageConverter = new ImageConverter();
                byte[] barcodeImage = (byte[])_imageConverter.ConvertTo(image, typeof(byte[]));
                string imreBase64Data = Convert.ToBase64String(barcodeImage);
                string imgDataURL = string.Format("data:image/jpeg;base64,{0}", imreBase64Data);

                return Json(new
                {
                    data = imgDataURL,
                    value = inventory,
                    leftText1 = leftText1,
                    line2 = line2,
                    rightText = rightText,
                    text3 = text3,
                    price = price
                }, JsonRequestBehavior.AllowGet);
                //barcodeImage = barcoder.Encode(type, value, Color.Black, Color.White, 80, 30);
            }


            catch (Exception ex)
            {
                Log.Error("InventoryMasterController-GenerateBarcode-Exception", ex);
                return Json(new { data = "error" }, JsonRequestBehavior.AllowGet);
            }
        }

    }

}