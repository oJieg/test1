using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;


namespace test1
{

    public class BaseDataContactsCSV : IDataContactInterface
    {
        private readonly ILogger _logger;

        public string FormatFile { get { return ".csv"; } }

        private string _nameFile = string.Empty;
        private int _countLine = 0;

        private bool _flagTryAmout = false;
        private int _amoutOfContact = 0;

        private bool _needCreateFile = false;
        public BaseDataContactsCSV(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaseDataContactsCSV>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        public bool TryInitializationDB(string nameFile)
        {
            _nameFile = nameFile;
            try
            {
                if (!File.Exists(nameFile))
                {
                    _needCreateFile = true;
                    return true;
                }
                _countLine = AmountOfContact();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка инициализации BaseDataContactsCSV");
                return false;
            }
            return true;
        }

        public bool TryAddContact(string name, string? phone)
        {
            if (!ValidationInputClass.TryValidationForbiddenInputContact(name, phone))
            {
                _logger.LogWarning("Не верные данные пользователя name - {name} phone - {phone}", name, phone);
                return false;
            }
            if (_needCreateFile && !TryCreateFile(_nameFile))
            {
                return false;
            }
            _needCreateFile = false;

            try
            {
                File.AppendAllText(_nameFile, $"\"{AddEscapeChar(name)}\";\"{AddEscapeChar(phone)}\"\n",
                    Encoding.GetEncoding(1251));
                _flagTryAmout = false;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка записи в файл -{_nameFile}, для данных имя-{name}, телефон {phone}",
                    _nameFile, name, phone);
                return false;
            }
        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            if (_needCreateFile)
            {
                return true;
            }
            if (!HelperBaseData.ValidationOffset(offset, AmountOfContact()))
            {
                _logger.LogError("не верные offset({offset})", offset);
                return false;
            }

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

                    if (takeAndOffset < i)
                    {
                        break;
                    }

                    i++;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ошибка чтения файла {_nameFile} для элементов с " +
                    "{offset}, {take}-количество элементов.", _nameFile, offset, take);
                return false;
            }
        }

        public int AmountOfContact()
        {
            if (_needCreateFile)
            {
                return 0;
            }
            if (_flagTryAmout)
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "ошибка чтения файла {_nameFile} при подсчете количества контактов.", _nameFile);
                return 0;
            }
        }

        public bool TryCreateFile(string nameFile)
        {
            if (ValidationInputClass.TryValidatinNameFile(nameFile)
                 && !File.Exists(nameFile))
            {
                try
                {
                    File.Create(nameFile).Close();
                    TryInitializationDB(nameFile);
                    return true;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "не удалось создать файл {nameFile} csv.", nameFile);
                }
            }

            _logger.LogWarning("Была неудачная попытка создать файл {nameFile}. " +
                "Имя файла состоит из запрещённых символов или такой файл уже есть.", nameFile);
            return false;
        }

        private static Contact ParsLineInContact(string line)
        {
            string[] matches = line.Split(";");

            string firstWord = TrimEscapeChar(matches[0]);
            string secondWord = String.Empty;
            if (matches.Length > 1)
            {
                secondWord = TrimEscapeChar(matches[1]);
            }
            return new Contact(firstWord, secondWord);
        }

        private static string TrimEscapeChar(string word)
        {
            word = Regex.Replace(word, "\"\"", "\"");
            char charEscape = '\"';
            return word.Trim(charEscape);
        }

        private static string? AddEscapeChar(string? word)
        {
            return Regex.Replace(word, "\"", "\"\"");
        }
    }
}