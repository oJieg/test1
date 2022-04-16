using System;
using System.Collections.Generic;

namespace test1
{
    internal class ScreenMainBD : Screen
    {
        private readonly IDataContactInterface _dataContacts;
        public ScreenMainBD(int numberOfLinesOnRender, IDataContactInterface dataContacts)
            : base(numberOfLinesOnRender)
        {
            _dataContacts = dataContacts;
            _pageCounterRender = true;
        }

        protected override void Update()
        {
            base.Update();
            _fullAmountOfLine = _dataContacts.AmountOfContact();
        }
        protected override void Title()
        {
            Console.WriteLine("контакты:");
        }

        protected override List<string> DataForPageRender()
        {
            List<string> data = new List<string>();
            _dataContacts.TryTakeContacts(_offsetForTotalNumber, _takeForTotalNumber, out List<Contact> outContact);
            foreach (Contact contact in outContact)
            {
                data.Add(contact.ToString());
            }
            return data;
        }

        protected override void ChoiseMenuRender()
        {
            base.ChoiseMenuRender();
            Console.WriteLine("N - добавить новый контакт, Т- тестовые контакты");
        }

        protected override void ChoiseInpyt(int InputInt, string InputString)
        {
            base.ChoiseInpyt(InputInt, InputString);
            if (InputString == "t" || InputString == "T")
            {
                for (int i = 0; i < 5; i++)
                {
                    _dataContacts.TryAddContact($"name{i}", $"phone{i}");
                }
            }
            if (InputString == "n" || InputString == "N")
            {
                AddContact();
            }
        }

        private void AddContact()
        {
            Console.Clear();
            Console.WriteLine("1 или пустая строка - отмена ");
            Console.WriteLine("ввидите имя:");

            string? name = Console.ReadLine();
            if (name == "1" || string.IsNullOrWhiteSpace(name))
            {
                return;
            }
            Console.Clear();
            Console.WriteLine("ввидите телефон");
            string? phone = Console.ReadLine();
            _dataContacts.TryAddContact(name, phone);
        }

    }
}
