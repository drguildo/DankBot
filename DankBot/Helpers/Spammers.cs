namespace DankBot.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    using Serilog;

    public class Spammers
    {
        private const string SPAMMERS_FILENAME = "export.csv";

        private readonly ILogger _logger;
        private readonly HashSet<int> _ids;

        private DateTime _fileCreationDate;

        public Spammers(ILogger logger)
        {
            _logger = logger;
            _ids = new HashSet<int>();

            _fileCreationDate = File.GetCreationTime(SPAMMERS_FILENAME);

            this.Load();
        }

        public bool IsSpammer(int id)
        {
            // If a newer version of the spammer IDs file exists, load it.
            DateTime currentFileCreationDate = File.GetCreationTime(SPAMMERS_FILENAME);
            if (currentFileCreationDate > _fileCreationDate)
            {
                this.Load();
                _fileCreationDate = currentFileCreationDate;
            }

            return _ids.Contains(id);
        }

        private void Load()
        {
            _ids.Clear();

            foreach (var line in File.ReadLines(SPAMMERS_FILENAME))
            {
                int spammerId = System.Convert.ToInt32(line);
                _ids.Add(spammerId);
            }

            _logger.Information($"Loaded {_ids.Count} spammer IDs.");
        }
    }
}