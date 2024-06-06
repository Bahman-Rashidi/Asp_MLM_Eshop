using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class ProductGroup
    {
        #region constructor
        public ProductGroup()
        {
            this.Products = new HashSet<Product>();
        }
        #endregion

        #region Properties
        [Key]
        public int ProductGroupID { get; set; }

        [Display(Name = "Parent group")]
        public int? ParentId { get; set; }

        [StringLength(100)]
        [Display(Name = "Product group")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string ProductGroupTitle { get; set; }

        [Display(Name = "Name In System")]
        public string NameInSystem { get; set; }

        [StringLength(150)]
        [Display(Name = "Category image")]
        [UIHint("Upload")]
        public string ImageUrl { get; set; }

        public Nullable<int> langugaeId { get; set; }
        #endregion

        #region Relations
        //Navigation Properties
        public virtual ICollection<Product> Products { get; set; }
        public virtual ProductGroup Parent { get; set; }
        #endregion

    }
}