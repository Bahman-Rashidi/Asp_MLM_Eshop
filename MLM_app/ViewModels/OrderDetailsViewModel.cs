using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    public class OrderDetailsViewModel
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int OrderedCount { get; set; }
        public string ProductTitle { get; set; }
        public int Price { get; set; }
        public int Off { get; set; }
        [Display(Name = "Date Order")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public System.DateTime OrderDate { get; set; }

    }
}