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
        protected int _totalPages; // сколько всего страниц

        protected int _currentPageNumber; // открытая в данный момент страница
        protected int _offsetForTotalNumber; //offset текушей страницы
        protected int _takeForTotalNumber; //take для текушей страницы

        protected int _fullAmountOfLine; //в данный момент сколько строк есть в наличии для вывода
        protected bool _pageCounterRender; // выводить ли на экран отображение страницы текушей(1/5)
        private bool _exitFlag = false; //флаг выхода из окна

        public Screen(int numberOfLinesOnRender)
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            //_fullAmountOfLine = FullAmoutOfLine();
            _pageCounterRender = false;
            _currentPageNumber = 1;
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

                KeyInput(out int InputInt, out string InputString);
                ChoiseInpyt(InputInt, InputString);

            }
        }

        //запускается перед каждой новой итарацией вызова
        protected virtual void Update()
        {
            TakeAndOffsetForTotalPage();
        }

        //рендер тукушей стоницы (1/5)
        protected void PageCounterRender()
        {
            Console.WriteLine($"{_currentPageNumber}/{_totalPages}");
        }

        //считает сколько страниц всего в наличии
        protected virtual void FullAmoutOfLine()
        {
            int numberPage = _fullAmountOfLine / _numberOfLinesOnRender;

            if (numberPage > 0)
            {
                if (_fullAmountOfLine % _numberOfLinesOnRender != 0)
                {
                    _totalPages = numberPage + 1;
                }
                else
                {
                    _totalPages = numberPage;
                }
            }
            else
            {
                _totalPages = 1;
            }
        }

        //явно возврашает первый элемент и кол-во элементов
        protected void TakeAndOffsetForTotalPage()
        {
            _offsetForTotalNumber = (_currentPageNumber - 1) * _numberOfLinesOnRender;
            _takeForTotalNumber = _numberOfLinesOnRender;
        }
        protected virtual void MessageForNotValidInput(string message)
        {
            Console.WriteLine($"{message}, Для продлжения нажмите Enter");
            Console.ReadKey();
        }

        //дефолт -1
        protected void ExitScreen()
        {
            _exitFlag = true;
        }

        protected void NextPage()
        {
            if (_currentPageNumber < _totalPages)
            {
                _currentPageNumber++;
            }
        }

        protected void PreviousPage()
        {
            if (_currentPageNumber > 1)
            {
                _currentPageNumber--;
            }
        }

        protected virtual void KeyInput(out int InputInt, out string InputString)
        {
            ConsoleKeyInfo kay = Console.ReadKey();

            InputString = kay.Key.ToString();
            if (InputString.Length > 1 && (InputString.Remove(1) == "D" || InputString.Remove(3) == "Num"))
            {
                try
                {
                    InputInt = Convert.ToInt32(InputString[InputString.Length - 1].ToString());
                    return;
                }
                catch (Exception ex)
                {
                    //логи
                }
            }
            InputInt = -1;

        }

        //обработка выбора
        protected virtual void ChoiseInpyt(int InputInt, string InputString)
        {
            if (InputString == "RightArrow")
            {
                NextPage();
                return;
            }
            if (InputString == "LeftArrow")
            {
                PreviousPage();
                return;
            }

            if (InputInt == 0 || InputString == "Escape")
            {
                ExitScreen();
                return;
            }
        }

        //рендер меню выбора
        protected virtual void ChoiseMenuRender()
        {
            Console.WriteLine("для переключения страниц используйте стрелочки, назад - 0 или Escape");
        }

        //сюда кладем чего отображать
        protected abstract void PageRender();
    }
}
