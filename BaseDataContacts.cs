using System;
using System.Collections.Generic;


namespace test1
{
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
            if (_quantityIssue > 0) //
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
            if (numberElement < _name.Count)
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
            for (int i = 0; i < length; i++)
            {
                AddContact("имя" + i, "телефон" + i);
            }
        }
    }
}
