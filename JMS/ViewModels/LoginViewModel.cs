using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
    public class LoginViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class LoggedInUserModel
    {
        public string Email { get; set; }
        public string UserName { get; set; }
        public int Role { get; set; }
    }
}