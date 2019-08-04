namespace DankBot.Domain.Handlers.Message
{
    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class TextHandler : IHandler<Message>
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        public TextHandler(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;
        }

        public void Handle(Message message)
        {
            if (message?.From.Id == _botClient.BotId)
            {
                // Don't process messages sent by us.
                return;
            }

            if (message?.Text != null)
            {
                _logger.Information($"{Helpers.UserToString(message.From)}: {message.Text}");
            }

            if (message?.Entities != null)
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