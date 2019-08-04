namespace DankBot.Domain.Dispatchers
{
    using System.Collections.Generic;

    using global::DankBot.Domain.Handlers;
    using global::DankBot.Domain.Handlers.Message;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class MessageHandlerDispatcher
    {
        private readonly IHandler<Message> _defaultHandler;
        private readonly Dictionary<MessageType, IHandler<Message>> _lookup;
        private readonly ILogger _logger;

        public MessageHandlerDispatcher(ITelegramBotClient botClient, ILogger logger)
        {
            _logger = logger;

            _defaultHandler = new DefaultMessageHandler(_logger);
            _lookup = new Dictionary<MessageType, IHandler<Message>>
            {
                { MessageType.ChatMembersAdded, new ChatMembersAddedHandler(botClient, _logger) },
                { MessageType.Text, new TextHandler(botClient, _logger) }
            };
        }

        public void Dispatch(Message message)
        {
            _logger.Information($"{message.Type} message received from {Helpers.UserToString(message.From)} in chat ID {message.Chat.Id}.");

            if (!_lookup.TryGetValue(message.Type, out IHandler<Message> handler))
            {
                _defaultHandler.Handle(message);
            }
            else
            {
                handler.Handle(message);
            }
        }
    }
}