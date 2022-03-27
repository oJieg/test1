using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;


namespace test1
{
    public class BaseDataContactsCSV : IDataContactInterface
    {
        private string _nameFile = "";
        private int _countLine = 0;
        public bool TryInitializationBD(string nameFile)
        {
            if (File.Exists($"{nameFile}.csv"))
            {
                _nameFile = $"{nameFile}.csv";
                _countLine = AmountOfContact();
                return true;
            }
            else
            {
                string forbiddenSymbols = new(Path.GetInvalidFileNameChars());
                Regex r = new(string.Format("[{0}]", Regex.Escape(forbiddenSymbols)));
                if (r.Match(nameFile).Success)
                {
                    return false;
                }
                else
                {
                    File.Create($"{nameFile}.csv").Close();
                    _nameFile = $"{nameFile}.csv";
                    return true;
                }
            }
        }

        public int AmountOfContact()
        {
            try
            {
                using StreamReader sw = new(_nameFile);
                _countLine = 0;
                while (sw.ReadLine() != null)
                {
                    _countLine++;
                }
                return _countLine;
            }
            catch (Exception)
            {
                return 0;
            }

        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            try
            {
                using StreamReader sw = new(_nameFile, Encoding.GetEncoding(1251));
                int i = 0;
                string? readLine = "";
                while ((readLine = sw.ReadLine()) != null)
                {
                    if (i >= offset && i < take + offset)
                    {
                        outContacts.Add(ParsLineInContact(readLine));
                    }
                    i++;
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }

        }

        public bool TryAddContact(string name, string? phone)
        {
            if (ValidationInput(name) || ValidationInput(phone))
            {
                return false;
            }

            try
            {
                File.AppendAllText(_nameFile, $"\"{name}\";\"{phone}\"\n");
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static Contact ParsLineInContact(string line)
        {
            Regex r = new("[^;]+");
            MatchCollection matches = r.Matches(line);

            string oneWord = DeliteEscape(matches[0].Value);
            string twoWord = "";
            if (matches.Count > 1)
            {
                twoWord = DeliteEscape(matches[1].Value);
            }

            Contact outContsct = new(oneWord, twoWord);
            Console.WriteLine(outContsct);
            return outContsct;
        }

        private static string DeliteEscape(string wold)
        {
            char charEscape = '\"';
            if (wold[0] == charEscape && wold[^1] == charEscape)
            {
                return wold[1..^2];
            }
            return wold;
        }

        private static bool ValidationInput(string? input)
        {
            if (input == null)
            {
                return true;
            }
            Regex regex = new(";");
            return regex.Match(input).Success;
        }
    }
}
