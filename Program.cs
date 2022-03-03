

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
        public string[] SequentialIssuanceOfContact() //потоковая выдача, 0- имя 1- телефон !!! короче это правальная тема какая то, убрал ее ибо не смог сделать ее устойчивой от не вверных входных данных!
        {
            if (_quantityIssue>0) //
            {
                string[] _outData = new string[2];

                _outData[0] = _name[_numberIssue]; //тут явно для безопасности нужно что то делать, а то все ломается легко(
                _outData[1] = _phone[_numberIssue];
                _quantityIssue--;
                _numberIssue++;
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

        public void TestAddContact(int length)
        {
            for(int i = 0; i<length;i++)
            {
                AddContact("имя"+i, "телефон"+i);
            }
        }
    }

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
           if(_amountOfContact/_numberOfLinesOnRender>0)  
            {
                if(_amountOfContact%_numberOfLinesOnRender !=0 )
                {
                    return  _amountOfContact / _numberOfLinesOnRender + 1;
                }
                else { return  _amountOfContact / _numberOfLinesOnRender; }
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
            if (numberPage<=0) { numberPage = 1;}
            if (numberPage > NumberPage()) { numberPage = NumberPage();}   //зашита от долбаебов которые хер знает какую страницу открыть.

            Console.WriteLine(numberPage + "/" + NumberPage()); // какая страница открыта
            Console.WriteLine(); //пустая строка
            int _firstElemennt = (numberPage-1) * _numberOfLinesOnRender; //номер первого элемента на этой страницы
            string[] _contact = new string[2]; //для временного хранения контакта

            if (_dataContact.AmountOfContact() >= _firstElemennt + _numberOfLinesOnRender) //заполнена ли эта траница полностью, т.е проверяем контактов не меньше ли чем вывод нужно вывести на экран
            {
                _dataContact.StarnAndQuantityIssue(_firstElemennt, _numberOfLinesOnRender);
                for (int i = _firstElemennt; i < _numberOfLinesOnRender + _firstElemennt; i++)
                {
                    try { _contact = _dataContact.IssuingElemetnNumber(i); }
                    catch 
                    { 
                        _contact[0]= "error elemetn";
                        _contact[1]= "error elemetn";
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
                for (int i = _firstElemennt; i < _dataContact.AmountOfContact() ; i++)
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
            if(_currentPage>1)
            {
                _currentPage--;
            }
        }

 }



    internal class Program
    {
        static void Main(string[] args)
        {
            BaseDataContacts newBook = new BaseDataContacts();
            RenderContact render = new RenderContact(newBook, 4);
            MenuInput input = new MenuInput(render, newBook);

            newBook.TestAddContact(6); // что бы не пустое было
            int page = 1; //какую страницу рендерить дальше
            do
            {
                 render.RenderPage(page);
                 page = input.MainInput();
            }
            while (page != 0);

        }
    }
}