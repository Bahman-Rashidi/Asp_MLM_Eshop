using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM_app.ViewModels
{
    public class PeymentProcessVm
    {
        public int PaymentId { get; set; }
        public long Amount { get; set; }

        public string ApplicationUserId { get; set; }
 
        public int OrderId { get; set; }
        public long SaleOrderId { get; set; }
        public DateTime InsertDateTime { get; set; }





    }
}