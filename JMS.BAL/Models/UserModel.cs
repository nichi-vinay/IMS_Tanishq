using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.BAL.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string RoleName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string UserName { get; set; }        
        public bool Status { get; set; }
    }
}