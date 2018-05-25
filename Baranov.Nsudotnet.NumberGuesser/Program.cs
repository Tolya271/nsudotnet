using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Baranov.Nsudotnet.NumberGuesser
{
    class Program
    {
        private static Random _random = new Random();

        static string[] Abuses = {"Why, Mr. {0}? Why, why? Why do you do it? Why, why get up? Why keep guessing?",
                          "If stupidity were a crime, you'd get a life sentence, {0}.",
                          "You are the reason God doesn't talk to us anymore, {0}.",
                          "Roses are red, violets are blue, I have 5 fingers, the 3rd ones for you, {0}."};
        static void Main(string[] args)
        {
            Console.WriteLine("Hello! What is your name?");
            string username = Console.ReadLine();
            int[] history = new int[1000];
            int guessingNumber = _random.Next(0, 100);
            Console.WriteLine("Guess number if you can, {0}", username);
            DateTime start = DateTime.Now;
            for (int attempt = 0; ; attempt++)
            {
                string userInput = Console.ReadLine();
                if (userInput.Equals("q"))
                {
                    Console.WriteLine("I'm sorry.Goodbye(((");
                    return;
                }
                int userNumber;
                bool correct = int.TryParse(userInput, out userNumber);
                if (!correct || userNumber < 0 || userNumber > 100)
                {
                    Console.WriteLine("Nice try, {0}, but you must enter number(!) between 0 and 100", username);
                    attempt--;
                    continue;
                }
                if (userNumber == guessingNumber)
                {
                    Console.WriteLine("Congratulations!!!It took you {0} attempts and {1} minute(s) to guess the number", attempt, DateTime.Now.Subtract(start).Minutes);
                    for (int i = 0; i < attempt; i++) 
                    {
                        Console.WriteLine("{0} : it was too {1}", history[i], history[i] > guessingNumber ? "big" : "small");
                    }
                    Console.ReadLine();
                    return;
                }
                history[attempt] = userNumber;
                Console.WriteLine("Guessed number {0} than {1}", guessingNumber > userNumber ? "more" : "less", userNumber);
                if (attempt % 4 == 3){
                    Console.WriteLine(Abuses[_random.Next(Abuses.Length)], username);
                }
            }
        }
    }
}
