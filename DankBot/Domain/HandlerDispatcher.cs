namespace DankBot.Domain
{
    using System.Collections.Generic;

    using global::DankBot.Domain.Handlers;

    using NLog;

    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class HandlerDispatcher
    {
        private readonly Dictionary<MessageType, MessageHandler> _lookup;
        private readonly ILogger _logger;

        public HandlerDispatcher(ITelegramBotClient botClient, ILogger logger)
        {
            _lookup = new Dictionary<MessageType, MessageHandler>
            {
                { MessageType.ChatMembersAdded, new ChatMembersAddedHandler(botClient, logger) },
                { MessageType.Text, new TextHandler(botClient, logger) }
            };
            _logger = logger;
        }

        public void Handle(Message message)
        {
            _logger.Info($"{message.Type} message received from {MessageHandler.UserToString(message.From)} in chat ID {message.Chat.Id}.");

            if (!_lookup.TryGetValue(message.Type, out MessageHandler handler))
            {
                _logger.Warn($"No handler found for message type {message.Type}");
            }
            else
            {
                handler.Handle(message);
            }
        }
    }
}