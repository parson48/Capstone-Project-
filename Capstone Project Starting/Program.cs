using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace Capstone_Project_Starting
{
    class Program
    {
        static void Main(string[] args)
        {
            string dataPath = @"WordList\WordList.txt";

            MainMenu(dataPath);
        }
        
   
        private static void MainMenu(string dataPath)
        {
            //
            // variable declaration
            //
            bool exitApplication = false;
            char userConvertedResponse;

            do
            {
                DisplayScreenHeader("Application Menu");

                MenuLine("1", "Play Hangman");
                MenuLine("2", "Add Words");
                MenuLine("3", "Delete Words");
                MenuLine("4", "Quit Application");

                ConsoleKeyInfo userInput = Console.ReadKey();
                userConvertedResponse = char.Parse(userInput.KeyChar.ToString().ToLower());
                switch (userConvertedResponse)
                {
                    case ('1'):
                        {
                            Hangman(dataPath);
                            break;
                        }
                    case ('2'):
                        {

                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Invalid choice. Please select a number 1-X");
                            break;
                        }
                }
            } while (!exitApplication);
        }

        /// <summary>
        /// Makes a line of the menu, coloring in the first part cyan and the second part white. 
        /// </summary>
        /// <param name="precludingSymbol">Put in a single symbol, typically what you want the user to type in. Ie A, 1, etc...</param>
        /// <param name="line">A description of what typing the prevbious symbol will do.</param>
        private static void MenuLine(string precludingSymbol, string line)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"[{precludingSymbol}] ");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine(line);
        }

        /// <summary>
        /// Plays a game of hangman
        /// </summary>
        private static void Hangman(string dataPath)
        {
            //
            //
            // variable declaration
            //        
            //"changingWord" is the word that is shown, that slowly gets revealed.
            //"hiddenCharacters" is the hidden word converted into an array of characters, to be compared to changingWord
            //"hiddenWord" is the list of all the hangman words -  basic list in case WordList ever gets deleted
            //
            List<string> hiddenWords = new List<string>()
                {
                "statuesque", "contribute", "gold", "quack", "notice", "reset", "whip", "mundane", "gallimaufry"
                };
            char[] hiddenCharacters;
            char userConvertedResponse;
            bool completedWord = false;
            bool letterInWord = false;
            int completedCharacter;
            int hiddenCharactersPlacement;
            List<char> userPickedCharacters = new List<char>();
            List<char> alphabet = new List<char>();

            AlphabetCreation(alphabet);

            //
            // Initializes the hidden word, by checking all the user inputted words then randomly picking one from the list
            //
            ReadWordsFromFile(dataPath, hiddenWords);
            int randomNumber = RandomNumber(0, hiddenWords.Count);
            char[] changingWord = new char[hiddenWords[randomNumber].Length];

            // Writes down the hidden word, used only for testing
            //WriteLine("{0}", hiddenWords[randomNumber]);

            //Makes the "hiddenCharacters" array into the hidden word
            hiddenCharacters = hiddenWords[randomNumber].ToCharArray(0, hiddenWords[randomNumber].Length);

            DisplayScreenHeader("Hangman");

            //Writes out asterisks for each letter in the hidden word.
            Console.WriteLine();
            Console.Write("Word: ");
            for (int i = 0; i < hiddenWords[randomNumber].Length; i++)
            {
                changingWord[i] = '*';
                Console.Write($"{changingWord[i]}");
            }
            Console.WriteLine();
            Console.WriteLine();

            //The loop that does it all. If all the asterisks are removed, exits the loop
            do
            {
                
                // Reads the user's input of a letter. Does not check to see if the input is a number, as inputting a number would equal any of the hidden letter.
                letterInWord = false;
                Console.Write("Guess a letter >> ");
                ConsoleKeyInfo userInput = Console.ReadKey();
                userConvertedResponse = char.Parse(userInput.KeyChar.ToString().ToLower());

                DisplayScreenHeader("Hangman");

                //Checks every letter to see if it is equal to the user input.
                //If it is equal, 
                hiddenCharactersPlacement = 0;
                foreach (char c in hiddenCharacters)
                {
                    if (c == userConvertedResponse)
                    {
                        letterInWord = true;
                        changingWord[hiddenCharactersPlacement] = c;
                    }
                    hiddenCharactersPlacement = hiddenCharactersPlacement + 1;
                }

                userPickedCharacters.Add(userConvertedResponse);

                //Says if the letter is in the word or not.
                if (letterInWord == true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Yes! {0} is in the word.", userConvertedResponse);
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Sorry. {0} is not in the word.", userConvertedResponse);
                }

                //Displays the current progress on the hidden word    
                Console.Write("Word: ");
                for (int i = 0; i < hiddenWords[randomNumber].Length; i++)
                {
                    Console.Write($"{changingWord[i]}");
                }
                Console.WriteLine();
                Console.WriteLine();
                int tempTopCursorPosition = Console.CursorTop;
                int tempLeftCursorPosition = Console.CursorLeft;

                PickedLetterDisplay(alphabet, userPickedCharacters, tempLeftCursorPosition, tempTopCursorPosition);

                // If all the asterisks are revealed, and thus all the letters have been guessed, it exits the loop.
                completedCharacter = 0;
                foreach (char c in changingWord)
                {
                    if (c == '*')
                    {
                        completedCharacter = completedCharacter + 1;
                    }
                    else
                    {
                        completedCharacter = completedCharacter + 0;
                    }
                }
                if (completedCharacter == 0)
                {
                    completedWord = true;
                }
                else { }
            } while (!completedWord);

            //Congratulates the user
            Console.WriteLine("Good job on guessing the hidden word!");
            Console.WriteLine($"The hidden word was {hiddenWords[randomNumber]}.");
            Console.ReadKey();
        }

        /// <summary>
        /// Sends back a random number between the given lower bound and the upper bound.
        /// </summary>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        private static int RandomNumber(int lowerBound, int upperBound)
        {
            Random ranNumberGenerator = new Random();
            int randomNumber;
            randomNumber = ranNumberGenerator.Next(lowerBound, upperBound);

            return randomNumber;
        }

        /// <summary>
        /// Displays letters in a typicaly qwerty keyboard format.
        /// </summary>
        /// <param name="pickedCharacters"></param>
        private static void PickedLetterDisplay(List<char> alphabet, List<char> pickedCharacters, int cursorLeftPosition, int cursorTopPosition)
        {
            //
            // Variable declaration
            //
            bool characterPicked;

            //
            //Sets the cursor position 5 lower than the rest of the hangman game.
            //
            Console.SetCursorPosition(cursorLeftPosition, cursorTopPosition + 5);

            //
            // Writes out all the letters.
            // If the letter is already picked by the user, it instead displays a special character
            // The if/if elses outside of the pickedCharacters foreach is to make the characters look in a keyboard format.
            //
            foreach (char letter in alphabet)
            {
                characterPicked = false;
                if (letter == 'a')
                {
                    Console.Write(" ");
                }
                else if (letter == 'z')
                {
                    Console.Write("  ");
                }

                foreach (char character in pickedCharacters)
                {
                    if (letter == character)
                    {
                        characterPicked = true;
                    }
                }

                if (characterPicked == true)
                {
                    Console.Write("*  ");
                }
                else
                {
                    Console.Write($"{letter}  ");
                }

                if (letter == 'p')
                {
                    Console.WriteLine();
                }
                else if (letter == 'l')
                {
                    Console.WriteLine();
                }
            }

            Console.SetCursorPosition(cursorLeftPosition, cursorTopPosition);

        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        private static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// Simply adds all letters in the alphabet, in qwerty order.
        /// </summary>
        /// <param name="alphabet"></param>
        private static void AlphabetCreation(List<char> alphabet)
        {
            alphabet.Add('q');
            alphabet.Add('w');
            alphabet.Add('e');
            alphabet.Add('r');
            alphabet.Add('t');
            alphabet.Add('y');
            alphabet.Add('u');
            alphabet.Add('i');
            alphabet.Add('o');
            alphabet.Add('p');
            alphabet.Add('a');
            alphabet.Add('s');
            alphabet.Add('d');
            alphabet.Add('f');
            alphabet.Add('g');
            alphabet.Add('h');
            alphabet.Add('j');
            alphabet.Add('k');
            alphabet.Add('l');
            alphabet.Add('z');
            alphabet.Add('x');
            alphabet.Add('c');
            alphabet.Add('v');
            alphabet.Add('b');
            alphabet.Add('n');
            alphabet.Add('m');
        }


        private static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("***************************************************************");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Reads and Replaces the current hiddenWord list with the list from the file
        /// </summary>
        /// <param name="dataPath"></param>
        /// <param name="hiddenWord"></param>
        private static void ReadWordsFromFile(string dataPath, List<string> hiddenWord)
        {
            string[] listOfWords = File.ReadAllLines(dataPath);

            for (int i = 0; i < listOfWords.Length; i++)
                listOfWords[i] = listOfWords[i].ToLower();

            foreach (string word in listOfWords)
            {
                if (!hiddenWord.Contains(word))
                {
                    hiddenWord.Add(word);
                }
            }

        }
    }


    
}
