namespace DankBot.Mappings
{
    using global::DankBot.Domain;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    internal class TelegramUserMappings : ClassMapping<TelegramUser>
    {
        public TelegramUserMappings()
        {
            Id(e => e.Id, mapper => mapper.Generator(Generators.Assigned));
            Property(e => e.IsBot);
            Property(e => e.FirstName);
            Property(e => e.LastName);
            Property(e => e.Username);
        }
    }
}