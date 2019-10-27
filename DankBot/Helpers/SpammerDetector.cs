namespace DankBot.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    using Serilog;

    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class Result
    {
        public int Offenses { get; set; }
        public List<string> Messages { get; set; }
        public int TimeAdded { get; set; }
    }

    public class Spammer
    {
        public bool Ok { get; set; }
        public Result Result { get; set; }
    }

    public class SpammerDetector : ISpammerDetector
    {
        private const string API_ENDPOINT = "https://combot.org/api/cas/check?user_id=";

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
        /// Uses the Combot Anti-Spam (CAS) API to determine whether an account is a known spammer.
        /// </summary>
        /// <param name="id">The Telegram ID of the account to check.</param>
        /// <returns>Whether the account is a known spammer.</returns>
        public async Task<bool> IsSpammerAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(API_ENDPOINT + id).ConfigureAwait(false);
                Spammer spammer = JsonConvert.DeserializeObject<Spammer>(json);
                return spammer.Ok;
            }
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