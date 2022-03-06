using System;


namespace test1
{
    public class RenderContact
    {
        private readonly int _numberOfLinesOnRender; //сколько строк на одном экране
        private readonly BaseDataContacts _dataContact; //ссылка на БД

        //в контструкторе задаем сколько строк может быть одновременно на экране
        //и присылваем экземпляр класса зазы данных
        public RenderContact(BaseDataContacts data, int numberOfLinesOnRender) 
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            _dataContact = data;
        }

        public int NumberPage() // сколько страниц выходит из расчета количества контактов и длины строк на экране.
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

        //вывод определенной страницы, если число больше чем всего станиц то последнию выводит,
        //если меньше нуля, то первую.
        public void RenderPage(int numberPage) 
        {                                      
            Console.Clear();

            if (numberPage <= 0)
            {
                numberPage = 1;
            }
            if (numberPage > NumberPage())
            {
                numberPage = NumberPage();
            }

            Console.WriteLine(numberPage + "/" + NumberPage()); // какая страница открыта
            Console.WriteLine(); //пустая строка
            int firstElement = (numberPage - 1) * _numberOfLinesOnRender; //номер первого элемента на этой страницы

            //заполнена ли эта траница полностью, т.е проверяем контактов не меньше ли чем вывод нужно вывести на экран
            if (_dataContact.AmountOfContact() >= firstElement + _numberOfLinesOnRender)                                                                    
            {                                                                               

                try
                {
                    RenderingContact(_dataContact.TakeContact(firstElement, _numberOfLinesOnRender));
                }
                catch (Exception)
                {
                    Console.WriteLine("error");
                }
            }
            else
            {
                try 
                {
                    //разделил на 2 части что бы умешалось в 120 строк)
                    RenderingContact(
                        _dataContact.TakeContact(firstElement, _dataContact.AmountOfContact() - firstElement)); 
                }
                catch (Exception)
                {
                    Console.WriteLine("error");
                }
            }
        }

        private void RenderingContact(Contact[] contact)
        {

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
