namespace DankBot
{
    using System;
    using global::DankBot.Domain;
    using NLog;
    using NLog.Config;
    using NLog.Targets;

    internal sealed class Program
    {
        private const string DankBotTokenEnvironmentVariable = "DANKBOT_TOKEN";

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

            var dankBot = new DankBot(logger, dankBotToken);
            dankBot.Run();
        }
    }
}