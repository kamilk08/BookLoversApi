using System;

namespace BookLovers.Base.Infrastructure.Validation
{
    public static class UrlValidator
    {
        public static bool IsValidUrl(string webSite)
        {
            return Uri.TryCreate(webSite, UriKind.RelativeOrAbsolute, out _);
        }
    }
}