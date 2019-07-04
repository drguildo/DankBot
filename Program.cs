namespace DankBot
{
    using System;

    sealed class Program
    {
        private const string DankBotTokenEnvironmentVariable = "DANKBOT_TOKEN";

        static void Main(string[] args)
        {
            string dankBotToken = Environment.GetEnvironmentVariable(DankBotTokenEnvironmentVariable);

            if (dankBotToken == null)
            {
                Console.Error.WriteLine($"Token environment variable {DankBotTokenEnvironmentVariable} isn't defined.");
                Environment.Exit(-1);
            }

            var dankBot = new DankBot(dankBotToken);
            dankBot.Run();
        }
    }
}
