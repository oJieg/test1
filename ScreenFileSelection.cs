using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace test1
{
    public class ScreenFileSelection : Screen
    {
        private string _formatFile;
        private string[] listNameFile;
        //private int amountOfNameFile;
        private string _nameFile;
        private bool _flagCorrectNameFile = false;
        public ScreenFileSelection(int numberOfLinesOnRender, string formatFile)
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
            base.Update();
            ListNameFile();
        }

        protected override void PageRender()
        {
            int takeAndOffset = _offsetForTotalNumber + _takeForTotalNumber;
            if (takeAndOffset > _fullAmountOfLine)
            {
                takeAndOffset = _fullAmountOfLine;
            }

            for (int i = _offsetForTotalNumber; i < takeAndOffset; i++)
            {
                Console.WriteLine($"{i - _offsetForTotalNumber + 1}-{Path.GetFileName(listNameFile[i])}");
            }
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine($"выберети файл от 1 до {_numberOfLinesOnRender}, для создания нового файла выберете N, для выхода 0");
            base.ChoiseMenuRender();
        }

        protected override void ChoiseInpyt(int InputInt, string InputString)
        {
            base.ChoiseInpyt(InputInt, InputString);
            if (InputString == "n" || InputString == "N")
            {
                CreateFile();
                _flagCorrectNameFile = true;
                return;
            }

            if (InputInt > 0 && InputInt < _fullAmountOfLine)
            {
                _flagCorrectNameFile = true;
                _nameFile = listNameFile[InputInt - 1 + _offsetForTotalNumber];
                ExitScreen();
                return;
            }

        }

        private void ListNameFile()
        {
            if (!Directory.Exists(@"\DataBase"))
            {
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\DataBase");
            }
            listNameFile = Directory.GetFiles(@$"{Directory.GetCurrentDirectory()}\DataBase", $"*.{_formatFile}");
            _fullAmountOfLine = listNameFile.Length;
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
