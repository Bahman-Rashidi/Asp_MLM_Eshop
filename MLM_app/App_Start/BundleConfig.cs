using System.Web;
using System.Web.Optimization;

namespace MLM_app
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/css").Include(
         // "~/Content/bootstrap-rtl.css",
         "~/Content/bootstrap.css",

          //"~/Content/font-awesome.css",
          //"~/Content/prettyPhoto.css",
          //"~/Content/price-range.css",
          //"~/Content/animate.css",
          "~/Content/main.css",
          "~/Content/PagedList.css",
          "~/Content/eshoper.css",
          "~/Content/responsive.css",
          "~/Content/site.css"
          
          ));



            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/jquery.js",
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery.scrollUp.js",
                      "~/Scripts/price-range.js",
                      "~/Scripts/jquery.prettyPhoto.js",
                      "~/Scripts/main.js",
                      "~/Scripts/eshoper.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrapNew").Include(

                      "~/Scripts/bootstrap.js",
                      "~/Scripts/jquery.scrollUp.js",
                      "~/Scripts/price-range.js",
                      "~/Scripts/jquery.prettyPhoto.js",
                      "~/Scripts/main.js",
                      "~/Scripts/eshoper.js",
                      "~/Scripts/respond.js"));


            bundles.Add(new StyleBundle("~/Content/cssLTR").Include(
                "~/Content/bootstrap.min.css",
                "~/Content/font-awesome.css",
                "~/Content/prettyPhoto.css",
                "~/Content/price-range.css",
                "~/Content/animate.css",
                "~/Content/main.css",
                "~/Content/PagedList.css",
                "~/Content/eshoper.css",
                "~/Content/responsive.css",
                "~/Content/site.css"));
        }
    }
}
