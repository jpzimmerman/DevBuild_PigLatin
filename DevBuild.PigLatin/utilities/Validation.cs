using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace DevBuild.Utilities
{
    public enum InformationType { Name = 0, EmailAddress, TelephoneNumber, Date, Alphanumeric, Alphabetical, Numeric, ContainsVowels, ContainsPunctuation, ContainsInvalidPunctuation, CustomField }

    /// <summary>
    /// This struct is used to hold an entry prompt string, an enum for the type of data being validated, 
    /// and strings for display if entry is valid or invalid for that type
    /// </summary>
    public struct ValidDataCheck
    {
        public string DataTypePrompt;
        public InformationType informationType;
        public string validDataMessage;
        public string invalidDataMessage;

        public ValidDataCheck(string dataPrompt, InformationType type, string validMessage, string invalidMessage)
        {
            DataTypePrompt = dataPrompt;
            informationType = type;
            validDataMessage = validMessage;
            invalidDataMessage = invalidMessage;
        }
    }

    class Validation
    {
        private const string nameRegexPattern               = @"^[A-Z][a-z]{4,29}$";
        private const string emailRegexPattern              = @"^(([A-Za-z0-9]){1,30})+[@]+[a-zA-Z]{5,10}([.])+([A-Za-z.]{3,5})$";
        private const string telephoneNumberRegexPattern    = @"^([(]?\d{3})[)-]\d{3}[-]\d{4}$";
        private const string dateRegexPattern               = @"^(\d\d)+[/]+(\d{2})[/](\d){4}$";
        private const string numericRegexPattern            = @"[0-9]";
        private const string alphabeticalRegexPattern       = @"^([A-Za-z])$";
        private const string alphanumericRegexPattern       = @"(^([A-Za-z0-9])$";
        private const string vowelsRegexPattern             = @"[AEIOUaeiou]";
        private const string punctuationRegexPattern        = @"[?!.-,':;]";
        private const string invalidPunctuationRegexPattern = @"[=+<>/\][)({}|]";

        //This Dictionary allows a programmer to specify the type of data they're looking to validate, and will provide a default regex pattern 
        //to check the data against. Default regex patterns are defined above, but I have also allowed the user to add a custom regex pattern 
        //to this Dictionary using the AddUserGeneratedRegex() method. 
        private static readonly Dictionary<InformationType, string> InformationRegexes = 
            new Dictionary<InformationType, string> {   { InformationType.Name, nameRegexPattern },
                                                        { InformationType.EmailAddress, emailRegexPattern },
                                                        { InformationType.TelephoneNumber, telephoneNumberRegexPattern },
                                                        { InformationType.Date, dateRegexPattern },
                                                        { InformationType.Alphabetical, alphabeticalRegexPattern },
                                                        { InformationType.Alphanumeric, alphanumericRegexPattern },
                                                        { InformationType.Numeric, numericRegexPattern },
                                                        { InformationType.ContainsVowels, vowelsRegexPattern },
                                                        { InformationType.ContainsPunctuation, punctuationRegexPattern },
                                                        { InformationType.ContainsInvalidPunctuation, invalidPunctuationRegexPattern }};

        /// <summary>
        /// This function validates information based on the type provided
        /// </summary>
        /// <param name="informationType">This is the information type to validate</param>
        /// <param name="data">This is the string containing the information itself</param>
        /// <returns></returns>
        public static bool ValidateInfo(InformationType informationType, string data)
        {
            //use the information type argument to set up a regex with the pattern from our Dictionary
            Regex r = new Regex(Validation.InformationRegexes[informationType]);
            return r.IsMatch(data);
        }


        /// <summary>
        /// Let's make this a bit like OpenGL and make it so the user can define their own custom regex pattern. 
        /// Adding a custom field is not permanent as of this writing, and the custom pattern will be reset at runtime.
        /// </summary>
        /// <param name="customRegexPattern">User-generated regex pattern to be added to our dictionary</param>
        public static void AddUserGeneratedRegex(string customRegexPattern)
        {
            try
            {
                if (!InformationRegexes.Keys.Contains(InformationType.CustomField))
                {
                    InformationRegexes.Add(InformationType.CustomField, customRegexPattern);
                }
                else throw new ArgumentException();
            }
            catch (ArgumentException e)
            {
                Console.WriteLine("A custom validation field has already been added. Multiple custom exceptions will be something we add down the line, but for now, you'll have to restart the program if you want to overwrite the custom field.");
            }
        }

    }
}
