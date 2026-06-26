using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    public class ActivityLogEntry
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }

        public ActivityLogEntry(int id, string action, string description)
        {
            Id = id;
            Action = action;
            Description = description;
            Timestamp = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Id}. {Action}: {Description} ({Timestamp:HH:mm:ss})";
        }
    }

    public class ActivityLogger
    {
        private static List<ActivityLogEntry> logs = new List<ActivityLogEntry>();
        private static int counter = 1;

        public void LogActivity(string action, string description)
        {
            logs.Add(new ActivityLogEntry(counter++, action, description));
        }

        public void ShowRecentLogs()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📊 RECENT ACTIVITY LOG:");
            Console.ResetColor();

            if (logs.Count == 0)
            {
                Console.WriteLine("No activities recorded yet.");
                return;
            }

            int count = Math.Min(7, logs.Count);
            var recent = logs.Skip(logs.Count - count).ToList();

            for (int i = 0; i < recent.Count; i++)
            {
                Console.WriteLine($"  {i + 1}. {recent[i]}");
            }

            if (logs.Count > count)
            {
                Console.WriteLine($"\n... and {logs.Count - count} more activities.");
                Console.WriteLine("Type 'show all' to see complete history.");
            }
            Console.WriteLine();
        }

        public void ShowFullHistory()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📊 COMPLETE ACTIVITY HISTORY:");
            Console.ResetColor();

            if (logs.Count == 0)
            {
                Console.WriteLine("No activities recorded yet.");
                return;
            }

            foreach (var log in logs)
            {
                Console.WriteLine($"  {log}");
            }
            Console.WriteLine();
        }
    }
}