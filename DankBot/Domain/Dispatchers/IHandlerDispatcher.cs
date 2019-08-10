namespace DankBot.Domain.Dispatchers
{
    public interface IHandlerDispatcher<T>
    {
        void Dispatch(T type);
    }
}