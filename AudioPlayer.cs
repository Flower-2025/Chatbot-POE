using System;
using System.Media;
using System.IO;

namespace CyberSecurityChatbot
{
    public class AudioPlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                //this line of code locates the audio in the folder and combines it with the base directory of the program to create the full path to the audio file
                string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "AudioTrack", "Voiceover.wav");
                //i used an If statment that if the audio is found it should play it, else it will print the error message in read 
                if (File.Exists(path))
                {
                    SoundPlayer player = new SoundPlayer(path);
                    player.PlaySync();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("[Voice greeting file not found - continuing without audio]");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Audio greeting could not be played: {ex.Message}");
            }
        }
    }
}