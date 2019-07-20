namespace DankBot
{
    using System.Threading;

    using global::DankBot.Domain;

    using Serilog;

    using Telegram.Bot;
    using Telegram.Bot.Args;
    using Telegram.Bot.Types;

    public class DankBot
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger _logger;

        private readonly HandlerDispatcher _dispatcher;

        public DankBot(
            ITelegramBotClient botClient,
            ILogger logger)
        {
            _botClient = botClient;
            _botClient.OnUpdate += OnUpdateHandler;

            _logger = logger;

            _dispatcher = new HandlerDispatcher(_botClient, _logger);
        }

        private void OnUpdateHandler(object sender, UpdateEventArgs e)
        {
            Message message = e.Update.Message;
            _dispatcher.Handle(message);
        }

        public void Run()
        {
            _botClient.StartReceiving();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}