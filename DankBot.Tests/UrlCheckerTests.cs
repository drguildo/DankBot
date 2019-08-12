namespace DankBot.Tests
{
    using System.Collections.Generic;

    using global::DankBot.Helpers;

    using Xunit;

    public class UrlCheckerTests
    {
        [Fact]
        public void UrlChecker_CheckWhiteListedHost_ShouldBeWhitelisted()
        {
            ICollection<string> whitelistHosts = new List<string>
            {
                "example.com"
            };

            var urlChecker = new UrlChecker(whitelistHosts);

            Assert.True(urlChecker.IsWhitelisted("http://example.com/"));
            Assert.True(urlChecker.IsWhitelisted("http://example.com/foo"));
            Assert.True(urlChecker.IsWhitelisted("https://example.com/"));
            Assert.True(urlChecker.IsWhitelisted("https://example.com/foo"));
        }

        [Fact]
        public void UrlChecker_CheckNonWhiteListedHost_ShouldNotBeWhitelisted()
        {
            ICollection<string> whitelistHosts = new List<string>
            {
                "example.com"
            };

            var urlChecker = new UrlChecker(whitelistHosts);

            Assert.False(urlChecker.IsWhitelisted("http://example.net/"));
            Assert.False(urlChecker.IsWhitelisted("http://example.net/foo"));
            Assert.False(urlChecker.IsWhitelisted("https://example.net/"));
            Assert.False(urlChecker.IsWhitelisted("https://example.net/foo"));
        }

        [Fact]
        public void UrlChecker_CheckAgainstEmptyWhitelist_ShouldNotBeWhitelisted()
        {
            ICollection<string> whitelistHosts = new List<string>();

            var urlChecker = new UrlChecker(whitelistHosts);

            Assert.False(urlChecker.IsWhitelisted("http://example.net/"));
        }
    }
}