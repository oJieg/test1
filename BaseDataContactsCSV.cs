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
        private readonly Regex _separatorChar = new("[^;]+", RegexOptions.Compiled);
        
        private bool _flagTryAmout = false;
        private int _amoutOfContact = 0;

        public bool TryInitializationDB(string nameFile)
        {
            if (!File.Exists(nameFile))
            {
                return false;
            }
            _nameFile = nameFile;
            _countLine = AmountOfContact();
            return true;
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
                _flagTryAmout = false;
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
                using StreamReader fileRead = new(_nameFile, Encoding.GetEncoding(1251));
                int i = 0;
                string? readLine = "";
                int takeAndOffset = take + offset;
                while ((readLine = fileRead.ReadLine()) != null)
                {
                    if (i >= offset && i < takeAndOffset)
                    {
                        outContacts.Add(ParsLineInContact(readLine));
                    }

                    if(takeAndOffset < i)
                    {
                        break;
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
            if(_flagTryAmout)
            {
                return _amoutOfContact;
            }

            try
            {
                using StreamReader fileRead = new(_nameFile);
                _countLine = 0;
                while (fileRead.ReadLine() != null)
                {
                    _countLine++;
                }
                _flagTryAmout = true;
                _amoutOfContact = _countLine;
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
             
            string firstWord = TrimEscapeChar(matches[0].Value);
            string secondWord = String.Empty;
            if (matches.Count > 1)
            {
                secondWord = TrimEscapeChar(matches[1].Value);
            }
            if(line[0] == ';')
            {
                return new Contact(" ", firstWord);
            }

            return new Contact(firstWord, secondWord);
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