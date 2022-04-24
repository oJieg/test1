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
            PageCounter = true;
        }

        protected override void Update()
        {
            base.Update();
            FullAmountOfLines = _dataContacts.AmountOfContact();
        }
        protected override void Title()
        {
            Console.WriteLine("контакты:");
        }

        protected override List<string> DataForPageRender()
        {
            List<string> data = new List<string>(TakeForTotalNumber);
            if (!_dataContacts.TryTakeContacts(OffsetForTotalNumber, TakeForTotalNumber, out List<Contact> outContact))
            {
                MessageForNotValidInput("ошибка чтения базы данных");
            }

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

        protected override void ChoiceInput(int InputInt, string InputString)
        {
            base.ChoiceInput(InputInt, InputString);
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

            if (!_dataContacts.TryAddContact(name, phone))
            {
                logger.Error("Не удалось добавить контакт, " +
    $"в имени или телефоне есть запрешенные символы name - {name}, phone - {phone}");
                MessageForNotValidInput("Не удалось добавить контакт." +
                    "В имени или телефоне присудствуют запрешенные символ - ;");
            }
        }
    }
}