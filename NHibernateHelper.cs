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

    public static class NHibernateHelper
    {
        public static ISession Session { get; }

        static NHibernateHelper()
        {
            var mapper = new ModelMapper();
            mapper.AddMapping<TelegramUserMappings>();
            HbmMapping mappings = mapper.CompileMappingForAllExplicitlyAddedEntities();

            var cfg = new Configuration();
            cfg.DataBaseIntegration(db =>
            {
                db.Dialect<SQLiteDialect>();
                db.Driver<SQLite20Driver>();
                db.ConnectionString = "data source=:memory:";
                db.ConnectionReleaseMode = ConnectionReleaseMode.OnClose;
                db.LogSqlInConsole = true;
                db.LogFormattedSql = true;
            }).AddMapping(mappings);

            var sessionFactory = cfg.BuildSessionFactory();
            var session = sessionFactory.OpenSession();

            using (var transaction = session.BeginTransaction())
            {
                new SchemaExport(cfg).Execute(
                useStdOut: true,
                execute: true,
                justDrop: false,
                connection: session.Connection,
                exportOutput: System.Console.Out);

                transaction.Commit();
            }

            session.Clear();

            NHibernateHelper.Session = session;
        }
    }
}
