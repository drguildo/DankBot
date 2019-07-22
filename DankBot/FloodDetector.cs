namespace DankBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class FloodDetector
    {
        private struct UserMessageTime
        {
            public int UserId { get; set; }
            public DateTime When { get; set; }
        }

        private readonly uint _timeRangeInSeconds;
        private readonly uint _messageThreshold;
        private List<UserMessageTime> _userMessageTimes;

        public FloodDetector(uint timeRangeInSeconds, uint messageThreshold)
        {
            _timeRangeInSeconds = timeRangeInSeconds;
            _messageThreshold = messageThreshold;

            _userMessageTimes = new List<UserMessageTime>();
        }

        public void Add(int userId, DateTime when)
        {
            this.Prune();

            _userMessageTimes.Add(new UserMessageTime
            {
                UserId = userId,
                When = when
            });
        }

        public bool IsFlooding(int userId)
        {
            this.Prune();

            int count = _userMessageTimes.Count(t => t.UserId == userId);

            if (count >= _messageThreshold)
            {
                return true;
            }

            return false;
        }

        private void Prune()
        {
            DateTime whenToPruneBefore = DateTime.Now.AddSeconds(-1 * _timeRangeInSeconds);

            _userMessageTimes.RemoveAll(t => t.When < whenToPruneBefore);
        }
    }
}