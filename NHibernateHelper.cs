namespace DankBot
{
    using global::DankBot.Mappings;

    using NHibernate;
    using NHibernate.Cfg;
    using NHibernate.Cfg.MappingSchema;
    using NHibernate.Dialect;
    using NHibernate.Driver;
    using NHibernate.Mapping.ByCode;
    using NHibernate.Tool.hbm2ddl;

    public class NHibernateHelper
    {
        public ISession Session { get; }

        public NHibernateHelper()
        {
            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.ConnectionString = "data source=:memory:";
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
            }).AddMapping(this.GetMappings());

            var sessionFactory = cfg.BuildSessionFactory();
            this.Session = sessionFactory.OpenSession();

            using (var transaction = this.Session.BeginTransaction())
            {
                new SchemaExport(cfg).Execute(
                useStdOut: true,
                execute: true,
                justDrop: false,
                connection: this.Session.Connection,
                exportOutput: System.Console.Out);
                transaction.Commit();
            }
            this.Session.Clear();
        }

        private HbmMapping GetMappings()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<TelegramUserMappings>();
            return mapper.CompileMappingForAllExplicitlyAddedEntities();
        }
    }
}