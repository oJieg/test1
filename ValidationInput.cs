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
            nameFile = Path.GetFileName(nameFile);
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
            string charNatCore = ";";
            correctInput = !inputName.Contains(charNatCore);
            if (correctInput && !string.IsNullOrEmpty(inputPhone))
            {
                correctInput = !inputPhone.Contains(charNatCore);
            }
            return correctInput;
        }

        public int TestAdd(int a, int b)
        {
            return a + b;
        }
    }
}