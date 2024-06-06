using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MLM_app.Models
{
    public class Order
    {
        #region constructor
        public Order()
        {
          
            this.OrderDetails = new HashSet<OrderDetail>();
            this.Payments = new HashSet<Payment>();
        }
        #endregion

        #region Properties
        [Key]
        [Display(Name = "Order number")]
        public int OrderID { get; set; }
        public string ApplicationUserId { get; set; }
        [Display(Name = "Date Order")]
        public System.DateTime OrderDate { get; set; }
        [Display(Name = "status Order")]
        public bool IsFinalized { get; set; }

        [Display(Name = "Method of receipt")]
        [Required(ErrorMessage = "Please enter {0} ")]

        public string ReciptionMethod { get; set; }



        [Display(Name = "Shipping cost")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#,0 $}")]
        [UIHint("Integer")]
        public int ReciptionPrice { get; set; }


        [Display(Name = "Duplicated in the bank")]
        public bool IsDuplicatedBank { get; set; }

        #endregion        
        
        #region Relations
        //Navigation Properties
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ApplicationUser ApplicationUser { get; set; }
        #endregion

    }
}