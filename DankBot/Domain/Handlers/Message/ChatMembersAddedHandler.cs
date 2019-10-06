namespace DankBot.Domain.Handlers.Message
{
    using System.Diagnostics;

    using global::DankBot.Helpers;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;

    public class ChatMembersAddedHandler : IHandler<Message>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;
        private readonly Spammers _spammers;

        private readonly int _myId;

        public ChatMembersAddedHandler(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;

            _myId = _botClient.GetMeAsync().Id;

            _spammers = new Spammers(_logger);
        }

        public void Handle(Message message)
        {
            Debug.Assert(message != null);

            if (message.NewChatMembers != null)
            {
                foreach (User user in message.NewChatMembers)
                {
                    if (user.Id == _myId)
                    {
                        continue;
                    }

                    _logger.Information($"{Helpers.UserToString(user)} joined. Date is {message.Date}.");

                    if (user.IsBot)
                    {
                        _logger.Information($"{Helpers.UserToString(user)} is a bot!!1");
                    }

                    if (_spammers.IsSpammer(user.Id))
                    {
                        _logger.Information($"{Helpers.UserToString(user)} is a spammer!!1");
                    }
                }
            }
        }
    }
}