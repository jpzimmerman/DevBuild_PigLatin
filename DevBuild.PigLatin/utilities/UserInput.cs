using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevBuild.Utilities
{
    public enum YesNoAnswer { No = 0, Yes = 1, AnswerNotGiven = 2, } //for convention's sake, let's make sure Yes = 1 and No = 0;
    
    class UserInput
    {

        /// <summary>
        /// This function prompts the user to type "y", "yes", "n", or "no" to provide a yes-or-no answer. Function stays in a loop until 
        /// user enters something we recognize as a yes or no answer.
        /// </summary>
        /// <returns>A yes-or-no answer enum set to Yes, No, or Answer Not Given. </returns>
        public static YesNoAnswer GetYesOrNoAnswer(string questionPrompt)
        {
            YesNoAnswer tmp = YesNoAnswer.AnswerNotGiven;
            string userResponse = "";

            while ( userResponse != "y" && userResponse != "yes" && 
                    userResponse != "n" && userResponse != "no")
            {
                Console.Write(questionPrompt);
                userResponse = Console.ReadLine().Trim().ToLower();
            }
            switch (userResponse)
            {
                case "y":
                case "yes": tmp = YesNoAnswer.Yes; break;
                case "n":
                case "no":  tmp = YesNoAnswer.No; break;
                default: tmp = YesNoAnswer.AnswerNotGiven; break;
            }
            return tmp;
        }
    }
}
