﻿namespace DankBot.Domain.Handlers
{
    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class TextHandler : MessageHandler
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        public TextHandler(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public override void Handle(Message message)
        {
            if (message == null)
            {
                return;
            }

            if (message.Text != null)
            {
                _logger.Information($"{MessageHandler.UserToString(message.From)}: {message.Text}");
            }

            if (message.Entities != null)
            {
                foreach (var entity in message.Entities)
                {
                    _logger.Information($"{entity.Type} entity");

                    if (entity.Type == MessageEntityType.BotCommand)
                    {
                        string cmd = message.Text.Substring(entity.Offset, entity.Length);
                        _logger.Information($"Command: {cmd}");
                    }
                }
            }
        }
    }
}