namespace DankBot.Helpers
{
    using System;
    using System.Collections.Generic;

    public class UrlChecker
    {
        private readonly ICollection<string> _whitelistHosts;

        public UrlChecker(ICollection<string> whitelistHosts)
        {
            _whitelistHosts = whitelistHosts;
        }

        public bool IsWhitelisted(string url)
        {
            var uri = new Uri(url);

            if (uri.Host.StartsWith("www."))
            {
                return _whitelistHosts.Contains(uri.Host.Substring(4));
            }

            return _whitelistHosts.Contains(uri.Host);
        }
    }
}