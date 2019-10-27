namespace DankBot.Domain.Handlers.Message
{
    using System.Diagnostics;
    using System.Linq;

    using global::DankBot.Helpers;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;

    public class TextHandler : IHandler<Message>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ISpammerDetector _spammerDetector;
        private readonly ILogger _logger;

        public TextHandler(ITelegramBotClient botClient, ISpammerDetector spammerDetector, ILogger logger)
        {
            _botClient = botClient;
            _spammerDetector = spammerDetector;
            _logger = logger;
        }

        public async void Handle(Message message)
        {
            Debug.Assert(message != null);

            if (message.From.Id == _botClient.BotId)
            {
                // Don't process messages sent by us.
                return;
            }

            if (message.Text != null)
            {
                _logger.Information($"{Utilities.UserToString(message.From)}: {message.Text}");
            }

            if (_spammerDetector.IsSpam(message))
            {
                // Don't ban admins.
                ChatMember[] admins = await _botClient.GetChatAdministratorsAsync(message.Chat.Id).ConfigureAwait(false);
                if (admins.Select(a => a.User.Id).Contains(message.From.Id))
                {
                    _logger.Information($"Ignoring spam from admin {message.From.Id}");
                    return;
                }

                await _botClient.DeleteMessageAsync(message.Chat.Id, message.MessageId).ConfigureAwait(false);
                await _botClient.KickChatMemberAsync(message.Chat.Id, message.From.Id).ConfigureAwait(false);
                await _botClient.SendTextMessageAsync(message.Chat.Id, $"Banned spammer {Utilities.UserToString(message.From)}.").ConfigureAwait(false);
            }
        }
    }
}