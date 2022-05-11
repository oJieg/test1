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
        private readonly string fullAddressDirectory;
        private readonly IDataContactInterface _tupeBD;

        public ScreenFileSelection(int numberOfLinesOnRender, IDataContactInterface tupeBD, ILoggerFactory loggerFactory)
            : base(numberOfLinesOnRender, loggerFactory)
        {
            Logger = loggerFactory.CreateLogger<ScreenFileSelection>();
            fullAddressDirectory = Path.Combine(Directory.GetCurrentDirectory(), nameDirectory);
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

            if (takeAndOffset > listNameFile.Length)
            {
                Logger.LogCritical("Произошла попытка обрашение к не сушествующему элементу массива listNameFile.");
                data.Add("error");
                return data;
            }

            try
            {
                for (int i = OffsetForTotalNumber; i < takeAndOffset; i++)
                {
                    data.Add(Path.GetFileName(listNameFile[i]));
                }
            }

            catch (Exception ex)
            {
                Logger.LogCritical(ex, "Не получилось создать  список имен файлов для рендеренга.");
            }
            return data;
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine($"Для создания нового файла выберете N");
            base.ChoiseMenuRender();
        }

        protected override void ChoiceInput(int InputInt, ConsoleKey InputKay)
        {
            base.ChoiceInput(InputInt, InputKay);
            if (InputKay == ConsoleKey.N)
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

                    ScreenMainBD screenMainBD = new(NumberOfLinesOnRender, _tupeBD, LoggerFactory);
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
                if (!Directory.Exists(fullAddressDirectory))
                {
                    Directory.CreateDirectory(fullAddressDirectory);
                    Logger.LogInformation("директория {fullAdresDirectory} создана", fullAddressDirectory);
                }
                listNameFile = Directory.GetFiles(fullAddressDirectory, $"*{_formatFile}");
                FullAmountOfLines = listNameFile.Length;
            }
            catch (IOException ex)
            {
                Logger.LogError(ex, "Каталог, заданный {fullAdresDirectory}, является файлом. " +
                    "Или  Имя сети неизвестно.", fullAddressDirectory);
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

            string nameFile = Console.ReadLine();

            if (_tupeBD.CreateFile(nameDirectory, nameFile))
            {
                Logger.LogInformation("был успешно создан файл {nameFile}", nameFile);
                NextSkreen(Path.Combine(Directory.GetCurrentDirectory(), fullAddressDirectory, $"{nameFile}{_formatFile}"));

            }
        }
    }
}