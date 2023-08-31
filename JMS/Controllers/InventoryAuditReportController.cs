using JMS.BAL.BussinesLogic;
using JMS.ViewModels;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class InventoryAuditReportController : Controller
    {
        

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult FetchInventoryDetails(int Id)
        {
            try
            {
                // GET: InventoryAudit
                var inventoryBal = new InventoryBAL();
                var inventoryDetails = inventoryBal.GetInventoryById(Id);

                if (inventoryDetails != null)
                {
                    
                    return Json(inventoryDetails, JsonRequestBehavior.AllowGet);
                }
                else
                {
                   
                    return Json(new { error = "Inventory not found" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
             
                return Json(new { error = "Error occurred while fetching inventory details" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}




