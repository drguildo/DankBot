namespace DankBot.Domain.Handlers.Message
{
    using System.Diagnostics;

    using global::DankBot.Helpers;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class ChatMembersAddedHandler : IHandler<Message>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ISpammerDetector _spammerDetector;
        private readonly ILogger _logger;

        private readonly int _myId;

        public ChatMembersAddedHandler(ITelegramBotClient botClient, ISpammerDetector spammerDetector, ILogger logger)
        {
            _botClient = botClient;
            _spammerDetector = spammerDetector;
            _logger = logger;

            _myId = _botClient.GetMeAsync().Id;
        }

        public async void Handle(Message message)
        {
            Debug.Assert(message != null);

            if (message.Chat.Type != ChatType.Group)
            {
                return;
            }

            if (message.NewChatMembers != null)
            {
                foreach (User user in message.NewChatMembers)
                {
                    if (user.Id == _myId)
                    {
                        continue;
                    }

                    _logger.Information($"{Utilities.UserToString(user)} joined. Date is {message.Date}.");

                    if (await _spammerDetector.IsSpammerAsync(user.Id).ConfigureAwait(false))
                    {
                        _logger.Information($"SPAMMER: {user.Id}");
                        await _botClient.SendTextMessageAsync(message.Chat.Id, "Spammer detected. Removing...", replyToMessageId: message.MessageId).ConfigureAwait(false);
                        await _botClient.KickChatMemberAsync(message.Chat.Id, user.Id).ConfigureAwait(false);
                    }
                    else
                    {
                        _logger.Information($"NOT A KNOWN SPAMMER: {user.Id}");
                    }
                }
            }
        }
    }
}