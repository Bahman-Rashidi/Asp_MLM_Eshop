using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class OrderDetail
    {
        #region
        public OrderDetail()
        {

        }
        #endregion

        #region Properties
        [Key]
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int OrderedCount { get; set; }
        public int Off { get; set; }
        public int Price { get; set; }
        #endregion

        #region Relations
        //Navigation Properties
        public virtual Order Orders { get; set; }
        public virtual Product Products { get; set; }
        #endregion
    }
}