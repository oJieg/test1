using System;
using System.Collections.Generic;

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
                Title();
                PageRender(DataForPageRender());
                ChoiseMenuRender();

                KeyInput(out int InputInt, out string InputString);
                ChoiseInpyt(InputInt, InputString);
            }
        }

        //рендер тукушей стоницы (1/5)
        protected void PageCounterRender()
        {
            Console.WriteLine($"{_currentPageNumber}/{_totalPages}");
        }

        //считает сколько страниц всего в наличии
        protected void FullAmoutOfLine()
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

        protected void MessageForNotValidInput(string message)
        {
            Console.WriteLine($"{message}, Для продлжения нажмите любую клавишу");
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

        protected void KeyInput(out int InputInt, out string InputString)
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
        
        //метод красивого отображения)
        protected void PageRender(List<string> dataForPageRender)
        {
            int i = 1;
            foreach(var dataRender in dataForPageRender)
            {
                Console.WriteLine($"{i} | {dataRender}");
                i++;
            }
        }

        //-------------------------------------------------------
        //запускается перед каждой новой итарацией вызова
        protected virtual void Update()
        {
            TakeAndOffsetForTotalPage();
        }

        //обработка выбора
        protected virtual void ChoiseInpyt(int inputInt, string inputString)
        {
            if (inputString == "RightArrow")
            {
                NextPage();
                return;
            }
            if (inputString == "LeftArrow")
            {
                PreviousPage();
                return;
            }
            if (inputInt == 0 || inputString == "Escape")
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

        //заготовок
        protected virtual void Title()
        {

        }

        //сюда кладем чего отображать
        protected abstract List<string> DataForPageRender();
    }
}
