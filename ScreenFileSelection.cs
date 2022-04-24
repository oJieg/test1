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
            PageCounter = true;
            _tupeBD = tupeBD;
        }

        protected override void Update()
        {
            ListNameFile();
            base.Update();
        }

        protected override List<string> DataForPageRender()
        {
            int takeAndOffset = OffsetForTotalNumber + TakeForTotalNumber;
            if (takeAndOffset > FullAmountOfLines)
            {
                takeAndOffset = FullAmountOfLines;
            }
            List<string> data = new();

            for (int i = OffsetForTotalNumber; i < takeAndOffset; i++)
            {
                data.Add(Path.GetFileName(listNameFile[i]));
            }
            return data;
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine($"Для создания нового файла выберете N");
            base.ChoiseMenuRender();
        }

        protected override void ChoiceInput(int InputInt, string InputString)
        {
            base.ChoiceInput(InputInt, InputString);
            if (InputString == "n" || InputString == "N")
            {
                CreateFile();
                return;
            }

            if (InputInt > 0 && InputInt <= LengthForTotalNumber)
            {
                _nameFile = listNameFile[InputInt - 1 + OffsetForTotalNumber];
                if (_tupeBD.TryInitializationDB(_nameFile))
                {
                    logger.Debug($"Откытие: {_nameFile}");

                    ScreenMainBD screenMainBD = new ScreenMainBD(NumberOfLinesOnRender, _tupeBD);
                    screenMainBD.MainRender();
                }
                //ExitScreen();
                return;
            }
        }

        //----------------------------------------------------------------------------
        private void ListNameFile()
        {
            if (!Directory.Exists(fullAdresDirectory)
            {
                Directory.CreateDirectory(fullAdresDirectory);
            }
            listNameFile = Directory.GetFiles(fullAdresDirectory, $"*{_formatFile}");
            FullAmountOfLines = listNameFile.Length;
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