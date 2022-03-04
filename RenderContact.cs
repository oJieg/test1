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


        // public void RenderPage()// если ничего не отправлять по умолчанияю первую страницу выводит... Не уверен что это вообще нужно. Кто же мешает в ручную написать 1-ю страницу. Какое то повторение кода...
        //{
        //    Console.WriteLine("1/" + NumberPage()); // какая страница открыта
        //    if (_dataContact.AmountOfContact() >= _numberOfLinesOnRender) //заполнена ли эта траница полностью, т.е проверяем контактов не меньше ли чем вывод нужно вывести на экран
        //    {
        //        _dataContact.StarnAndQuantityIssue(0, _numberOfLinesOnRender);
        //        for(int i=0; i<_numberOfLinesOnRender; i++)
        //        {
        //            Console.WriteLine("----------");
        //            Console.WriteLine(_dataContact.SequentialIssuanceOfContact());
        //            Console.WriteLine("----------");
        //        }
        //    }
        //    else
        //    {
        //        _dataContact.StarnAndQuantityIssue(0, _dataContact.AmountOfContact()); //задаем что хотим выводить потоком. Тут вывод от нуля(ибо первая странциа) до конца длины контактов
        //        for(int i=0; i<_dataContact.AmountOfContact(); i++)
        //        {
        //            Console.WriteLine("----------");
        //            Console.WriteLine(_dataContact.SequentialIssuanceOfContact());
        //            Console.WriteLine("----------");
        //        }
        //    }

        //}
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
                _dataContact.StarnAndQuantityIssue(_firstElemennt, _numberOfLinesOnRender);
                for (int i = _firstElemennt; i < _numberOfLinesOnRender + _firstElemennt; i++)
                {
                    try { _contact = _dataContact.IssuingElemetnNumber(i); }
                    catch
                    {
                        _contact[0] = "error elemetn";
                        _contact[1] = "error elemetn";
                    }
                    // обработай исключения потом как нибудь, не будь пидором
                    Console.WriteLine("----------");
                    Console.WriteLine(_contact[0]);
                    Console.WriteLine(_contact[1]);
                    Console.WriteLine("----------");
                }
            }
            else
            {
                _dataContact.StarnAndQuantityIssue(_firstElemennt, _dataContact.AmountOfContact()); //задаем что хотим выводить потоком. Тут вывод от нуля(ибо первая странциа) до конца длины контактов
                for (int i = _firstElemennt; i < _dataContact.AmountOfContact(); i++)
                {
                    try { _contact = _dataContact.IssuingElemetnNumber(i); }
                    catch
                    {
                        _contact[0] = "error elemetn";
                        _contact[1] = "error elemetn";
                    }
                    // 
                    Console.WriteLine("----------");
                    Console.WriteLine(_contact[0]);
                    Console.WriteLine(_contact[1]);
                    Console.WriteLine("----------");
                }
            }
        }
    }
}
