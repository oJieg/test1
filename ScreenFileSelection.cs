using System;
using System.Collections.Generic;
using System.IO;

namespace test1
{
    public class ScreenFileSelection : Screen
    {
        private string _formatFile;
        private string[] listNameFile;

        private string _nameFile;
        private const string nameDirectory = "DataBase";
        private string fullAdresDirectory;
        private IDataContactInterface _tupeBD;

        public ScreenFileSelection(int numberOfLinesOnRender, IDataContactInterface tupeBD)
            : base(numberOfLinesOnRender)
        {
            fullAdresDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameDirectory);
            _formatFile = tupeBD.FormatFile();
            _pageCounterRender = true;
            _tupeBD = tupeBD;
        }

        public string GetNameFile()
        {
            return _nameFile;
        }

        protected override void Update()
        {
            ListNameFile();
            base.Update();
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
            Console.WriteLine($"выберети файл от 1 до {_lengthForTotalNumber}, для создания нового файла выберете N, для выхода 0");
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

            if (InputInt > 0 && InputInt <= _lengthForTotalNumber)
            {
                _nameFile = listNameFile[InputInt - 1 + _offsetForTotalNumber];
                ExitScreen();
                return;
            }
        }

        //----------------------------------------------------------------------------
        private void ListNameFile()
        {
            if (!Directory.Exists(@$"\{nameDirectory}"))
            {
                Directory.CreateDirectory(fullAdresDirectory);
            }
            listNameFile = Directory.GetFiles(fullAdresDirectory, $"*{_formatFile}");
            _fullAmountOfLine = listNameFile.Length;
        }

        private void CreateFile()
        {
            Console.Clear();
            Console.WriteLine("введите имя файла: ");
            string nameFile = string.Empty;
            nameFile += Console.ReadLine();

            if (!_tupeBD.CreateFile(nameDirectory, nameFile))
            {
                MessageForNotValidInput("не верное название файла, можно файл уже сушествует");
            }
        }
    }
}
