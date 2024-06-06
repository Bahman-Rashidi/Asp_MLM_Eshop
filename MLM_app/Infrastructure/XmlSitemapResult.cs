using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;
using MLM_app.Infrastructure;

namespace MLM_app.Infrastructure
{
    public class XmlSitemapResult : ActionResult
    {
        private static readonly XNamespace xmlns = "http://www.sitemaps.org/schemas/sitemap/0.9";
        private IEnumerable<ISitemapItem> _items;

        public XmlSitemapResult(IEnumerable<ISitemapItem> items)
        {
            _items = items;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            string encoding = context.HttpContext.Response.ContentEncoding.WebName;
            XDocument sitemap = new XDocument(new XDeclaration("1.0", encoding, "yes"),
             new XElement(xmlns + "urlset",
                  from item in _items
                  select CreateItemElement(item)
                  )
             );
            context.HttpContext.Response.ContentType = "application/rss+xml";
            context.HttpContext.Response.Flush();
            context.HttpContext.Response.Write(sitemap.Declaration + sitemap.ToString());
        }

        private XElement CreateItemElement(ISitemapItem item)
        {
            XElement itemElement = new XElement(xmlns + "url", new XElement(xmlns + "loc", item.Url.ToLower()));

            if (item.LastModified.HasValue)
                itemElement.Add(new XElement(xmlns + "lastmod", item.LastModified.Value.ToString("yyyy-MM-dd")));

            if (item.ChangeFrequency.HasValue)
                itemElement.Add(new XElement(xmlns + "changefreq", item.ChangeFrequency.Value.ToString().ToLower()));

            if (item.Priority.HasValue)
                itemElement.Add(new XElement(xmlns + "priority", item.Priority.Value.ToString(CultureInfo.InvariantCulture)));

            return itemElement;
        }
    }
}