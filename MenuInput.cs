﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class MenuInput
    {
        private RenderContact _renderPageInfo;
        private BaseDataContacts _dataContacts;
        private int _currentPage = 1;
        public MenuInput(RenderContact classRenderContact, BaseDataContacts dataContacts)
        {
            _renderPageInfo = classRenderContact;
            _dataContacts = dataContacts;
        }

        private void MainRenderMenu()
        {
            Console.WriteLine("==========");
            Console.WriteLine("1 - след страница, 2 - предыдушая страница, 3 - добавить контакт 4 -выйти");

        }
        private void AddContactRenderMenu()
        {
            Console.Clear();
            Console.WriteLine("==========");
            Console.WriteLine("1 или пустая строка - отмена ");
        }
        public int MainInput() // должно возврашать какую страницу в дальнейшем рендерить. Если вернет ноль - выйти
        {
            MainRenderMenu();
            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    NextPage();
                    break;

                case "2":
                    PrevPage();
                    break;

                case "3":
                    AddContact();
                    break;
                case "4":
                    return 0;
                    break;
            }
            return _currentPage;

        }

        private void AddContact() //добавить контакт
        {

            AddContactRenderMenu();
            Console.WriteLine("ввидите имя:");
            var name = Console.ReadLine();
            if (name == "1" || name == "")
            {
                return;
            }
            AddContactRenderMenu();
            Console.WriteLine("ввидите имя:");
            var phone = Console.ReadLine();
            if (phone == "1")
            {
                return;
            }
            if (phone == "") // поле телефона может быть и пустым же...
            {
                _dataContacts.AddContact(name);
            }
            else
            {
                _dataContacts.AddContact(name, phone);
            }
        }
        private void NextPage() //это наверное излишни под такое методы делать же? или норм? 
        {
            if (_currentPage < _renderPageInfo.NumberPage())
            {
                _currentPage++;
            }
        }

        private void PrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
            }
        }

    }

}
