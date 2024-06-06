using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.Models
{
    public class Comment
    {
        #region constructor
        public Comment()
        {
            
        }
        #endregion

        #region Properties
        [Key]
        public int CommentId { get; set; }

        public Nullable<int> PageID { get; set; }

        public Nullable<int> ProductID { get; set; }

        public Nullable<int> SliderId { get; set; }

        public Nullable<int> langugaeId { get; set; }

        public int? ParentId { get; set; }

        [Display(Name = "Date Send")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime CommentDate { get; set; }

        [Display(Name = "Display comment status.")]
        public bool CommentIsActive { get; set; }

        [Display(Name = "Content")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string CommentContent { get; set; }

        [StringLength(100)]
        [Display(Name = "Sender Name")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string Author { get; set; }

        [StringLength(200)]
        [Display(Name = "Emal")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "The format of {0} entered is incorrect")]
        //[EmailAddress(ErrorMessage="")]
        public string AuthorEmail { get; set; }
        #endregion

        #region Relations
        //Navigation Properties
        public virtual Page Pages { get; set; }
        public virtual Product Products { get; set; }
        public virtual Comment Parent { get; set; }
        #endregion
    }
}