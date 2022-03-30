using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace test1
{

    public class BaseDataContactsCSV : IDataContactInterface
    {
        private string _nameFile = string.Empty;
        private int _countLine = 0;
        private Regex _separatorChar = new("[^;]+", RegexOptions.Compiled);

        public bool TryInitializationDB(string? nameFile)
        {
            if (File.Exists($"{nameFile}.csv"))
            {
                _nameFile = $"{nameFile}.csv";
                _countLine = AmountOfContact();
                return true;
            }

            return ValidationImputClass.TryValidatoinNameFile(nameFile, out _nameFile);
        }

        public bool TryAddContact(string name, string? phone)
        {

            if(!ValidationImputClass.TryValidationForbiddenInputContact(name, phone))
            {
                return false;
            }

            try
            {
                File.AppendAllText(_nameFile, $"\"{AddEscapeChar(name)}\";\"{AddEscapeChar(phone)}\"\n");
                return true;
            }
            catch
            {
                return false;
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
            return outContsct;
        }

        private static string TrimEscapeChar(string word)
        {
            word = Regex.Replace(word,"\"\"", "\"");
            char charEscape = '\"';
            return word.Trim(charEscape);
        }

        private static string? AddEscapeChar(string? word)
        {
            if (word == null)
            {
                return null;
            }
           return Regex.Replace(word, "\"", "\"\"");
        }
    }
}