using System;
using System.Collections.Generic;

namespace test1
{
    public class BaseDataContactsTemporary : IDataContactInterface
    {
        private readonly List<Contact> _contacts = new();
        public bool TryInitializationDB(string? nameFile)
        {
            return true;
        }

        public bool TryAddContact(string name, string? phone)
        {
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
            catch (Exception)
            {
                return false;
            }
        }

        public bool TryTakeContacts(int offset, int take, out List<Contact> outContacts)
        {
            outContacts = new();
            int amoutOfContact = AmountOfContact();
            if (offset < 0 || offset > amoutOfContact)
            {
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
    }
}
