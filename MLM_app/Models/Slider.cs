using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;

namespace MLM_app.Models
{
    [System.ComponentModel.DataAnnotations.Schema.Table("Sliders")]
    [DisplayName("Sliders")]
    public class Slider : System.Object
    {
        
        #region constructor
        /// <summary>
        /// constructor
        /// </summary>
        public Slider()
        {
            Infrastructure.Utility oUtility = new Utility();
            InsertDateTime = oUtility.DateMilady();

        }
        #endregion end  constructor

        #region Slider Table Fields
        //########################################|SliderId|###########################################       
        [System.ComponentModel.DataAnnotations.Key]
        [System.ComponentModel.DataAnnotations.Required]
        // Change the field name as follows
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("SliderId", Order = 0, TypeName = "int")]
        // These two attributes exclude the field from auto-incrementing property
        //[System.ComponentModel.DataAnnotations.Schema.DatabaseGenerated
        //    (System.ComponentModel.DataAnnotations.Schema.DatabaseGeneratedOption.None)]
        public int SliderId { get; set; }
        //###################################################################################################        
        public Nullable<int> langugaeId { get; set; }

        //########################################|Image|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(2000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("ImageUrl", Order = 1, TypeName = "varchar")]
        [Display(Name = "Image")]
        [UIHint("Upload")]
        public string ImageUrl { get; set; }
        //###################################################################################################

        //########################################|Title|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(2000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Title", Order = 2, TypeName = "nvarchar")]

        [DataType(DataType.MultilineText)]
        [Display(Name = "Title")]
        public string Title { get; set; }
        //###################################################################################################

        //########################################|link|#######################################################        
        [System.ComponentModel.DataAnnotations.MaxLength(2000)]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Link", Order = 3, TypeName = "nvarchar")]
        [Display(Name = "Link")]
        public string Link { get; set; }
        //###################################################################################################

        //########################################|link|#######################################################        
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("Order", Order = 4, TypeName = "int")]
        [Display(Name = "Display Order")]
        public int Order { get; set; }
        //###################################################################################################

        //########################################|InsertDateTime|############################################
        [System.ComponentModel.DataAnnotations.Required]
        [System.ComponentModel.DataAnnotations.Schema.Column
        ("InsertDateTime", Order = 5, TypeName = "datetime")]
        [Display(Name = "Submit Date and Time")]
        public DateTime InsertDateTime { get; set; }
        //################################################################################################### 

        [Display(Name = "Page Content")]
        [Required(ErrorMessage = "Please enter {0}")]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string PageText { get; set; }

        public string ApplicationUserId { get; set; }

        [Display(Name ="News Summary")]
        [Required(ErrorMessage = "Please enter {0}")]
        public string SilderSummary { get; set; }

        [Display(Name = "Comment display status")]
        public bool CommentIsActive { get; set; }

        [Display(Name = "News display status")]
        public bool SilderIsActive { get; set; }


        public int VisitCount { get; set; }

        public int Like { get; set; }
        public int DisLike { get; set; }
        #endregion

        #region Table Relationships
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        #endregion
    }
}