using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DevBuild.Utilities;

namespace DevBuild.PigLatin
{
    class Program
    {
        public static char[] vowels = { 'a', 'e', 'i', 'o', 'u', 'A', 'E', 'I', 'O', 'U' };
        public static char[] punctuationMarks = { '?', '!', '.', ',' };

        static void Main(string[] args)
        {
            string userInputString = "";
            YesNoAnswer userAnswer = YesNoAnswer.AnswerNotGiven;

            Console.Write(  "***********************************************************\n" +
                            "*                  Dev.Build(2.0) - Pig Latin             *\n" +
                            "***********************************************************\n\n");
            
            //Greet the user
            Console.WriteLine("Welcome to the Pig Latin Translator!");

            while (true)
            {
                //Prompt the user for a word
                while (String.IsNullOrEmpty(userInputString))
                {
                    Console.Write("Enter a line to be translated: ");
                    userInputString = Console.ReadLine().Trim();
                }

                #region Check user's string for email address, phone number, etc.
                //Run data through all the regexes we have, just to see if user entered an email address, phone number, etc.
                if (Validation.ValidateInfo(InformationType.Numeric, userInputString) || 
                    Validation.ValidateInfo(InformationType.EmailAddress, userInputString) ||
                    Validation.ValidateInfo(InformationType.ContainsInvalidPunctuation, userInputString))
                {
                    //if we find that the user input contains numbers, is an email address, or is otherwise invalid 
                    //let's repeat their input, clear the user input field, and try again
                    Console.WriteLine(userInputString);
                    userInputString = "";
                    continue;
                }
                #endregion
                //If we get to this point, let's translate the user's input into Pig Latin
                //let's try the simplest translation first
                Console.WriteLine(PigLatinTranslator(userInputString));

                #region Learning words, checking user's input string against words we know
                //Let's also add each word to a dictionary we keep building on as we learn it, 
                //so the program is smart enough to know when the user is entering a string of nonsense
                //Let's store this with StreamWriter and StreamReader at first, then serialize/deserialize data later
                #endregion

                //ask the user if they want to translate another word
                userInputString = "";
                userAnswer = UserInput.GetYesOrNoAnswer("Do you wish to translate again? ");
                switch (userAnswer)
                {
                    case YesNoAnswer.Yes: continue;
                    case YesNoAnswer.No: return;
                    default: continue;
                }
            }
        }

        static string PigLatinTranslator(string startingString)
        {
            string translatedSentence = "";
            string finalString = "", tmpString = "";
            List<int> vowelPositions = new List<int>();
            List<char> firstConsonants = new List<char>();
            int vowelTestResult;
            
            string[] words = startingString.Trim().Split(' ');
            foreach (string inputString in words)
            {
                //if the substring we got from Split() is empty, let's make sure we're skipping it to avoid errors
                if (String.IsNullOrEmpty(inputString)) { continue; }
                
                tmpString = inputString;

                #region Translation if first letter of word is vowel
                if (vowels.Contains(tmpString[0]))
                {
                    tmpString = String.Concat(tmpString, "way");
                    if (ContainsPunctuation(tmpString))
                    {
                        tmpString = MovePunctuation(tmpString);
                    }
                    translatedSentence = String.Concat(translatedSentence, tmpString, " ");
                }
                #endregion
                else
                {
                    //go through all of the vowels, find our first occurrence
                    //then take all the consonants before it and strip them into a separate string.
                    foreach (char testVowel in vowels)
                    {
                        if ((vowelTestResult = tmpString.IndexOf(testVowel)) >= 0)        //we found a vowel, now let's record its position
                        {
                            vowelPositions.Add(vowelTestResult);
                        }
                    }
                    if (vowelPositions.Count > 0)   //now that we have a list of positions for our vowels, let's find the position of our *first* vowel
                    {
                        if (char.IsUpper(tmpString[0]))     //is first letter of string capitalized?
                        {
                            StringBuilder tmp = new StringBuilder(tmpString);
                            tmp[vowelPositions.Min()] = char.ToUpper(tmp[vowelPositions.Min()]);
                            tmp[0] = char.ToLower(tmp[0]);
                            tmpString = tmp.ToString();
                            //tmpString = char.ToUpper(tmpString[vowelPositions.Min()]) + tmpString.Substring(vowelPositions.Min() +1) + char.ToLower(tmpString[0]) + tmpString.Substring(1, vowelPositions.Min() - 1);
                        }
                        finalString = tmpString.Substring(vowelPositions.Min()) + tmpString.Substring(0, vowelPositions.Min()) + "ay";
                    }
                    else
                    {
                        if (char.IsUpper(tmpString[0]))     //is first letter of string capitalized?
                        {
                            finalString = char.ToUpper(tmpString[1]) + tmpString.Substring(2) + char.ToLower(tmpString[0]) + "ay";
                        }
                        else
                        {
                            finalString = tmpString.Substring(1) + tmpString[0] + "ay";
                        }
                    }
                    if (ContainsPunctuation(finalString))
                    {
                        finalString = MovePunctuation(finalString);
                    }
                    translatedSentence = String.Concat(translatedSentence, finalString, " ");
                }
                //Since we're using this method to loop through an entire sentence,
                //let's reset the lists we were using to track vowel positions and first consonants
                vowelPositions.Clear();         
                firstConsonants.Clear();
            }
            return translatedSentence;
        }

        static string MovePunctuation(string wordWithPunctuation)
        {
            string correctedString = "";
            List<char> foundPunctuation = new List<char>();

            //find out which punctuation marks are in the string in question
            foreach (char c in punctuationMarks)
            {
                if (wordWithPunctuation.Contains(c))
                {
                    foundPunctuation.Add(c);
                }
                else continue;
            }

            //split the string by its punctuation mark (likely only one per word)
            //then reassemble the word without punctuation
            foreach (char c in foundPunctuation)
            {
                string[] tmpString = wordWithPunctuation.Split(c);             
                wordWithPunctuation = String.Join("", tmpString);
                wordWithPunctuation = String.Concat(wordWithPunctuation, c);
                correctedString = wordWithPunctuation;
            }
            return correctedString;
        }

        static bool ContainsPunctuation(string inputString)
        {
            foreach (char c in punctuationMarks)
            {
                if (inputString.Contains(c))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
