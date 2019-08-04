namespace DankBot
{
    using System.Threading;

    using global::DankBot.Domain.Dispatchers;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Args;

    public class DankBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        private readonly UpdateHandlerDispatcher _dispatcher;

        public DankBot(
            ITelegramBotClient botClient,
            ILogger logger)
        {
            _botClient = botClient;
            _botClient.OnUpdate += OnUpdateHandler;

            _logger = logger;

            _dispatcher = new UpdateHandlerDispatcher(_botClient, _logger);
        }

        private void OnUpdateHandler(object sender, UpdateEventArgs e)
        {
            _dispatcher.Dispatch(e.Update);
        }

        public void Run()
        {
            _botClient.StartReceiving();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}