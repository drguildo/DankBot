namespace DankBot.Helpers
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Newtonsoft.Json;

    public class Result
    {
        public int Offenses { get; set; }
        public List<string> Messages { get; set; }
        public int TimeAdded { get; set; }
    }

    public class Spammer
    {
        public bool Ok { get; set; }
        public Result Result { get; set; }
    }

    public class SpammerDetector
    {
        private const string API_ENDPOINT = "https://combot.org/api/cas/check?user_id=";

        public async Task<bool> IsSpammerAsync(int id)
        {
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(API_ENDPOINT + id).ConfigureAwait(false);
                Spammer spammer = JsonConvert.DeserializeObject<Spammer>(json);
                return spammer.Ok;
            }
        }
    }
}