using System;
using MLM_app.Infrastructure;

namespace MLM_app.Infrastructure
{
    public interface ISitemapItem
    {
        string Url { get; }
        DateTime? LastModified { get; }
        ChangeFrequency? ChangeFrequency { get; }
        float? Priority { get; }
    }
}
