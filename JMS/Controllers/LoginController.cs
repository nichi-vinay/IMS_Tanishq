using JMS.BAL.BussinesLogic;
using JMS.BAL.ViewModel;
using System.Web.Mvc;
using System.Linq;
using JMS.Common.Helper;
using System;
using log4net;

namespace JMS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        private static readonly ILog Log = LogManager.GetLogger(typeof(LoginController));
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginValidation(LoginViewModel model)
        {
            try
            {

                var passwordText = Helper.encrypt(model?.Password);
                var userBALobj = new UserBAL();
                var lstusers = userBALobj.GetAllUsers().Items;
                var result = lstusers.Where(x => x.UserName == model?.Email && x.Password == passwordText && x.Status == true).FirstOrDefault();
                if (result != null)
                {
                    var obj = new LoggedInUserModel()
                    {
                        Email = result.UserName,
                        UserName = result.Name,
                        Role = result.RoleId,
                        UserId = result.Id
                    };
                    Session["LoggedInUser"] = obj;                    

                    return Json(new { data = "Redirect", url = Url.Action("Index", "Invoice") });
                }
                else
                {
                    return Json(new { data = "Failed" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                Log.Error("LoginController-LoginValidation-Exception", ex);
                
                throw;
            }
        }
    }
}