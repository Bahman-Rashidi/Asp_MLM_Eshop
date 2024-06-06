using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;
namespace MLM_app.Models
{
    public class ReciptionMethod
    {

        #region constructor
        public ReciptionMethod()
        {
            
        }
        #endregion

        #region Properties
        [Key]
        public int ReciptionMethodId { get; set; }


        [Display(Name = "Method of receipt en")]

        public string EnReceptionMethodName { get; set; }


        [Display(Name = "Description")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string ReceptionMethodText { get; set; }


        [Display(Name = "Cost of delivery")]
      [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [UIHint("Integer")]
        public int Price { get; set; }

        #endregion

    }
}