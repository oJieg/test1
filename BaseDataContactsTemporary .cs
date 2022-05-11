using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test1
{
    public class BaseDataContactsTemporary : IDataContactInterface
    {
        private readonly ILogger _logger;
        public string FormatFile { get { return string.Empty; } }

        public BaseDataContactsTemporary(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaseDataContactsTemporary>();
        }

        private readonly List<Contact> _contacts = new();
        public bool TryInitializationDB(string? nameFile)
        {
            return true;
        }

        public bool TryAddContact(string name, string? phone)
        {
            if (!ValidationInputClass.TryValidationForbiddenInputContact(name, phone))
            {
                _logger.LogWarning("Не верные данные пользователя name - {name} phone - {phone}", name, phone);
                return false;
            }

            try
            {
                _contacts.Add(new Contact(name, phone));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Не удалось создать контакт в памяти {name}, {phone}.", name, phone);
                return false;
            }
        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            int amoutOfContact = AmountOfContact();
            if (offset < 0 || offset > amoutOfContact)
            {
                _logger.LogError("не верные offset({offset}) и take({take}) в методе TryTakeContacts(в памяти)", offset, take);
                return false;
            }

            try
            {
                if (amoutOfContact < offset + take)
                {
                    take = amoutOfContact - offset;
                }
                outContacts.AddRange(_contacts.GetRange(offset, take));

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ошибка чтения файла  для элементов с " +
                        "{offset}, {take}-количество элементов.", offset, take);
                return false;
            }
        }

        public int AmountOfContact()
        {
            return _contacts.Count;
        }
        public bool CreateFile(string directory, string nameFile)
        {
            return true;
        }
    }
}