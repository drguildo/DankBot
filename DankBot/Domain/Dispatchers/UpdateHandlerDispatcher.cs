namespace DankBot.Domain.Dispatchers
{
    using System.Collections.Generic;

    using global::DankBot.Domain.Handlers;
    using global::DankBot.Domain.Handlers.Update;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class UpdateHandlerDispatcher
    {
        private readonly IHandler<Update> _defaultHandler;
        private readonly Dictionary<UpdateType, IHandler<Update>> _lookup;
        private readonly ILogger _logger;

        public UpdateHandlerDispatcher(ITelegramBotClient botClient, ILogger logger)
        {
            _logger = logger;

            _defaultHandler = new DefaultUpdateHandler(_logger);
            _lookup = new Dictionary<UpdateType, IHandler<Update>>
            {
                { UpdateType.Message, new MessageHandler(botClient, _logger) }
            };
        }

        public void Dispatch(Update update)
        {
            _logger.Information($"{update.Type} update received with ID {update.Id}.");

            if (!_lookup.TryGetValue(update.Type, out IHandler<Update> handler))
            {
                _defaultHandler.Handle(update);
            }
            else
            {
                handler.Handle(update);
            }
        }
    }
}