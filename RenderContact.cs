using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    class RenderContact
    {
        private int _numberOfLinesOnRender; //сколько строк на одном экране
        private BaseDataContacts _dataContact; //ссылка на БД
        public RenderContact(BaseDataContacts data, int numberOfLinesOnRender) //в контструкторе задаем сколько строк может быть одновременно на экране и присылваем экземпляр класса зазы данных
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            _dataContact = data;
        }
        public int NumberPage() // сколько страниц выходит из расчета количества контактов и длины строк на экране. Сделал публичной что бы можно было из других класов глянуть сколько там всего страниц и не пытаться открыть лишнее
        {
            //int _numberPage; 
            int _amountOfContact = _dataContact.AmountOfContact(); //количество контактов в базе
            if (_amountOfContact / _numberOfLinesOnRender > 0)
            {
                if (_amountOfContact % _numberOfLinesOnRender != 0)
                {
                    return _amountOfContact / _numberOfLinesOnRender + 1;
                }
                else { return _amountOfContact / _numberOfLinesOnRender; }
            }
            else { return 1; }
        }



        public void RenderPage(int numberPage) //вывод определенной страницы, если число больше чем всего станиц то последнию выводит, если меньше нуля, то первую.
        {
            Console.Clear();
            if (numberPage <= 0) { numberPage = 1; }
            if (numberPage > NumberPage()) { numberPage = NumberPage(); }   //зашита от долбаебов которые хер знает какую страницу открыть.

            Console.WriteLine(numberPage + "/" + NumberPage()); // какая страница открыта
            Console.WriteLine(); //пустая строка
            int _firstElemennt = (numberPage - 1) * _numberOfLinesOnRender; //номер первого элемента на этой страницы
            string[] _contact = new string[2]; //для временного хранения контакта

            if (_dataContact.AmountOfContact() >= _firstElemennt + _numberOfLinesOnRender) //заполнена ли эта траница полностью, т.е проверяем контактов не меньше ли чем вывод нужно вывести на экран
            {
                Contact[] contact;
                try { contact = _dataContact.TakeContact(_firstElemennt, _numberOfLinesOnRender); }  //тут код стал более понятным для чтения, но вот обработка исключений упоролась
                catch(Exception) { contact = new Contact[0]; contact[0] = new Contact("erroe"); }

                for (int i = 0; i < contact.Length; i++)
                {
                    Console.WriteLine("----------");
                    Console.WriteLine(contact[i].Name);
                    Console.WriteLine(contact[i].Phone);
                    Console.WriteLine("----------");
                }
                
            }
            else
            {
                Contact[] contact;
                try { contact = _dataContact.TakeContact(_firstElemennt, _dataContact.AmountOfContact()); }
                catch (Exception) { contact = new Contact[0]; contact[0] = new Contact("erroe"); }

                for (int i = 0; i < contact.Length; i++)
                {
                    Console.WriteLine("----------");
                    Console.WriteLine(contact[i].Name);
                    Console.WriteLine(contact[i].Phone);
                    Console.WriteLine("----------");
                }

            }
        }
    }
}
