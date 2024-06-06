using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MLM_app.Infrastructure
{
    public class RegularExpressions
    {
        // Note: Wrong Usage!
        //public string Email =
        //	@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        // Note: Wrong Usage!
        //public static string Email =
        //	@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        public const string Email =
            @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
    }
}