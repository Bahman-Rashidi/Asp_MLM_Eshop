using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class PageGroup
    {
        #region constructor
        public PageGroup()
        {
            this.Pages = new HashSet<Page>();
        }
        #endregion

        #region Properties
        [Key]
        public int PageGroupId { get; set; }

        [StringLength(100)]
        [Display(Name ="Page group")]
        [Required(ErrorMessage = "Please enter {0} ")]
        public string PageGroupTitle { get; set; }

        public Nullable<int> langugaeId { get; set; }
        #endregion

        #region Relations
        //Navigation Properties
        public virtual ICollection<Page> Pages { get; set; }
        #endregion
    }
}