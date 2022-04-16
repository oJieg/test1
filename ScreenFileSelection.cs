using System;
using System.Collections.Generic;
using System.IO;

namespace test1
{
    public class ScreenFileSelection : Screen
    {
        private string _formatFile;
        private string[] listNameFile;
        //private int amountOfNameFile;
        private string _nameFile;

        public ScreenFileSelection(int numberOfLinesOnRender, string formatFile)
            : base(numberOfLinesOnRender)
        {
            //_numberOfLinesOnRender = numberOfLinesOnRender;
            _formatFile = formatFile;
            _pageCounterRender = true;
        }

        public string GetNameFile()
        {
            return _nameFile;
        }

        protected override void Update()
        {
            base.Update();
            ListNameFile();
        }

        protected override List<string> DataForPageRender()
        {
            int takeAndOffset = _offsetForTotalNumber + _takeForTotalNumber;
            if (takeAndOffset > _fullAmountOfLine)
            {
                takeAndOffset = _fullAmountOfLine;
            }
            List<string> data = new();

            for (int i = _offsetForTotalNumber; i < takeAndOffset; i++)
            {
                data.Add(Path.GetFileName(listNameFile[i]));
            }
            return data;
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
                return;
            }

            if (InputInt > 0 && InputInt <= _takeForTotalNumber)
            {
                _nameFile = listNameFile[InputInt - 1 + _offsetForTotalNumber];
                ExitScreen();
                return;
            }

        }
        //----------------------------------------------------------------------------
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
                if (!ValidationInputClass.TryValidatinNameFile(nameFile)
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
