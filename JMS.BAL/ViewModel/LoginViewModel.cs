using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JMS.BAL.ViewModel
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
        public int UserId { get; set; }
    }
}
