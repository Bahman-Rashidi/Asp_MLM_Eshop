using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.ViewModels
{
    public class CommentViewModel
    {

        public Nullable<int> PageID { get; set; }

        public Nullable<int> ProductID { get; set; }

        public Nullable<int> SliderId { get; set; }

        public Nullable<int> ParentId { get; set; } 
        [Display(Name = "Date Send")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime CommentDate { get; set; }
      
        [Display(Name = "Sender Name")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string Author { get; set; }


        [Display(Name = "Content")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string CommentContent { get; set; }


        [Display(Name = "Emal")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "the format of {0} is incorrect")]
        //[EmailAddress(ErrorMessage="")]
        public string AuthorEmail { get; set; }



        [System.ComponentModel.DataAnnotations.Schema.NotMapped]
        [Required(ErrorMessage = "Please enter {0} ")]
        [Display(Name = "ResultOfSum")]
        public string Captcha { get; set; }
    }
}