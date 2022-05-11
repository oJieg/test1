using System.IO;
using System.Text.RegularExpressions;

namespace test1
{
    public class ValidationInputClass
    {
        public static bool TryValidatinNameFile(string? nameFile)
        {
            if (string.IsNullOrWhiteSpace(nameFile))
            {
                return false;
            }

            string forbiddenSymbols = new(Path.GetInvalidFileNameChars());
            Regex r = new(string.Format("[{0}]", Regex.Escape(forbiddenSymbols)));
            if (!r.Match(nameFile).Success)
            {
                return true;
            }

            return false;
        }

        public static bool TryValidationForbiddenInputContact(string inputName, string? inputPhone)
        {
            bool correctInput;
            Regex _validationSeparatorChar = new(";", RegexOptions.Compiled);
            correctInput = !_validationSeparatorChar.Match(inputName).Success;
            if (inputPhone != null && correctInput)
            {
                correctInput = !_validationSeparatorChar.Match(inputPhone).Success;
            }
            return correctInput;
        }

        public int TestAdd(int a, int b)
        {
            return a + b;
        }
    }
}