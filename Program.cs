namespace DankBot
{
    using System;

    using NLog;
    using NLog.Config;
    using NLog.Targets;

    using Telegram.Bot;

    using global::DankBot.Domain;
    using global::DankBot.Repositories;

    internal sealed class Program
    {
        private const string DankBotTokenEnvironmentVariable = "DANKBOT_TOKEN";

        private const string DankBotDbName = "DankBot.db";

        private static void Main()
        {
            var nlogConfig = new LoggingConfiguration();
            var nlogConsole = new ColoredConsoleTarget("logconsole");
            nlogConfig.AddRule(LogLevel.Info, LogLevel.Fatal, nlogConsole);
            LogManager.Configuration = nlogConfig;
            ILogger logger = LogManager.GetLogger("DankBot");
            logger.Info("DankBot is starting.");

            string dankBotToken = Environment.GetEnvironmentVariable(DankBotTokenEnvironmentVariable);
            if (dankBotToken == null)
            {
                logger.Fatal($"Token environment variable {DankBotTokenEnvironmentVariable} isn't defined.");
                Environment.Exit(-1);
            }

            IAdminRepository adminRepository = new AdminRepository(DankBotDbName);

            var botClient = new TelegramBotClient(dankBotToken);
            var dankBot = new DankBot(botClient, logger, adminRepository);
            dankBot.Run();
        }
    }
}