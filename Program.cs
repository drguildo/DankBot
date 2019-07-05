namespace DankBot
{
    using System;
    using NLog;

    sealed class Program
    {
        private const string DankBotTokenEnvironmentVariable = "DANKBOT_TOKEN";

        static void Main(string[] args)
        {
            var nlogConfig = new NLog.Config.LoggingConfiguration();
            var nlogConsole = new NLog.Targets.ColoredConsoleTarget("logconsole");
            nlogConfig.AddRule(LogLevel.Info, LogLevel.Fatal, nlogConsole);
            NLog.LogManager.Configuration = nlogConfig;

            ILogger logger = NLog.LogManager.GetLogger("DankBot");

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
