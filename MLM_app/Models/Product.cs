using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MLM_app.Infrastructure;

namespace MLM_app.Models
{
    public class Product
    {
        #region constructor
        public Product()
        {
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Comments = new HashSet<Comment>();

            Infrastructure.Utility oUtility = new Utility();
            InsertDateTime = oUtility.DateMilady();
        }
        #endregion

        #region Properties
        [Key]
        public int ProductID { get; set; }

        [Display(Name = "Product group")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public int ProductGroupID { get; set; }


        public Nullable<int> langugaeId { get; set; }

        [Display(Name = "Product status")]
        public bool ProductStatus { get; set; }

        [StringLength(150)]
        [Display(Name = "Product title")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string ProductTitle { get; set; }

        [Display(Name = "Product description")]
        [Required(ErrorMessage = "Please enter {0} ")]
        [UIHint("Ckeditor")]
        [AllowHtml]
        public string ProductDescription { get; set; }

        [Display(Name = "Product price")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [Required(ErrorMessage = "Please enter {0} ")]
        //[UIHint("Integer")]
        public int ProductPrice { get; set; }

        [StringLength(150)]
        [Display(Name = "Product image")]
        [UIHint("Upload")]
        public string ImageUrl { get; set; }

        [StringLength(150)]
        [Display(Name = "Thumbnail")]
        public string ProductThumbnailImageUrl { get; set; }

        [Display(Name = "Comment display status")]
        public bool CommentIsActive { get; set; }

        [Display(Name ="Discount")]
        public int Off { get; set; }

        public int VisitCount { get; set; }

        public int Like { get; set; }
        public int DisLike { get; set; }

        public DateTime InsertDateTime { get; set; }

        [Display(Name = "Display license")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public String LicenseNumber { get; set; }

        [Display(Name = "In stock count")]
        public int AvailableNumber { get; set; }

        [Display(Name = "Comma-separated list of keywords")]
        public string KeyWords { get; set; }

        #endregion

        #region Relations
        //Navigation Properties
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ProductGroup ProductGroups { get; set; }
        #endregion
    }
}