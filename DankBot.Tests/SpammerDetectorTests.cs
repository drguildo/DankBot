﻿namespace DankBot.Tests
{
    using NSubstitute;

    using Serilog;

    using Xunit;

    public class SpammerDetectorTests
    {
        [Fact]
        public void SpammerDetector_CheckKnownSpammerId_ShouldReturnTrue()
        {
            var spammerDetector = new Helpers.SpammerDetector(Substitute.For<ILogger>());

            bool isSpammer = spammerDetector.IsSpammerAsync(821063044).Result;

            Assert.True(isSpammer);
        }

        [Fact]
        public void SpammerDetector_CheckNonSpammerId_ShouldReturnFalse()
        {
            var spammerDetector = new Helpers.SpammerDetector(Substitute.For<ILogger>());

            bool isSpammer = spammerDetector.IsSpammerAsync(46059387).Result;

            Assert.False(isSpammer);
        }
    }
}