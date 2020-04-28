namespace DankBot.Helpers
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    using Serilog;

    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class SpammerDetector : ISpammerDetector
    {
        private readonly ICollection<string> _spamUrlRegularExpressions;
        private readonly ILogger _logger;

        public SpammerDetector(ILogger logger)
        {
            _logger = logger;
            _spamUrlRegularExpressions = new List<string>();
        }

        public SpammerDetector(ILogger logger, ICollection<string> spamUrlRegularExpressions)
        {
            _logger = logger;
            _spamUrlRegularExpressions = spamUrlRegularExpressions;
        }

        /// <summary>
        /// Checks the given Telegram message for indications that it is spam.
        /// </summary>
        /// <param name="message">The message to check.</param>
        /// <returns>Whether the given message is considered spam.</returns>
        public bool IsSpam(Message message)
        {
            if (message.Entities == null)
            {
                return false;
            }

            foreach (var messageEntity in message.Entities)
            {
                string entity = message.Text.Substring(messageEntity.Offset, messageEntity.Length);

                if (messageEntity.Type == MessageEntityType.Url)
                {
                    foreach (string spamUrlRegularExpression in _spamUrlRegularExpressions)
                    {
                        if (Regex.IsMatch(entity, spamUrlRegularExpression))
                        {
                            Log($"Message ID: {message.MessageId}, Sender ID: {message.From.Id}, URL: {entity}");
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private void Log(string msg) => _logger.Information($"{nameof(SpammerDetector)}: {msg}");
    }
}