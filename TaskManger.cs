using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReminderDate { get; set; }

        public TaskItem(int id, string title)
        {
            Id = id;
            Title = title;
            IsCompleted = false;
            CreatedAt = DateTime.Now;
        }
    }

    public static class TaskManager
    {
        private static List<TaskItem> tasks = new List<TaskItem>();
        private static int counter = 1;

        public static void AddTask(string title)
        {
            var task = new TaskItem(counter++, title);
            tasks.Add(task);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"✅ Task added: '{title}' (ID: {task.Id})");
            Console.ResetColor();
            Console.WriteLine("Would you like to set a reminder? (yes/no)");

            string response = Console.ReadLine()?.ToLower().Trim();
            if (response == "yes" || response == "y")
            {
                SetReminder(task);
            }
        }

        private static void SetReminder(TaskItem task)
        {
            Console.WriteLine("When would you like to be reminded? (e.g., 'tomorrow', 'in 3 days', or specific date)");
            string input = Console.ReadLine();

            DateTime? reminderDate = ParseReminderInput(input);
            if (reminderDate.HasValue)
            {
                task.ReminderDate = reminderDate;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Reminder set for '{task.Title}' on {reminderDate:yyyy-MM-dd HH:mm}");
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Could not parse date. No reminder set.");
            }
        }

        private static DateTime? ParseReminderInput(string input)
        {
            input = input.ToLower().Trim();

            if (input.Contains("tomorrow"))
                return DateTime.Now.AddDays(1);
            else if (input.Contains("in "))
            {
                var parts = input.Split(' ');
                for (int i = 0; i < parts.Length; i++)
                {
                    if (parts[i] == "in" && i + 1 < parts.Length && int.TryParse(parts[i + 1], out int days))
                    {
                        return DateTime.Now.AddDays(days);
                    }
                }
            }
            else if (DateTime.TryParse(input, out DateTime parsedDate))
                return parsedDate;

            return null;
        }

        public static void ListTasks()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n📋 YOUR TASKS:");
            Console.ResetColor();

            if (tasks.Count == 0)
            {
                Console.WriteLine("No tasks yet. Add one with 'add task'!");
                return;
            }

            foreach (var task in tasks)
            {
                string status = task.IsCompleted ? "✅" : "⬜";
                string reminder = task.ReminderDate.HasValue ? $" (Reminder: {task.ReminderDate:yyyy-MM-dd})" : "";
                Console.WriteLine($"{status} #{task.Id}: {task.Title}{reminder}");
            }
            Console.WriteLine();
        }

        public static void CompleteTask(int taskId)
        {
            var task = tasks.FirstOrDefault(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"✅ Task '{task.Title}' completed!");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Task #{taskId} not found.");
                Console.ResetColor();
            }
        }
    }
}