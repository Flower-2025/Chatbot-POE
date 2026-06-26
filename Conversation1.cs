using System;

namespace CyberSecurityChatbot
{
    public class UserInteraction
    {
        public static string AskName()
        {
            Console.Write("Please enter your name: ");
            string name = Console.ReadLine();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.Write("Name cannot be empty. Please enter your name: ");
                name = Console.ReadLine();
            }

            return name;
        }

        public static void WelcomeUser(string name)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine($"║    Welcome {name}!                                      ║");
            Console.WriteLine("║    I am your Cybersecurity Awareness Assistant         ║");
            Console.WriteLine("║    I can help you learn about:                          ║");
            Console.WriteLine("║    • Password safety                                    ║");
            Console.WriteLine("║    • Phishing scams                                     ║");
            Console.WriteLine("║    • Safe browsing habits                               ║");
            Console.WriteLine("║                                                          ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.ResetColor();
        }
    }
}