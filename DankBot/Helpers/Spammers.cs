namespace DankBot.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;
    using Serilog;

    public class Result
    {
        public int offenses { get; set; }
        public List<string> messages { get; set; }
        public int time_added { get; set; }
    }

    public class Spammer
    {
        public bool ok { get; set; }
        public Result result { get; set; }
    }

    public class Spammers
    {
        private const string API_ENDPOINT = "https://combot.org/api/cas/check?user_id=";

        private readonly ILogger _logger;

        public Spammers(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<bool> IsSpammerAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(API_ENDPOINT + id);
                Spammer spammer = JsonConvert.DeserializeObject<Spammer>(json);
                return spammer.ok;
            }
        }
    }
}