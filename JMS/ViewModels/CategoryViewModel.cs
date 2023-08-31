using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JMS.ViewModels
{
   
        public class CategoryViewModel
        {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNumber { get; set; }
        public bool Status { get; set; }
        public int SelectedStatus { get; set; }
        public int SelectedCategory { get; set; }

        public List<CategoryModel> Items { get; set; }
        }

        public class CategoryModel
        {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public string CategoryNumber { get; set; }
        public bool Status { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? MISC1 { get; set; }
        public string MISC2 { get; set; }

    }
    }
