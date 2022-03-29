using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace test1
{

    public class BaseDataContactsCSV : IDataContactInterface
    {
        private string _nameFile = String.Empty;
        private int _countLine = 0;
        private Regex _separatorChar = new("[^;]+", RegexOptions.Compiled);
        private readonly Regex _validationSeparatorChar = new(";", RegexOptions.Compiled);
        public bool TryInitializationDB(string nameFile)
        {
            if (File.Exists($"{nameFile}.csv"))
            {
                _nameFile = $"{nameFile}.csv";
                _countLine = AmountOfContact();
                return true;
            }

            string forbiddenSymbols = new(Path.GetInvalidFileNameChars());
            Regex r = new(string.Format("[{0}]", Regex.Escape(forbiddenSymbols)));
            if (!r.Match(nameFile).Success)
            {
                File.Create($"{nameFile}.csv").Close();
                _nameFile = $"{nameFile}.csv";
                return true;
            }
            return false;
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
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            try
            {
                using StreamReader sw = new(_nameFile, Encoding.GetEncoding(1251));
                int i = 0;
                string? readLine = "";
                int takeAddOffset = take + offset;
                while ((readLine = sw.ReadLine()) != null)
                {
                    if (i >= offset && i < takeAddOffset)
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

        private Contact ParsLineInContact(string line)
        {
            MatchCollection matches = _separatorChar.Matches(line);

            string oneWord = TrimEscapeChar(matches[0].Value);
            string twoWord = String.Empty;
            if (matches.Count > 1)
            {
                twoWord = TrimEscapeChar(matches[1].Value);
            }

            Contact outContsct = new(oneWord, twoWord);
            Console.WriteLine(outContsct);
            return outContsct;
        }

        private static string TrimEscapeChar(string wold)
        {
            char charEscape = '\"';
            if (wold[0] == charEscape && wold[^1] == charEscape)
            {
                return wold[1..^2];
            }
            return wold;
        }

        private bool ValidationInput(string? input)
        {
            if (input == null)
            {
                return true;
            }
            return  _validationSeparatorChar.Match(input).Success;
        }
    }
}