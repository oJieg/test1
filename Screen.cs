using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    public abstract class Screen
    {
        protected int _numberOfLinesOnRender; //сколько строк на одном экране
        protected int _totalNumberPage; // открытая в данный момент страница
        protected int _offsetForTotalNumber; //offset текушей страницы
        protected int _takeForTotalNumber;
        protected int _maxAmoutOfPage; // сколько всего страниц
        protected int _fullAmountOfLine; //в данный момент сколько строк есть в наличии
        protected bool _pageCounterRender; // выводить ли на экран отображение страницы текушей(1/5)
        private bool _exitFlag = false; //флаг выхода из приложения

        public Screen(int numberOfLinesOnRender)
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            //_fullAmountOfLine = FullAmoutOfLine();
            _pageCounterRender = false;
            _totalNumberPage = 1;
        }

        //главный метод который нужно вызывать
        public void MainRender()
        {
            while (!_exitFlag)
            {
                Update();
                FullAmoutOfLine();
                Console.Clear();
                if (_pageCounterRender)
                {
                    PageCounterRender();
                }
                PageRender();
                ChoiseMenuRender();
                ChoiseInpyt();
                //MessageForNotValidInput();
            }


            //RenderPage(_totalNumberPage);
            //ChoiceAction(MainInput());
            //RenderPage(_totalNumberPage);
        }

        //запускается перед каждой новой итарацией вызова
        protected virtual void Update()
        {
            TakeAndOffsetForTotalPage();
        }

        //рендер тукушей стоницы (1/5)
        protected void PageCounterRender()
        {
            Console.WriteLine($"{_totalNumberPage}/{_maxAmoutOfPage}");
        }

        //считает сколько страниц всего в наличии
        protected virtual void FullAmoutOfLine()
        {
            int numberPage = _fullAmountOfLine / _numberOfLinesOnRender;

            if (numberPage > 0)
            {
                if (_fullAmountOfLine % _numberOfLinesOnRender != 0)
                {
                    _maxAmoutOfPage =  numberPage + 1;
                }
                else
                {
                    _maxAmoutOfPage =  numberPage;
                }
            }
            else
            {
                _maxAmoutOfPage =  1;
            }
        }

        //явно возврашает первый элемент и кол-во элементов
        protected void TakeAndOffsetForTotalPage()
        {
            int numberPage;
            numberPage = _totalNumberPage <= 0 ? 1 : _totalNumberPage;
            numberPage = _totalNumberPage >= _maxAmoutOfPage ? _maxAmoutOfPage : _totalNumberPage;

            _offsetForTotalNumber = (numberPage - 1) * _numberOfLinesOnRender;
            _takeForTotalNumber = _numberOfLinesOnRender;
        }
        protected virtual void MessageForNotValidInput(string message)
        {
            Console.WriteLine($"{message}, Для продлжения нажмите Enter");
            Console.ReadLine();
        }

        //дефолт -1
        protected void ExitScreen()
        {
            _exitFlag = true;
        }

        protected void NextPage()
        {
            if (_totalNumberPage < _maxAmoutOfPage)
            {
                _totalNumberPage++;
            }
        }

        protected void PreviousPage()
        {
            if (_totalNumberPage > 1)
            {
                _totalNumberPage--;
            }
        }

        //запрос пользователя и бработка выбора
        protected abstract void ChoiseInpyt();

        //рендер меню выбора
        protected abstract void ChoiseMenuRender();

        //сюда кладем чего отображать
        protected abstract void PageRender();
    }
}
