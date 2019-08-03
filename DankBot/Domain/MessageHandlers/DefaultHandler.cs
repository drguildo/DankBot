namespace DankBot.Domain.MessageHandlers
{
    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;

    public class DefaultHandler : MessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        public DefaultHandler(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public override void Handle(Message message)
        {
            _logger.Information($"{message.Type} message received. No handler found.");
        }
    }
}