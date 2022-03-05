using System;
using System.Collections.Generic;


namespace test1
{
    public class Contact
    {
        public string Name { get; set; }
        public string Phone { get; set; }
        public Contact(string name, string phone)
        {
            Name = name;
            Phone = phone;
        }
        public Contact(string name)
        {
            Name = name;
            Phone = "none";
        }
    }
    class BaseDataContacts
    {

        private List<Contact> _contacts = new List<Contact>();

        //методы добавления контактов
        public void AddContact(string name, string phone)
        {
            _contacts.Add(new Contact(name,phone));
        }
        public void AddContact(string name) //как минимум у контакта должено быть имя
        {
            _contacts.Add(new Contact(name));
        }
   
        public Contact[] TakeContact(int offset, int take) //офсет - начальный элемент, тейк - количество элементов
        {
            //валидация
            if(offset < 0 && offset > AmountOfContact())
            {
                throw new Exception("eeror offset");
            }
            if(offset+take > AmountOfContact())
            {
                throw new Exception("eeror take");
            }

            Contact[] outContacts = new Contact[take];
            for(int i = 0; i < take; i++)
            {
                outContacts[i]=_contacts[i+offset];
            }
            return outContacts;
        }
        public int AmountOfContact() //количество контактов в базе
        {

            return _contacts.Count;
        }
        public void TestAddContact(int length) //в целях автоматического добавления тестовых котактов
        {
            for (int i = 0; i < length; i++)
            {
                AddContact("имя" + i, "телефон" + i);
            }
        }
    }
}
