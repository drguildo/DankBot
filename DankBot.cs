namespace DankBot
{
    using System.Text;
    using System.Threading;

    using NLog;

    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class DankBot
    {
        private readonly ILogger _logger;
        private readonly ITelegramBotClient _botClient;
        private readonly User _me;

        public DankBot(ITelegramBotClient botClient, ILogger logger)
        {
            _botClient = botClient;
            _logger = logger;

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
            if (message?.Text != null)
            {
                _logger.Info($"{this.UserToString(message.From)}: {message.Text}");
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
                        //await _botClient.KickChatMemberAsync()
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