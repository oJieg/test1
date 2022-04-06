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
        protected int _totalNumberPage { get; set; } // открытая в данный момент страница
        protected int _maxAmoutOfPage; // сколько всего страниц
        protected int _fullAmountOfLine; //в данный момент сколько строк есть в наличии
        protected bool _pageCounterRender; // выводить ли на экран отображение страницы текушей(1/5)
        private bool _exitFlag = false; //флаг выхода из приложения

        public Screen(int numberOfLinesOnRender)
        {
            _numberOfLinesOnRender = numberOfLinesOnRender;
            _fullAmountOfLine = FullAmoutOfLine();
            _pageCounterRender = false;
        }

        //главный метод который нужно вызывать
        public void MainRender()
        {
            while (!_exitFlag)
            {
                Update();
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
        //
        protected virtual void Update()
        {

        }

        //рендер тукушей стоницы (1/5)
        protected void PageCounterRender()
        {
            Console.WriteLine($"{_totalNumberPage}/{_maxAmoutOfPage}");
        }

        //возврашает сколько строк всего в наличии
        protected virtual int FullAmoutOfLine()
        {
            return 1;
        }

        //явно возврашает первый элемент и кол-во элементов
        protected void TakeAndOffsetForTotalPage(out int offset, out int take)
        {
            int numberPage;
            numberPage = _totalNumberPage <= 0 ? 1 : _totalNumberPage;
            numberPage = _totalNumberPage >= _maxAmoutOfPage ? _maxAmoutOfPage : _totalNumberPage;

            offset = (numberPage - 1) * _numberOfLinesOnRender;
            take = _numberOfLinesOnRender;
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
