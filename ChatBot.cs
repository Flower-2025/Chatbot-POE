using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CyberSecurityChatbot
{
    public class ChatBot
    {
        private List<string> conversationHistory = new List<string>();
        private Dictionary<string, string> userMemory = new Dictionary<string, string>();
        private ActivityLogger logger = new ActivityLogger();

        public void StartChat(string name)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n>> Ask me about cybersecurity topics (type 'exit' to quit) <<");
            Console.WriteLine(">> Type 'tasks' to manage your cybersecurity tasks <<");
            Console.WriteLine(">> Type 'quiz' to test your cybersecurity knowledge <<");
            Console.WriteLine(">> Type 'log' to see recent activity <<");
            Console.ResetColor();

            while (true)
            {
                Console.Write("\n> ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Please enter a valid question.");
                    Console.ResetColor();
                    continue;
                }

                string lowerInput = input.ToLower();

                // Exit condition
                if (lowerInput == "exit" || lowerInput == "quit" || lowerInput == "goodbye")
                {
                    logger.LogActivity("Exit", $"User {name} exited the chat");
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"\nGoodbye {name}! Stay safe online! Remember to always use strong passwords and avoid suspicious links.");
                    Console.ResetColor();
                    break;
                }

                // NEW: Activity Log Command
                if (lowerInput == "log" || lowerInput == "activity log" || lowerInput == "show log")
                {
                    ShowActivityLog();
                    continue;
                }

                // NEW: Task Management Commands
                if (lowerInput == "tasks" || lowerInput == "show tasks" || lowerInput == "list tasks")
                {
                    TaskManager.ListTasks();
                    continue;
                }

                if (lowerInput.StartsWith("add task") || lowerInput.Contains("add task"))
                {
                    string taskTitle = ExtractTaskTitle(input);
                    if (!string.IsNullOrEmpty(taskTitle))
                    {
                        TaskManager.AddTask(taskTitle);
                        logger.LogActivity("Task Added", $"Task: {taskTitle}");
                    }
                    else
                    {
                        Console.WriteLine("Please specify the task. Example: 'add task Update my passwords'");
                    }
                    continue;
                }

                if (lowerInput.Contains("complete task") || lowerInput.Contains("done"))
                {
                    var match = Regex.Match(input, @"(\d+)");
                    if (match.Success && int.TryParse(match.Groups[1].Value, out int taskId))
                    {
                        TaskManager.CompleteTask(taskId);
                        logger.LogActivity("Task Completed", $"Task ID: {taskId}");
                    }
                    else
                    {
                        Console.WriteLine("Please specify the task number. Example: 'complete task 1'");
                    }
                    continue;
                }

                // NEW: Quiz Command
                if (lowerInput == "quiz" || lowerInput == "start quiz" || lowerInput == "take quiz")
                {
                    QuizManager.StartQuiz();
                    logger.LogActivity("Quiz Started", "User started the cybersecurity quiz");
                    continue;
                }

                // NEW: Memory Commands
                if (lowerInput.Contains("my name is"))
                {
                    var match = Regex.Match(input, @"my name is (.+)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string userName = match.Groups[1].Value.Trim();
                        userMemory["name"] = userName;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✅ I'll remember your name: {userName}");
                        Console.ResetColor();
                        logger.LogActivity("Memory Saved", $"Name: {userName}");
                    }
                    continue;
                }

                if (lowerInput.Contains("i am interested in") || lowerInput.Contains("i like"))
                {
                    var match = Regex.Match(input, @"(?:interested in|like)\s+(.+)", RegexOptions.IgnoreCase);
                    if (match.Success)
                    {
                        string interest = match.Groups[1].Value.Trim();
                        userMemory["interest"] = interest;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"✅ I'll remember you're interested in: {interest}");
                        Console.ResetColor();
                        logger.LogActivity("Memory Saved", $"Interest: {interest}");
                    }
                    continue;
                }

                // NEW: Sentiment Detection
                if (ContainsSentiment(lowerInput))
                {
                    HandleSentiment(lowerInput);
                    continue;
                }

                // NEW: "Tell me more" feature
                if (lowerInput.Contains("tell me more") || lowerInput.Contains("explain more") || lowerInput.Contains("more about"))
                {
                    GetMoreDetails(lowerInput);
                    continue;
                }

                // Enhanced Responses with NLP
                string response = GetEnhancedResponse(lowerInput, name);
                TypingEffect(response);
                logger.LogActivity("Chat Response", $"Response to: {input}");
            }
        }

        private string GetEnhancedResponse(string lowerInput, string name)
        {
            // Check for greetings with name memory
            if (lowerInput.Contains("hello") || lowerInput.Contains("hi") || lowerInput.Contains("hey"))
            {
                string displayName = userMemory.ContainsKey("name") ? userMemory["name"] : name;
                return $"Hello {displayName}! How can I help you with cybersecurity today?";
            }

            // Check for password keywords
            if (lowerInput.Contains("password") || lowerInput.Contains("strong password") || lowerInput.Contains("secure password"))
            {
                string[] responses = {
                    "🔑 A strong password should have at least 12 characters, include uppercase, lowercase, numbers, and special characters like !@#$%^&*.",
                    "🔐 Never reuse passwords across different accounts. Use a password manager to keep track!",
                    "💡 Enable Two-Factor Authentication (2FA) whenever possible for an extra layer of security."
                };
                Random rand = new Random();
                return responses[rand.Next(responses.Length)];
            }

            // Phishing responses
            if (lowerInput.Contains("phishing") || lowerInput.Contains("scam") || lowerInput.Contains("fake email"))
            {
                string[] responses = {
                    "🎣 Phishing is when attackers trick you into giving personal information through fake emails or websites.",
                    "⚠️ Always check the sender's email address carefully - scammers often use similar-looking addresses.",
                    "📧 Never click links or download attachments from unknown senders. When in doubt, delete the email!"
                };
                Random rand = new Random();
                return responses[rand.Next(responses.Length)];
            }

            // Privacy responses
            if (lowerInput.Contains("privacy") || lowerInput.Contains("private") || lowerInput.Contains("data protection"))
            {
                return "🔒 Protect your privacy by:\n- Using strong passwords\n- Enabling 2FA\n- Being careful what you share on social media\n- Using a VPN on public Wi-Fi";
            }

            // 2FA responses
            if (lowerInput.Contains("2fa") || lowerInput.Contains("two factor") || lowerInput.Contains("multi factor"))
            {
                return "🔐 Two-Factor Authentication adds an extra security layer. Use apps like Google Authenticator or Authy instead of SMS when possible.";
            }

            // Help response
            if (lowerInput.Contains("what can i ask") || lowerInput.Contains("help") || lowerInput.Contains("what can you do"))
            {
                return GetHelpText();
            }

            // Thank you response
            if (lowerInput.Contains("thank"))
            {
                return "You're welcome! Stay safe online! 🛡️";
            }

            // Random cybersecurity tip
            if (lowerInput.Contains("tip") || lowerInput.Contains("advice") || lowerInput.Contains("suggestion"))
            {
                return GetRandomTip();
            }

            // Default fallback with personalization
            string displayName = userMemory.ContainsKey("name") ? userMemory["name"] : name;
            if (userMemory.ContainsKey("interest"))
            {
                return $"Since you're interested in {userMemory["interest"]}, here's a tip: {GetRandomTip()}";
            }

            return "I didn't quite understand that. Try asking about:\n- Password safety\n- Phishing scams\n- Privacy protection\n- 2FA\nOr type 'help' for more options.";
        }

        private string ExtractTaskTitle(string input)
        {
            var match = Regex.Match(input, @"add task\s+(?:to\s+)?(.+)", RegexOptions.IgnoreCase);
            if (match.Success)
                return match.Groups[1].Value.Trim();

            // Alternative: "add task - title"
            var match2 = Regex.Match(input, @"add task\s*[-:]\s*(.+)", RegexOptions.IgnoreCase);
            if (match2.Success)
                return match2.Groups[1].Value.Trim();

            return null;
        }

        private bool ContainsSentiment(string input)
        {
            string[] negativeWords = { "worried", "scared", "frustrated", "nervous", "anxious", "overwhelmed", "confused" };
            foreach (string word in negativeWords)
            {
                if (input.Contains(word))
                    return true;
            }
            return false;
        }

        private void HandleSentiment(string input)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            TypingEffect("I understand this can be concerning. Cybersecurity is important for everyone.");
            TypingEffect("Here's a helpful tip to ease your mind:");
            Console.ResetColor();
            TypingEffect(GetRandomTip());
            logger.LogActivity("Sentiment Detected", $"User expressed: {input}");
        }

        private void GetMoreDetails(string input)
        {
            string topic = "cybersecurity";
            if (input.Contains("password"))
                topic = "passwords";
            else if (input.Contains("phishing"))
                topic = "phishing";
            else if (input.Contains("privacy"))
                topic = "privacy";
            else if (input.Contains("2fa"))
                topic = "2FA";

            string details = "";
            switch (topic)
            {
                case "passwords":
                    details = "🔑 Detailed Password Tips:\n- Use at least 12 characters\n- Mix uppercase, lowercase, numbers, and symbols\n- Avoid personal info like birthdays\n- Use a password manager\n- Change passwords every 3-6 months";
                    break;
                case "phishing":
                    details = "🎣 Detailed Phishing Protection:\n- Verify sender's email address\n- Hover over links to see the real URL\n- Don't provide personal info via email\n- Report phishing attempts to your IT department or the company being impersonated";
                    break;
                case "privacy":
                    details = "🔒 Detailed Privacy Protection:\n- Review privacy settings on all accounts\n- Limit what you share publicly\n- Use incognito mode for sensitive searches\n- Consider using privacy-focused search engines like DuckDuckGo";
                    break;
                case "2FA":
                    details = "🔐 Detailed 2FA Information:\n- 2FA requires something you know (password) and something you have (phone/authenticator)\n- Use authenticator apps over SMS for better security\n- Backup codes are essential - store them safely\n- Enable 2FA on all important accounts";
                    break;
                default:
                    details = "📚 Here's a general cybersecurity tip: " + GetRandomTip();
                    break;
            }
            TypingEffect(details);
            logger.LogActivity("More Info", $"User requested details on: {topic}");
        }

        private string GetRandomTip()
        {
            string[] tips = {
                "🔐 Use a password manager to create and store strong, unique passwords.",
                "📧 Think before you click - always verify the sender of emails.",
                "🔒 Enable two-factor authentication on all important accounts.",
                "🔄 Keep your software and operating systems updated regularly.",
                "🛡️ Use a VPN when connecting to public Wi-Fi networks.",
                "📱 Review app permissions and remove unnecessary access.",
                "🗑️ Securely delete files instead of just moving to recycle bin.",
                "📋 Check for HTTPS and padlock icon before entering personal info.",
                "🔑 Create passwords with at least 12 characters using mixed case, numbers, and symbols.",
                "🚨 Report suspicious activity immediately to the relevant platform."
            };
            Random rand = new Random();
            return tips[rand.Next(tips.Length)];
        }

        private string GetHelpText()
        {
            return @"📚 AVAILABLE COMMANDS:
• 'add task [title]' - Add a new cybersecurity task
• 'list tasks' - View all your tasks
• 'complete task [number]' - Mark a task as completed
• 'quiz' - Take the cybersecurity quiz
• 'log' - View recent activity log
• 'my name is [name]' - Save your name
• 'I like [topic]' - Save your interests
• 'tell me more about [topic]' - Get more details
• 'tip' - Get a random cybersecurity tip
• 'help' - Show this help message
• 'bye' - Exit the application

💡 I also detect when you're worried or frustrated and provide encouragement!";
        }

        private void ShowActivityLog()
        {
            logger.ShowRecentLogs();
        }

        private void TypingEffect(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            foreach (char c in message)
            {
                Console.Write(c);
                System.Threading.Thread.Sleep(8);
            }
            Console.WriteLine();
            Console.ResetColor();
        }
    }
}
