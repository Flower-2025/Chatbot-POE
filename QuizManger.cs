using System;
using System.Collections.Generic;

namespace CyberSecurityChatbot
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public string[] Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string question, string[] options, string correctAnswer, string explanation)
        {
            Question = question;
            Options = options;
            CorrectAnswer = correctAnswer;
            Explanation = explanation;
        }
    }

    public static class QuizManager
    {
        private static List<QuizQuestion> questions = new List<QuizQuestion>();
        private static int currentIndex = 0;
        private static int score = 0;
        private static bool isActive = false;

        static QuizManager()
        {
            InitializeQuestions();
        }

        private static void InitializeQuestions()
        {
            questions.AddRange(new[]
            {
                new QuizQuestion(
                    "What should you do if you receive an email asking for your password?",
                    new[] { "A) Reply with your password", "B) Delete the email", "C) Report it as phishing", "D) Forward it to friends" },
                    "C",
                    "✓ Correct! Reporting phishing emails helps protect yourself and others from scams."
                ),
                new QuizQuestion(
                    "Which of these is a strong password?",
                    new[] { "A) password123", "B) MyBirthday1990", "C) P@ssw0rd!2024#", "D) 12345678" },
                    "C",
                    "✓ Correct! A strong password includes uppercase, lowercase, numbers, and special characters."
                ),
                new QuizQuestion(
                    "What is Two-Factor Authentication (2FA)?",
                    new[] { "A) Using two passwords", "B) A second layer of security", "C) Logging in twice", "D) Changing password every 2 days" },
                    "B",
                    "✓ Correct! 2FA adds an extra layer of security beyond just a password."
                ),
                new QuizQuestion(
                    "What is a common sign of a phishing email?",
                    new[] { "A) Personal greeting", "B) Urgent language requesting action", "C) Correct spelling", "D) Known sender address" },
                    "B",
                    "✓ Correct! Phishing emails often create urgency to trick you into acting quickly."
                ),
                new QuizQuestion(
                    "Why should you avoid using public Wi-Fi for banking?",
                    new[] { "A) It's slow", "B) It costs money", "C) It's unsecure and can be intercepted", "D) It's against the law" },
                    "C",
                    "✓ Correct! Public Wi-Fi can be compromised, exposing your sensitive information."
                ),
                new QuizQuestion(
                    "What is social engineering in cybersecurity?",
                    new[] { "A) Building social networks", "B) Manipulating people to reveal information", "C) Engineering social media", "D) Creating social apps" },
                    "B",
                    "✓ Correct! Social engineering exploits human psychology rather than technical vulnerabilities."
                ),
                new QuizQuestion(
                    "How often should you update your software?",
                    new[] { "A) Never", "B) Once a year", "C) Regularly when updates are available", "D) Only when it breaks" },
                    "C",
                    "✓ Correct! Regular updates patch security vulnerabilities and protect your system."
                ),
                new QuizQuestion(
                    "What does HTTPS indicate?",
                    new[] { "A) The site is fast", "B) The connection is secure", "C) The site is popular", "D) The site is free" },
                    "B",
                    "✓ Correct! HTTPS encrypts data between your browser and the website."
                ),
                new QuizQuestion(
                    "Why should you use different passwords for different accounts?",
                    new[] { "A) It's easier to remember", "B) Prevents multiple accounts being compromised", "C) It's required by law", "D) It makes passwords stronger" },
                    "B",
                    "✓ Correct! If one account is breached, others remain secure with unique passwords."
                ),
                new QuizQuestion(
                    "What should you do if you suspect your device has malware?",
                    new[] { "A) Ignore it", "B) Run a full antivirus scan", "C) Continue using it", "D) Delete all files" },
                    "B",
                    "✓ Correct! Running a full antivirus scan can detect and remove malware."
                )
            });
        }

        public static void StartQuiz()
        {
            currentIndex = 0;
            score = 0;
            isActive = true;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n🎯 CYBERSECURITY QUIZ");
            Console.WriteLine("Answer with A, B, C, or D");
            Console.ResetColor();

            ShowQuestion();
        }

        private static void ShowQuestion()
        {
            if (currentIndex >= questions.Count)
            {
                EndQuiz();
                return;
            }

            var q = questions[currentIndex];
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\nQuestion {currentIndex + 1}/{questions.Count}:");
            Console.ResetColor();
            Console.WriteLine(q.Question);
            foreach (var option in q.Options)
            {
                Console.WriteLine($"  {option}");
            }
            Console.Write("Your answer: ");
        }

        public static void HandleAnswer(string answer)
        {
            if (!isActive) return;

            answer = answer.ToUpper().Trim();
            var q = questions[currentIndex];

            if (answer == q.CorrectAnswer)
            {
                score++;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(q.Explanation);
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"❌ Incorrect. The answer was: {q.CorrectAnswer}");
                Console.WriteLine(q.Explanation.Replace("✓", "ℹ️"));
                Console.ResetColor();
            }

            currentIndex++;
            ShowQuestion();
        }

        private static void EndQuiz()
        {
            isActive = false;
            int total = questions.Count;
            double percentage = (double)score / total * 100;

            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("\n📊 QUIZ COMPLETE!");
            Console.WriteLine($"Score: {score}/{total} ({percentage:F1}%)");
            Console.ResetColor();

            string feedback;
            if (percentage >= 80)
                feedback = "🌟 Outstanding! You're a cybersecurity expert!";
            else if (percentage >= 60)
                feedback = "👍 Good job! Keep learning to become a cybersecurity pro!";
            else if (percentage >= 40)
                feedback = "📚 Nice try! Review the basics of cybersecurity to improve.";
            else
                feedback = "💪 Don't give up! Cybersecurity is important - keep learning!";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(feedback);
            Console.ResetColor();
            Console.WriteLine();
        }

        // Call this from ChatBot when user is in quiz mode
        public static bool IsActive() => isActive;
    }
}