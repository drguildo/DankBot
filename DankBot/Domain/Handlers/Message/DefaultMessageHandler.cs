namespace DankBot.Domain.Handlers.Message
{
    using Serilog;
    using Telegram.Bot.Types;

    public class DefaultMessageHandler : IHandler<Message>
    {
        private readonly ILogger _logger;

        public DefaultMessageHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(Message message)
        {
            _logger.Information($"{message.Type} message received. No handler found.");
        }
    }
}