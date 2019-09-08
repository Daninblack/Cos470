using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace DollarWords
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = @"C:\Users\Daniel\Desktop\words.txt";
            List<string> lines = File.ReadAllLines(filePath).ToList();

            //Watch used to time how long it takes to find all the dollar words
            var watch = System.Diagnostics.Stopwatch.StartNew();
            DollarWordsToFile(lines);
            watch.Stop();
            var time = watch.ElapsedMilliseconds;

            DisplayInformation(time);
        }

        public static int LetterValue(char c)
        {
            //Converts char to lower case
            char letter = char.ToLower(c);

            //Gets the character's ASCII value
            int number = (int)letter;

            //Only returns values for characters A-Z
            if(number >= 97 && number <= 122)
            {
                return number - 96;
            }
            return 0;
        }

        public static int WordValue(string word)
        {
            int value = 0;
            foreach(char c in word)
            {
                //Adds the value of each character
                value += LetterValue(c);
            }
            return value;
        }

        public static Boolean IsItDollarWord(int value)
        {
            if (value == 100)
            {
                return true;
            }
            else
                return false;
        }

        public static void DollarWordsToFile(List<string> lines)
        {
            //Creates a file to write the "Dollar Words" into
            TextWriter file = new StreamWriter(@"C:\Users\Daniel\Desktop\DollarWords.txt");

            //Values needed for information
            double totalWords = 0;
            double totalDollarWords = 0;
            string shortestDollarWord = "";
            string longestDollarWord = "";
            string mostExpensiveWord = "";
            int min = int.MaxValue;
            int max = int.MinValue;
            int maxWordValue = 0;

            foreach (string line in lines)
            {
                int value = WordValue(line);
                totalWords++;

                //Finds the most expensive word
                if(value > maxWordValue)
                {
                    maxWordValue = value;
                    mostExpensiveWord = line;
                }

                //Writes only dollar words to the text file
                if (IsItDollarWord(value))
                {
                    file.WriteLine(line);
                    totalDollarWords++;

                    //Calculates the shortest and longest dollar words
                    if (min > line.Length)
                    {
                        min = line.Length;
                        shortestDollarWord = line;
                    }
                    if (line.Length > max)
                    {
                        max = line.Length;
                        longestDollarWord = line;
                    }
                }
            }

            file.Close();
            file.Dispose();

            DisplayInformation(totalDollarWords, totalWords, shortestDollarWord, longestDollarWord, mostExpensiveWord);
        }

        public static void DisplayInformation(double totalDollarWords, double totalWords, string shortest, string longest, string mostExpensiveWord)
        {
            double percentage;

            Console.WriteLine("Some information:\n");
            //Prints the percentage of dollar words found in the text file
            if (totalDollarWords == 0)
            {
                Console.WriteLine("No dollar words were found.");
            }
            else
            {
                percentage = Math.Round((totalDollarWords / totalWords) * 100, 2);

                Console.WriteLine(percentage + "% of the words are dollar words.");
            }

            //Prints the shortest and longest dollar words found in the text file
            Console.WriteLine("Shortest dollar word is " + shortest);
            Console.WriteLine("Longest dollar word is " + longest);

            //Prints the most expensive word found in the text file
            Console.WriteLine("The most expensive word is " + mostExpensiveWord);
        }

        public static void DisplayInformation(long time)
        {
            Console.WriteLine("It took " + time + " milliseconds to find all the dollar words.");
        }

    }
}
