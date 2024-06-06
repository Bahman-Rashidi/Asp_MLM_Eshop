using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MLM_app.Models
{
    public class Page
    {
        #region constructor
        public Page()
        {
            this.Comments = new HashSet<Comment>();
        }
        #endregion

        #region Properties
        [Key]
        public int PageId { get; set; }
        [Display(Name ="Page group")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public int PageGroupId { get; set; }
        public Nullable<int> langugaeId { get; set; }
        public string ApplicationUserId { get; set; }

        [Display(Name = "Page Title")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [StringLength(500)]
        public string PageTitle { get; set; }

        [Display(Name ="News Summary")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string PageSummary { get; set; }

        [Display(Name = "Page Content")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string PageText { get; set; }

        [Display(Name = "News display status")]
        public bool PageIsActive { get; set; }

        [Display(Name = "Comment display status")]
        public bool CommentIsActive { get; set; }

        [Display(Name = "Page Creation Date")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        [UIHint("DateTimePicker")]
        public DateTime PageDate { get; set; }


        public int VisitCount { get; set; }

        public int Like { get; set; }
        public int DisLike { get; set; }
        #endregion

        #region Relations
        //Navigation Properties
        public virtual PageGroup PageGropus { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        #endregion
    }
}