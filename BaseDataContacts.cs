using System;
using System.Collections.Generic;

namespace test1
{
    public class BaseDataContacts
    {
        private readonly List<Contact> _contacts = new List<Contact>();

        //методы добавления контактов
        public void AddContact(string name, string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                _contacts.Add(new Contact(name));
            }
            else
            {
                _contacts.Add(new Contact(name, phone));
            }     
        }

        public Contact[] TakeContact(int offset, int take) //офсет - начальный элемент, тейк - количество элементов
        {
            //валидация
            if (offset < 0 && offset > AmountOfContact())
            {
                throw new Exception("error offset");
            }
            if (offset + take > AmountOfContact())
            {
                throw new Exception("error take");
            }

            Contact[] outContacts = new Contact[take];
            for (int i = 0; i < take; i++)
            {
                outContacts[i] = _contacts[i + offset];
            }
            return outContacts;
        }

        //количество контактов в базе
        public int AmountOfContact() 
        {
            return _contacts.Count;
        }
    }
}
