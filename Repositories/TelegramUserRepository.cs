namespace DankBot.Repositories
{
    using NHibernate;

    using global::DankBot.Domain;

    class TelegramUserRepository : ITelegramUserRepository
    {
        private readonly ISession _session;

        public TelegramUserRepository(ISession session)
        {
            _session = session;
        }

        public void Add(TelegramUser telegramUser)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(telegramUser);
                transaction.Commit();
            }
        }

        public TelegramUser GetById(int id)
        {
            return _session.Get<TelegramUser>(id);
        }
    }
}