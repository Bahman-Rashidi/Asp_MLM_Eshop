using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http.WebHost;
using System.Web.Routing;
using System.Web.SessionState;


namespace MLM_app.Infrastructure
{
    public class Utils
    {
        #region Create Thumbnail
        public static string CreateThumbnail(string OriginalFileFullPath)
        {
            string imageUploadPath = "/Images/Uploads/Products/";
            string filename = string.Empty;

            if (File.Exists(OriginalFileFullPath))
            {
                Image img = Bitmap.FromFile(OriginalFileFullPath);
                Bitmap bmp = new Bitmap(img);

               // bmp = BitmapManipulator.ThumbnailBitmap(bmp, 150, 150);
                bmp = BitmapManipulator.ThumbnailBitmap(bmp, 200, 200);
                string thumbfilename = Path.GetFileNameWithoutExtension(OriginalFileFullPath) + "_Thumb" + Path.GetExtension(OriginalFileFullPath);

                string thumb_file_relative_path = imageUploadPath + thumbfilename;

                bmp.Save(HttpContext.Current.Server.MapPath(thumb_file_relative_path), System.Drawing.Imaging.ImageFormat.Jpeg);
                img.Dispose();
                bmp.Dispose();
                filename = thumb_file_relative_path;
            }
            return filename;
        }

        #endregion

        #region Working with sessions in Web APIs
        /************************************/
        // To work with sessions and access sessions in Web APIs
        public class MyHttpControllerHandler : HttpControllerHandler, IRequiresSessionState
        {
            public MyHttpControllerHandler(RouteData routeData)
                : base(routeData)
            {
            }
        }
        public class MyHttpControllerRouteHandler : HttpControllerRouteHandler
        {
            protected override IHttpHandler GetHttpHandler(RequestContext requestContext)
            {
                return new MyHttpControllerHandler(requestContext.RouteData);
            }
        }

        #endregion
    }
}