using System;

namespace CyberSecurityChatbot
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();

            // Step 1: Play voice greeting
            AudioPlayer.PlayGreeting();

            // Step 2: Show ASCII logo
            Logo.ShowLogo();

            // Step 3: Get user name
            string userName = UserInteraction.AskName();

            // Step 4: Welcome user with formatted message
            UserInteraction.WelcomeUser(userName);

            // Step 5: Start chatbot conversation
            ChatBot bot = new ChatBot();
            bot.StartChat(userName);
        }
    }
}
