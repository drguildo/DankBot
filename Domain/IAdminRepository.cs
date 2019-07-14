namespace DankBot.Domain
{
    public interface IAdminRepository
    {
        void Add(Admin admin);
        Admin GetById(int id);
    }
}