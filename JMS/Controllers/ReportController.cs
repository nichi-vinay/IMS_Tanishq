using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using JMS.Common.Helper;
using JMS.Models;
using log4net;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class AuditData1
    {
        public bool IsDataExist { get; set; }
        public List<int> ListOfItemsFromShelves { get; set; }
        public List<int> ListOfItemsFromInventory { get; set; }

        public List<int> ListOfDifferenceItems { get; set; }
        public List<InventoryModel> ListOfDiffrenceItemsFromShelves { get; set; }
        public List<InventoryModel> ListOfDifferenceItemsFromInventory { get; set; }
    }


    public class ReportController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(ReportController));

        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AuditReport()
        {
            var obj = new AuditData1();            
            obj.ListOfDiffrenceItemsFromShelves = new List<InventoryModel>();
            obj.ListOfDifferenceItemsFromInventory = new List<InventoryModel>();
            return View(obj);
        }


        [HttpPost]
        public ActionResult AuditReport(HttpPostedFileBase file)
        {
            var inventoryBalObj = new InventoryBAL();
            var obj = new AuditData1();

            if (file != null && file.ContentLength > 0)
            {
                try
                {
                    // extract only the fielname
                    var fileName = Path.GetFileName(file.FileName);
                    // store the file inside ~/App_Data/uploads folder
                    var path = Path.Combine(Server.MapPath("~/App_Data/"), fileName);
                    file.SaveAs(path);

                    var listOfItemsFromShelves = GetDataFromUploadedCSVFile(path);
                    listOfItemsFromShelves = listOfItemsFromShelves.Distinct().ToList();

                    var inStockInventoryItems = inventoryBalObj.GetAllInventory().Items;

                    var listOfItemsFromInventory = inStockInventoryItems.Select(x => x.Id).ToList();

                    var listOfDifferenceItemsFromInventory = listOfItemsFromShelves.Except(listOfItemsFromInventory).ToList(); //list3 contains only 1, 2
                    var listOfDiffrenceItemsFromShelves = listOfItemsFromInventory.Except(listOfItemsFromShelves).ToList(); //list4 contains only 6, 7
                    var listOfDifferenceItems = listOfDiffrenceItemsFromShelves.Concat(listOfDifferenceItemsFromInventory).ToList(); //resultList contains 1, 2, 6, 7

                    var listDiffItemsFromInventory = new List<InventoryModel>();
                    foreach(var item in listOfDifferenceItemsFromInventory)
                    {
                        var diffInventoryModelObj = new InventoryModel();

                        diffInventoryModelObj = inventoryBalObj.GetInventoryById(item);

                        if(diffInventoryModelObj != null)
                        listDiffItemsFromInventory.Add(diffInventoryModelObj);
                    }

                    obj = new AuditData1()
                    {
                        ListOfItemsFromShelves = listOfItemsFromShelves,
                        ListOfItemsFromInventory = listOfItemsFromInventory,
                        ListOfDifferenceItems = listOfDifferenceItems,
                        ListOfDiffrenceItemsFromShelves = inStockInventoryItems.Where(x => listOfDiffrenceItemsFromShelves.Contains(x.Id)).ToList(),
                        ListOfDifferenceItemsFromInventory = listDiffItemsFromInventory
                    };

                  var  obj1 = new InventoryAuditModel
                    {
                        AuditDate=DateTime.Now.ToString(),
                        ItemsInShelves = listOfItemsFromShelves.Count,
                        ItemsInInventory = listOfItemsFromInventory.Count,
                        VarianceItems = listOfDifferenceItems.Count,
                        VarianceItemsInShelves = obj.ListOfDiffrenceItemsFromShelves.Count,
                        VarianceItemsInInventory = obj.ListOfDifferenceItemsFromInventory.Count
                    };
                    var inventoryAuditBAl = new InventoryAuditBAL();
                    inventoryAuditBAl.AddAuditData(obj1);
                    if (obj.ListOfDifferenceItemsFromInventory.Count > 0 || listOfDiffrenceItemsFromShelves.Count > 0)
                    {
                        obj.IsDataExist = true;

                    }


                    else
                        obj.IsDataExist = false;
                }

                catch (Exception ex)
                {
                    Log.Error("ReportController-AuditReport-Exception", ex);
                }
            }

            return View(obj);

        }
        private List<int> GetDataFromUploadedCSVFile(string fileName)
        {
            try
            {
                List<int> lstItems = new List<int>();

                using (var reader = new StreamReader(fileName))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        lstItems.Add(Convert.ToInt32(line));
                    }
                }

                return lstItems;
            }
            catch (Exception ex)
            {
                Log.Error("ReportController-GetDataFromUploadedCSVFile-Exception", ex);
                return null;
            }
        }
    }
}