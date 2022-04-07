using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace test1
{
    public class FileSelectionScreen : Screen
    {
        private string _formatFile;
        private string[] listNameFile;
        //private int amountOfNameFile;
        private string _nameFile;
        private bool _flagCorrectNameFile = false;
        public FileSelectionScreen(int numberOfLinesOnRender, string formatFile)
            : base(numberOfLinesOnRender)
        {
            //_numberOfLinesOnRender = numberOfLinesOnRender;
            _formatFile = formatFile;
            _pageCounterRender = true;
        }

        public bool GetNameFile(out string nameFile)
        {
            nameFile = _nameFile;
            return _flagCorrectNameFile;
        }

        protected override void Update()
        {
            TakeAndOffsetForTotalPage();
            ListNameFile();
            _fullAmountOfLine = listNameFile.Length;
        }

        protected override void PageRender()
        {
            TakeAndOffsetForTotalPage();
            //for (int i = offset; i <take; i++)

            int i = 0;
            foreach (string nameFile in listNameFile)
            {
                int takeAndOffset = _offsetForTotalNumber+ _takeForTotalNumber;
                i++;
                if (i > _offsetForTotalNumber && i <= takeAndOffset)
                {
                    Console.WriteLine($"{i-_offsetForTotalNumber}-{Path.GetFileName(nameFile)}");
                }
                if (i >= takeAndOffset)
                {
                    return;
                }
            }
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine($"выберети файл от 1 до {_numberOfLinesOnRender}, для создания нового файла выберете N, для выхода 0");
            Console.WriteLine("для переключения страниц используйте стрелочки");
        }

        protected override void ChoiseInpyt()
        {
            ConsoleKeyInfo kay = Console.ReadKey();
            if (kay.Key == ConsoleKey.RightArrow)
            {
                NextPage();
                return;
            }
            if (kay.Key == ConsoleKey.LeftArrow)
            {
                PreviousPage();
                return;
            }
            string input = kay.Key.ToString();

            input = input[input.Length - 1].ToString();

            if (input == "n" || input == "N")
            {
                CreateFile();
                _flagCorrectNameFile = true;
                return;
            }

            if (input == "0")
            {
                ExitScreen();
                return;
            }

            try
            {
                int intImput = Convert.ToInt32(input);
                if (intImput > 0 && intImput <= _fullAmountOfLine)
                {
                    _flagCorrectNameFile = true;
                    _nameFile = listNameFile[intImput - 1+_offsetForTotalNumber];
                    ExitScreen();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageForNotValidInput("не ввеерный ввод:   ");
                return;
                //логи
            }
        }

        private void ListNameFile()
        {
            if (!Directory.Exists(@"\DataBase"))
            {
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\DataBase");
            }
            listNameFile = Directory.GetFiles(@$"{Directory.GetCurrentDirectory()}\DataBase", $"*.{_formatFile}");
        }

        private void CreateFile()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("введите имя файла: ");
                string? nameFile = Console.ReadLine();
                if (!ValidationImputClass.TryValidatoinNameFile(nameFile)
                    || File.Exists($@"{Directory.GetCurrentDirectory()}\DataBase\{nameFile}.{_formatFile}"))
                {
                    MessageForNotValidInput("недопустимые ссиволы");
                    continue;
                }

                try
                {
                    File.Create($@"{Directory.GetCurrentDirectory()}\DataBase\{nameFile}.{_formatFile}");
                    return;
                }
                catch (Exception)
                {
                    MessageForNotValidInput("ошибка создания файла");
                    //логи
                }
            }
        }
    }
}
