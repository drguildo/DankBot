namespace DankBot.Domain.Handlers.Update
{
    using Serilog;

    using Telegram.Bot.Types;

    public class DefaultUpdateHandler : IHandler<Update>
    {
        private readonly ILogger _logger;

        public DefaultUpdateHandler(ILogger logger)
        {
            _logger = logger;
        }

        public void Handle(Update update)
        {
            _logger.Information($"{update.Type} update received. No handler found.");
        }
    }
}