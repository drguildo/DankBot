namespace DankBot
{
    using System;
    using System.Text;
    using System.Threading;
    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;

    public class DankBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly User _me;

        public DankBot(string token)
        {
            _botClient = new TelegramBotClient(token);
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

            Console.WriteLine($"{message.Type} message received from {this.UserToString(message.From)} in chat ID {message.Chat.Id}.");

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
                Console.WriteLine($"{this.UserToString(message.From)}: {message.Text}");
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

                    if (user.IsBot)
                    {
                        Console.WriteLine($"Bot {this.UserToString(user)} joined. Date is {message.Date}.");
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
                userString.Append($" {user.LastName}");
            }
            if (!string.IsNullOrWhiteSpace(user.Username))
            {
                userString.Append($" (@{user.Username})");
            }
            return userString.ToString();
        }
    }
}
