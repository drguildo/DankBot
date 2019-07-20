namespace DankBot
{
    using System;

    using global::DankBot.Domain;
    using global::DankBot.Repositories;

    using Serilog;

    using Telegram.Bot;

    internal sealed class Program
    {
        private const string DankBotTokenEnvironmentVariable = "DANKBOT_TOKEN";

        private const string DankBotDbName = "DankBot.db";

        private static void Main()
        {
            ILogger logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            logger.Information("DankBot is starting.");

            string dankBotToken = Environment.GetEnvironmentVariable(DankBotTokenEnvironmentVariable);
            if (dankBotToken == null)
            {
                logger.Fatal($"Token environment variable {DankBotTokenEnvironmentVariable} isn't defined.");
                Environment.Exit(-1);
            }

            IAdminRepository adminRepository = new AdminRepository(DankBotDbName);

            var botClient = new TelegramBotClient(dankBotToken);
            var dankBot = new DankBot(botClient, logger);
            dankBot.Run();
        }
    }
}