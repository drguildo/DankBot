namespace DankBot.Domain.Handlers.Update
{
    using global::DankBot.Domain.Dispatchers;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;

    public class MessageHandler : IHandler<Update>
    {
        private readonly MessageHandlerDispatcher _messageHandlerDispatcher;
        private readonly ILogger _logger;

        public MessageHandler(ITelegramBotClient botClient, ILogger logger)
        {
            _logger = logger;

            _messageHandlerDispatcher = new MessageHandlerDispatcher(botClient, _logger);
        }

        public void Handle(Update update)
        {
            if (update.Message == null)
            {
                _logger.Error($"{nameof(MessageHandler)} called but {nameof(update.Message)} is null.");
                return;
            }

            _messageHandlerDispatcher.Dispatch(update.Message);
        }
    }
}