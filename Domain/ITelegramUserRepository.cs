namespace DankBot.Domain
{
    public interface ITelegramUserRepository
    {
        void Add(TelegramUser telegramUser);
        TelegramUser GetById(int id);
    }
}