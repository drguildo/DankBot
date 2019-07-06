namespace DankBot.Domain
{
    internal class TelegramUser
    {
        public virtual int Id { get; set; }
        public virtual bool IsBot { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }
        public virtual string Username { get; set; }
    }
}