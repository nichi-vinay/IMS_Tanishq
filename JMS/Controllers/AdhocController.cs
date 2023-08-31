using JMS.BAL.ViewModel;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JMS.Controllers
{
    public class AdhocController : Controller
    {
        // GET: Adhoc
        private static readonly ILog Log = LogManager.GetLogger(typeof(AdhocController));
        public ActionResult Index()
        {
            try
            {
                var adhocViewModel = new AdhocViewModel()
                {
                    SelectedCompany = 1,
                    SelectedCategory = 1,
                    SelectedJewelType = 1
                };
                return View(adhocViewModel);
            }
            catch (Exception ex)
            {
                Log.Error("AdhocController-Index-Exception", ex);
                throw;
            }
        }
    }
}