using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace test1
{
    public class ValidationImputClass
    {
        public static bool TryValidatoinNameFile(string? nameFile, out string fullNameFile)
        {
            fullNameFile = string.Empty;
            if (nameFile == string.Empty || nameFile == null)
            {
                return false;
            }

            string forbiddenSymbols = new(Path.GetInvalidFileNameChars());
            Regex r = new(string.Format("[{0}]", Regex.Escape(forbiddenSymbols)));
            if (!r.Match(nameFile).Success)
            {
                File.Create($"{nameFile}.csv").Close();
                fullNameFile = $"{nameFile}.csv";
                return true;
            }
            return false;
        }

        public static bool TryValidationForbiddenInputContact(string inputName, string? inputPhone)
        {
            bool correctInput;
            Regex _validationSeparatorChar = new(";", RegexOptions.Compiled);
            correctInput= _validationSeparatorChar.Match(inputName).Success;
            if(inputPhone != null && correctInput)
            {
                correctInput = _validationSeparatorChar.Match(inputPhone).Success;
            }
            return !correctInput;
        }
    }
}
