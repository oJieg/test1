using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;

namespace test1
{
    public class ScreenFileSelection : Screen
    {
        private readonly string _formatFile;
        private string[] listNameFile;

        //private string _nameFile;
        private const string nameDirectory = "DataBase";
        private readonly string fullAdresDirectory;
        private readonly IDataContactInterface _tupeBD;

        public ScreenFileSelection(int numberOfLinesOnRender, IDataContactInterface tupeBD, ILogger  logger)
            : base(numberOfLinesOnRender, logger)
        {
            fullAdresDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameDirectory);
            _formatFile = tupeBD.FormatFile;
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
            List<string> data = new(takeAndOffset);

            try
            {
                for (int i = OffsetForTotalNumber; i < takeAndOffset; i++)
                {
                    data.Add(Path.GetFileName(listNameFile[i]));
                }
            }
            catch (IndexOutOfRangeException ex)
            {
                Logger.LogCritical(ex, "Произошло обрашение к не сушествующему элементу массива listNameFile.");
            }
            catch (Exception ex)
            {
                Logger.LogCritical(ex, "ошибка получения списка для рендера его.");
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
                NextSkreen(listNameFile[InputInt - 1 + OffsetForTotalNumber]);
                return;
            }
        }

        //----------------------------------------------------------------------------
        private void NextSkreen(string nameFile)
        {
            try
            {
                if (_tupeBD.TryInitializationDB(nameFile))
                {
                    Logger.LogInformation("Откытие: {_nameFile}", nameFile);

                    ScreenMainBD screenMainBD = new(NumberOfLinesOnRender, _tupeBD, Logger);
                    screenMainBD.MainRender();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ошибка при выборе файла из списка");
            }
        }

        private void ListNameFile()
        {
            try
            {
                if (!Directory.Exists(fullAdresDirectory))
                {
                    Directory.CreateDirectory(fullAdresDirectory);
                    Logger.LogInformation("директория {fullAdresDirectory} создана", fullAdresDirectory);
                }
                listNameFile = Directory.GetFiles(fullAdresDirectory, $"*{_formatFile}");
                FullAmountOfLines = listNameFile.Length;
            }
            catch (IOException ex)
            {
                Logger.LogError(ex, "Каталог, заданный {fullAdresDirectory}, является файлом. " +
                    "Или  Имя сети неизвестно.", fullAdresDirectory);
            }
            catch (UnauthorizedAccessException ex)
            {
                Logger.LogError(ex, "У вызывающего объекта Directory.CreateDirectory отсутствует необходимое разрешение.");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "ошибка получения списка файлов с расширением {_formatFile}", _formatFile);
            }
        }

        private void CreateFile()
        {
            Console.Clear();
            Console.WriteLine("введите имя файла: ");
            string nameFile = string.Empty;
            nameFile += Console.ReadLine();

            if (_tupeBD.CreateFile(nameDirectory, nameFile))
            {
                Logger.LogInformation("был успешно создан файл {nameFile}", nameFile);
                NextSkreen(Path.Combine(Directory.GetCurrentDirectory(), fullAdresDirectory, $"{nameFile}{_formatFile}"));

            }
        }
    }
}