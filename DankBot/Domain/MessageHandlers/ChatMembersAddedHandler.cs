namespace DankBot.Domain.MessageHandlers
{
    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;

    public class ChatMembersAddedHandler : MessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        private readonly int _myId;

        public ChatMembersAddedHandler(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;

            _myId = _botClient.GetMeAsync().Id;
        }

        public override void Handle(Message message)
        {
            if (message?.NewChatMembers != null)
            {
                foreach (User user in message.NewChatMembers)
                {
                    if (user.Id == _myId)
                    {
                        continue;
                    }

                    _logger.Information($"{MessageHandler.UserToString(user)} joined. Date is {message.Date}.");

                    if (user.IsBot)
                    {
                        _logger.Information($"{MessageHandler.UserToString(user)} is a bot!!1");
                    }
                }
            }
        }
    }
}