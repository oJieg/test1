using System;
using System.Collections.Generic;
using NLog;

namespace test1
{
    public class BaseDataContactsTemporary : IDataContactInterface
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly List<Contact> _contacts = new();
        public bool TryInitializationDB(string? nameFile)
        {
            return true;
        }

        public bool TryAddContact(string name, string? phone)
        {
            if (!ValidationInputClass.TryValidationForbiddenInputContact(name, phone))
            {
                logger.Warn($"не корктные данные для ввода {name}, {phone}");
                return false;
            }

            try
            {
                if (string.IsNullOrWhiteSpace(phone))
                {
                    _contacts.Add(new Contact(name));
                }
                else
                {
                    _contacts.Add(new Contact(name, phone));
                }
                return true;
            }
            catch (Exception ex)
            {
                logger.Error($"не удалось создать контакт в памяти {name}, {phone}. Ошибка: {ex}");
                return false;
            }
        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            int amoutOfContact = AmountOfContact();
            if (offset < 0 || offset > amoutOfContact)
            {
                logger.Error($"не верные offset({offset}) и take({take}) в методе TryTakeContacts(в памяти)");
                return false;
            }
            if (amoutOfContact < offset + take)
            {
                take = amoutOfContact - offset;
            }

            foreach (Contact contact in _contacts.GetRange(offset, take))
            {
                outContacts.Add(contact);
            }
            return true;
        }

        public int AmountOfContact()
        {
            return _contacts.Count;
        }
        public bool CreateFile(string directory, string nameFile)
        {
            return true;
        }
        public string FormatFile()
        {
            return string.Empty;
        }
    }
}
