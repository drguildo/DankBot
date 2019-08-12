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
            return _whitelistHosts.Contains(uri.Host);
        }
    }
}