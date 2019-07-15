namespace DankBot
{
    using System.Text;
    using System.Threading;

    using global::DankBot.Domain;

    using NLog;

    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class DankBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;
        private readonly IAdminRepository _adminRepository;
        private readonly User _me;

        public DankBot(
            ITelegramBotClient botClient,
            ILogger logger,
            IAdminRepository adminRepository)
        {
            _botClient = botClient;
            _logger = logger;
            _adminRepository = adminRepository;

            _me = _botClient.GetMeAsync().Result;

            _botClient.OnUpdate += OnUpdateHandler;
        }

        private void OnUpdateHandler(object sender, UpdateEventArgs e)
        {
            Message message = e.Update.Message;

            if (message == null)
            {
                return;
            }

            _logger.Info($"{message.Type} message received from {this.UserToString(message.From)} in chat ID {message.Chat.Id}.");

            switch (message.Type)
            {
                case MessageType.ChatMembersAdded:
                    this.ProcessChatMembersAddedMessage(message);
                    break;

                case MessageType.Text:
                    this.ProcessTextMessage(message);
                    break;
            }
        }

        private void ProcessTextMessage(Message message)
        {
            if (message == null)
            {
                return;
            }

            if (message.Text != null)
            {
                _logger.Info($"{this.UserToString(message.From)}: {message.Text}");
            }

            if (message.Entities != null)
            {
                foreach (var entity in message.Entities)
                {
                    _logger.Info($"{entity.Type} entity");

                    if (entity.Type == MessageEntityType.BotCommand)
                    {
                        string cmd = message.Text.Substring(entity.Offset, entity.Length);
                        _logger.Info($"Command: {cmd}");
                    }
                }
            }
        }

        private void ProcessChatMembersAddedMessage(Message message)
        {
            if (message?.NewChatMembers != null)
            {
                foreach (User user in message.NewChatMembers)
                {
                    if (user.Id == _me.Id)
                    {
                        continue;
                    }

                    _logger.Info($"{this.UserToString(user)} joined. Date is {message.Date}.");

                    if (user.IsBot)
                    {
                        _logger.Info($"{this.UserToString(user)} is a bot!!1");
                    }
                }
            }
        }

        public void Run()
        {
            _botClient.StartReceiving();
            Thread.Sleep(Timeout.Infinite);
        }

        private string UserToString(User user)
        {
            var userString = new StringBuilder();

            userString.Append(user.FirstName);

            if (!string.IsNullOrWhiteSpace(user.LastName))
            {
                userString.Append(' ').Append(user.LastName);
            }

            if (!string.IsNullOrWhiteSpace(user.Username))
            {
                userString.Append(" (").Append(user.Username).Append(')');
            }

            userString.Append(" [").Append(user.Id).Append(']');

            return userString.ToString();
        }
    }
}