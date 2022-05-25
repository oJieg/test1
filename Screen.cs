using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test1
{
    public abstract class Screen
    {
        protected int NumberOfLinesOnRender { get; private set; } //сколько строк на одном экране
        protected int TotalPages { get; private set; }// сколько всего страниц

        protected int CurrentPageNumber { get; private set; }// открытая в данный момент страница
        protected int OffsetForTotalNumber { get; private set; } //offset текушей страницы
        protected int TakeForTotalNumber { get; private set; }//take для текушей страницы
        protected int LengthForTotalNumber { get; private set; } //сколько элементов на текущей страницы

        protected int FullAmountOfLines { get; set; } //в данный момент сколько строк есть в наличии для вывода
        protected bool PageCounter { get; set; } // выводить ли на экран отображение страницы текушей(1/5)
        private bool _exitFlag = false; //флаг выхода из окна

        protected ILogger Logger { get; set; }
        protected ILoggerFactory LoggerFactory { get; }
        public Screen(int numberOfLinesOnRender, ILoggerFactory loggerFactory)
        {
            LoggerFactory = loggerFactory;
            if (numberOfLinesOnRender == 0)
            {
                Logger = LoggerFactory.CreateLogger<Screen>();
                Logger.LogCritical("Конструктор Screen не может принимать numberOfLinesOnRender = 0"); 
                throw new ArgumentException("numberOfLinesOnRender не может быть нулем");
            }
            this.NumberOfLinesOnRender = numberOfLinesOnRender;
            CurrentPageNumber = 1;
        }



        //главный метод который нужно вызывать
        public void MainRender()
        {
            while (!_exitFlag)
            {
                Update();
                FullAmoutOfLines();
                ClearConsoleOptimazation.Clear();
                if (PageCounter)
                {
                    if (TotalPages == 0)
                    {
                        Console.WriteLine("список пуст, создайте новое");
                    }
                    else
                    {
                        PageCounterRender();
                    }

                }
                Title();
                PageRender(DataForPageRender());
                ChoiseMenuRender();

                KeyInput(out int InputInt, out ConsoleKey InputKay);
                ChoiceInput(InputInt, InputKay);
            }
        }

        //рендер тукушей стоницы (1/5)
        protected void PageCounterRender()
        {
            Console.WriteLine($"{CurrentPageNumber}/{TotalPages}");
        }

        //считает сколько страниц всего в наличии
        protected void FullAmoutOfLines()
        {
            float numberPage = (float)FullAmountOfLines / (float)NumberOfLinesOnRender;
            TotalPages = (int)Math.Ceiling(numberPage);
        }

        //явно возврашает первый элемент и кол-во элементов для текушей страницы
        protected void TakeAndOffsetForTotalPage()
        {
            OffsetForTotalNumber = (CurrentPageNumber - 1) * NumberOfLinesOnRender;
            TakeForTotalNumber = NumberOfLinesOnRender;
            if ((OffsetForTotalNumber + TakeForTotalNumber) >= FullAmountOfLines)
            {
                LengthForTotalNumber = FullAmountOfLines - OffsetForTotalNumber;
            }
            else
            {
                LengthForTotalNumber = NumberOfLinesOnRender;
            }
        }

        protected static void MessageForNotValidInput(string message)
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
            if (CurrentPageNumber < TotalPages)
            {
                CurrentPageNumber++;
            }
        }

        protected void PreviousPage()
        {
            if (CurrentPageNumber > 1)
            {
                CurrentPageNumber--;
            }
        }

        protected void KeyInput(out int InputInt, out ConsoleKey inputKay)
        {
            ConsoleKeyInfo key = Console.ReadKey();

            inputKay = key.Key;
            string InputString = inputKay.ToString();
            if (InputString.Length > 1 && (InputString.Remove(1) == "D" || InputString.Remove(3) == "Num"))
            {
                try
                {
                    InputInt = Convert.ToInt32(InputString[^1].ToString());
                    return;
                }
                catch (Exception ex)
                {
                    Logger.LogError(ex, "Произошла ошибка конвертации названия клавиши в цифровое представление." +
                        "нажата клавиша {InputString}.", InputString);
                }
            }
            InputInt = -1;
        }

        //метод красивого отображения)
        protected static void PageRender(List<string> dataForPageRender)
        {
            int i = 1;
            foreach (var dataRender in dataForPageRender)
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
        protected virtual void ChoiceInput(int inputInt, ConsoleKey inputKay)
        {
            if (inputKay == ConsoleKey.RightArrow)
            {
                NextPage();
                return;
            }
            if (inputKay == ConsoleKey.LeftArrow)
            {
                PreviousPage();
                return;
            }
            if (inputInt == 0 || inputKay == ConsoleKey.Escape)
            {
                ExitScreen();
                return;
            }
        }

        //рендер меню выбора
        protected virtual void ChoiseMenuRender()
        {
            Console.WriteLine("Стрелочки на клавиатуре - переход по страницам, 0 или Escape - вернуться на предыдушее окно выбора");
            Console.WriteLine($"выберети пункт от 1 до { LengthForTotalNumber}");
        }

        //заготовок
        protected virtual void Title()
        {

        }

        //сюда кладем чего отображать
        protected abstract List<string> DataForPageRender();
    }
}