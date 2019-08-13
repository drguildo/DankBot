namespace DankBot.Domain.Handlers
{
    public interface IHandler<T>
    {
        void Handle(T type);
    }
}