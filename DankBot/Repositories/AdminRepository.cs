namespace DankBot.Repositories
{
    using global::DankBot.Domain;

    using LiteDB;

    internal class AdminRepository : IAdminRepository
    {
        private readonly LiteCollection<Admin> _collection;

        public AdminRepository(string connectionString)
        {
            var db = new LiteDatabase(connectionString);
            _collection = db.GetCollection<Admin>("admins");
        }

        public void Add(Admin admin)
        {
            _collection.Insert(admin);
        }

        public Admin GetById(int id)
        {
            return _collection.FindById(id);
        }
    }
}