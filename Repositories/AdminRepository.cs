namespace DankBot.Repositories
{
    using NHibernate;

    using global::DankBot.Domain;

    class AdminRepository : IAdminRepository
    {
        private readonly ISession _session;

        public AdminRepository(ISession session)
        {
            _session = session;
        }

        public void Add(Admin admin)
        {
            using (var transaction = _session.BeginTransaction())
            {
                _session.Save(admin);
                transaction.Commit();
            }
        }

        public Admin GetById(int id)
        {
            return _session.Get<Admin>(id);
        }
    }
}