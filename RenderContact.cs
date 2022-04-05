using System;
using System.Collections.Generic;

namespace test1
{
    public class RenderContact
    {
        private readonly int _numberOfLinesOnRender; //сколько строк на одном экране
        private readonly IDataContactInterface _dataContact; //ссылка на БД

        //в контструкторе задаем сколько строк может быть одновременно на экране
        //и присылваем экземпляр класса зазы данных
        public RenderContact(IDataContactInterface data, int numberOfLinesOnRender)
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            _dataContact = data;
        }

        public static void NewSkreen(IDataContactInterface data, int numberOfLinesOnRender)
        {
            Render
        }



        // количество страниц выходяших из расчета количества контактов и длины строк на экране.
        public int NumberPage()
        {
            int amountOfContact = _dataContact.AmountOfContact(); //количество контактов в базе

            int numberPage = amountOfContact / _numberOfLinesOnRender;
            if (numberPage > 0)
            {
                if (amountOfContact % _numberOfLinesOnRender != 0)
                {
                    return numberPage + 1;
                }
                else
                {
                    return numberPage;
                }
            }
            else
            {
                return 1;
            }
        }

        //вывод определенной страницы, если число больше чем всего станиц то последнию выводит,
        //если меньше нуля, то первую.
        public void RenderPage(int numberPage)
        {
            Console.Clear();
            int totalNumberPage = NumberPage();
            numberPage = numberPage <= 0 ? 1 : numberPage;
            numberPage = numberPage >= totalNumberPage ? totalNumberPage : numberPage;

            Console.WriteLine(numberPage + "/" + totalNumberPage); // какая страница открыта
            Console.WriteLine(); //пустая строка
            //номер первого элемента на этой страницы
            int firstElement = (numberPage - 1) * _numberOfLinesOnRender;

            //теперь считаем что класс BaseDataContacts возврашает всегда что то коректное
            if (_dataContact.TryTakeContacts(firstElement, _numberOfLinesOnRender, out List<Contact> contacts))
            {
                RenderingContact(contacts);
            }
            else
            {
                Console.WriteLine("error db");
            }
        }

        private static void RenderingContact(List<Contact> contact)
        {
            foreach (Contact contactItem in contact)
            {
                Console.WriteLine(contactItem);
            }
        }
    }
}