

using System;
using System.Collections.Generic;

namespace test1
{
    //class Test
    //{
    //    protected readonly string _testOutText;
    //    public Test(string textOut)
    //    {
    //        _testOutText = textOut;
    //    }

    //    public void PrintTestText()
    //    {
    //        Console.WriteLine(_testOutText);
    //    }

    //}

    //class TestEmplou : Test
    //{
    //    public int _count {get; private set;}

    //    public TestEmplou(string textOut, int count)
    //        :base(textOut)
    //    { 
    //        _count = count;
    //    }

    //    public void EditCount(int newCount)
    //    {
    //        _count=newCount;
    //    }

    //    public void PrintTestTextCount()
    //    {
    //        for(int i = 0; i < _count; i++)
    //        {
    //            Console.WriteLine(_testOutText);
    //        }
            
    //    }
    //}

    class BaseDataContacts
    {
        protected List<string> _name = new List<string>();
        protected List<string> _phone = new List<string>();
        private int _quantityIssue = 0; //для потоковой выдачи, сколько нужно выдать элементов
        private int _numberIssue = 0; //какой номер элемента выдавать


        //методы добавления контактов
        public void AddContact(string name, string phone) 
        {
            _name.Add(name);
            _phone.Add(phone);
        }
        public void AddContact(string name) //как минимум у контакта должено быть имя
        {
            _name.Add(name);
            _phone.Add("none");
        }

        //метод выдачи для полечения потоково сначало через StarnAndQuantityIssue задается с какого и сколько элементов вернуть, а потом вызываем указаное количество раз SequentialIssuanceOfContact для получение элементов //ну и велосипед я сделал блин(
        public void StarnAndQuantityIssue(int startPosition, int quantityIssue) //определяем с какого элемента и сколько нужно выдать в дальнейшем.
        {
            _numberIssue = startPosition;
            _quantityIssue = quantityIssue;
        }
        public string[] SequentialIssuanceOfContact() //потоковая выдача, 0- имя 1- телефон !!!обработай ошибки, не будь пидором!!!
        {
            if (_quantityIssue>0) //
            {
                string[] _outData = new string[2];
                _outData[0] = _name[_numberIssue];
                _outData[1] = _phone[_numberIssue];
                _quantityIssue--;
                return _outData;
            }
            else 
            {
                throw new Exception("xyi");
            }           
        }
        public int AmountOfContact() //количество контактов в базе
        {

            return _name.Count;
        }
        public string[] IssuingElemetnNumber(int numberElement) // для простоты можно тупо запросить какой то элемент и его вызвать конкретно так !!!и тут тоже обработай исключения!!!
        {
            if( numberElement< _name.Count)
            {
                string[] _outData = new string[2];
                _outData[0] = _name[numberElement];
                _outData[1] = _phone[numberElement];
                return _outData;
            }
            else { throw new Exception("net etogo elementa"); }

        }
    }

    class RenderContact
    {
        private int _numberOfLinesOnRender;
        private BaseDataContacts _dataContact;
        public RenderContact(BaseDataContacts data, int numberOfLinesOnRender) //в контструкторе задаем сколько строк может быть одновременно на экране и присылваем экземпляр класса зазы данных
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            _dataContact = data;
        }
        private int NumberPage() // сколько страниц выходит из расчета количества контактов и длины строк на экране
        {
            int _numberPage;
            int _amountOfContact = _dataContact.AmountOfContact();
           if(_amountOfContact/_numberOfLinesOnRender>0)  
            {
                if(_amountOfContact%_numberOfLinesOnRender !=0 )
                {
                    return _numberPage = _amountOfContact / _numberOfLinesOnRender + 1;
                }
                else { return _numberPage = _amountOfContact / _numberOfLinesOnRender; }
            }
            else { return 1; }
        }


        public void RenderPage()// если ничего не отправлять по умолчанияю первую страницу выводит
        {
            Console.WriteLine("1/" + NumberPage()); // какая страница открыта
            //дальше нужно писать обрашение к БД, но это вроде просто
        }
        public void RenderPage(int numberPage) //вывод определенной страницы, если число больше чем всего станиц то последнию выводит, если меньше нуля, то первую.
        {
            Console.WriteLine(numberPage + "/" + NumberPage()); // какая страница открыта
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            BaseDataContacts NewBook = new BaseDataContacts();

            Console.WriteLine(6 / 4);
            

            //Console.WriteLine("test");
            //var classText = new Test("test text in class");
            //classText.PrintTestText();

            //var classText2 = new TestEmplou("emplou test", 10);
            //classText2.PrintTestText();
            //Console.WriteLine(classText2._count);
            //classText2.PrintTestTextCount();



        }
    }
}