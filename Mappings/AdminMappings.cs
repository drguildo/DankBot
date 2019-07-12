namespace DankBot.Mappings
{
    using global::DankBot.Domain;

    using NHibernate.Mapping.ByCode;
    using NHibernate.Mapping.ByCode.Conformist;

    internal class AdminMappings : ClassMapping<Admin>
    {
        public AdminMappings()
        {
            Id(e => e.TelegramId, mapper => mapper.Generator(Generators.Assigned));
        }
    }
}