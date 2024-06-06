using System;
using MLM_app.Infrastructure;

namespace MLM_app.Infrastructure
{
    public class SitemapItem : ISitemapItem
    {
        public SitemapItem(string url, DateTime? lastModified, ChangeFrequency? changeFrequency, float? priority)
        {
            this.url = url;
            this.lastModified = lastModified;
            this.changeFrequency = changeFrequency;
            this.priority = priority;
        }

        private string url;
        private DateTime? lastModified;
        private ChangeFrequency? changeFrequency;
        private float? priority;

        public string Url
        {
            get { return this.url; }
        }

        public DateTime? LastModified
        {
            get { return this.lastModified; }
        }

        public ChangeFrequency? ChangeFrequency
        {
            get { return this.changeFrequency; }
        }

        public float? Priority
        {
            get { return this.priority; }
        }
    }
}