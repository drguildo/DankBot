namespace DankBot.Domain.MessageHandlers
{
    using Telegram.Bot.Types;

    public abstract class MessageHandler
    {
        public abstract void Handle(Message message);

        internal static string UserToString(User user)
        {
            var userString = new System.Text.StringBuilder();

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