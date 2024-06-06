using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    /// <summary>
    /// We use this class to display the shopping cart to the user
    /// </summary>
    public class ShoppingCartViewModel
    {
        [Key]
        public int ProductID { get; set; }

        [Display(Name ="Product Name")]
        public string ProductTitle { get; set; }

        [Display(Name = "Product price")]
        [DisplayFormat(DataFormatString = "{0:#,0 $}")]
        public int ProductPrice { get; set; }

        [Display(Name = "Quantity")]
        [DisplayFormat(DataFormatString = "{0:#,0 عدد}")]
        public int ProductCount { get; set; }

        [Display(Name = "Sum")]
        [DisplayFormat(DataFormatString = "{0:#,0 $}")]
        public int RowTotal { get; set; }

        public int Off { get; set; }
    }
}