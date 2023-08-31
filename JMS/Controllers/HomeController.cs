using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class AuditData
    {
        public int NumberOfItemsInShelves { get; set; }
        public int NumberOfItemsInInventory { get; set; }
        public int DifferenceItems { get; set; }
    }

    public class SaleData
    {
        public string JewelTypeName { get; set; }
        public string CountValue { get; set; }
    }

    public class SaleByYearData
    {
        public string YearValue { get; set; }
        public string InvoiceCount { get; set; }
    }

    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(HomeController));

        public ActionResult Index()
        {            
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index","Login");
        }

        public ActionResult GetInventoryAuditJsonData()
        {
            try
            {
                InventoryAuditBAL inventoryAuditBALObj = new InventoryAuditBAL();
                var inventoryAudits = inventoryAuditBALObj.GetAllAuditData().Items;

                return Json(new
                {
                    aaData = inventoryAudits
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("HomeController-GetInventoryAuditJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetCategoryWiseSalesJsonData()
        {
            try
            {
                HomeBAL obj = new HomeBAL();
                var salesData = obj.GetAllSalesData();

                var lstSalesData = new List<SaleData>();
                foreach(var item in salesData)
                {
                    lstSalesData.Add(new SaleData() { JewelTypeName = item.Key, CountValue = item.Value });
                }
                return Json(new
                {
                    aaData = lstSalesData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("HomeController-GetCategoryWiseSalesJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult GetSalesByYearJsonData()
        {
            try
            {
                HomeBAL obj = new HomeBAL();
                var salesByYearData = obj.GetAllSalesDataByYear();

                var lstSalesByYearData = new List<SaleByYearData>();
                foreach (var item in salesByYearData)
                {
                    lstSalesByYearData.Add(new SaleByYearData() { YearValue = item.Key, InvoiceCount = item.Value });
                }
                return Json(new
                {
                    aaData = lstSalesByYearData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error("HomeController-GetSalesByYearJsonData-Exception", ex);
                return Json(new { data = ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }

      
    }
}