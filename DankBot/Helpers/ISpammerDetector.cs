using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace DankBot.Helpers
{
    public interface ISpammerDetector
    {
        bool IsSpam(Message message);
        Task<bool> IsSpammerAsync(int id);
    }
}