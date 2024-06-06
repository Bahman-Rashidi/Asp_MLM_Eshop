using System.Web;
using System.Web.Mvc;
using Elmah;

namespace MLM_app
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            // Exception handling for Submit in Elmah should be written first in the code.
            filters.Add(new ElmahHandledErrorLoggerFilter());

            filters.Add(new HandleErrorAttribute());
        }

        // Exception handling
        public class ElmahHandledErrorLoggerFilter : IExceptionFilter
        {
            public void OnException(ExceptionContext context)
            {
                if (context.ExceptionHandled)
                    ErrorSignal.FromCurrentContext().Raise(context.Exception);
                // all other exceptions will be caught by ELMAH anyway
            }
        }
    }
}
