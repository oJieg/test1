using System;

namespace test1
{
    public class MenuInput
    {
        private readonly RenderContact _renderPageInfo;
        private readonly BaseDataContacts _dataContacts;
        private int _currentPage = 1;

        public MenuInput(RenderContact classRenderContact, BaseDataContacts dataContacts)
        {
            _renderPageInfo = classRenderContact;
            _dataContacts = dataContacts;
        }

        private void MainRenderMenu()
        {
            Console.WriteLine("==========");
            Console.WriteLine("1 - след страница, 2 - предыдушая страница, 3 - добавить контакт 4 -выйти 5 - добавить тестовые контакты");
        }

        private void AddContactRenderMenu()
        {
            Console.Clear();
            Console.WriteLine("==========");
            Console.WriteLine("1 или пустая строка - отмена ");
        }

        // должно возврашать какую страницу в дальнейшем рендерить. Если вернет ноль - выйти
        public int MainInput() 
        {
            MainRenderMenu();

            var input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    return NextPage();

                case "2":
                    return PrevPage();

                case "3":
                    AddContact();
                    return _currentPage;

                case "4":
                    return 0;

                case "5":
                    TestAddContact(7);
                    return _currentPage;

                default:  
                    return _currentPage;
            }
        }

        //добавить контакт
        private void AddContact()
        {
            AddContactRenderMenu();
            Console.WriteLine("ввидите имя:");

            string name = Console.ReadLine();
            if (name == "1" || string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            AddContactRenderMenu();
            Console.WriteLine("ввидите телефон");

            string phone = Console.ReadLine();
            if (phone == "1")
            {
                return;
            }

            _dataContacts.AddContact(name, phone); //правда он тут ругается, что есть вариант передать NULL
        }

        private int NextPage() 
        {
            if (_currentPage < _renderPageInfo.NumberPage())
            {
                _currentPage++;
            }
            return _currentPage;
        }

        private int PrevPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
            }
            return _currentPage;
        }

        //в целях автоматического добавления тестовых котактов
        public void TestAddContact(int length) 
        {
            for (int i = 0; i < length; i++)
            {
                _dataContacts.AddContact("имя" + i, "телефон" + i);
            }
        }
    }

}
