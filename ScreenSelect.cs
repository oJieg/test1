using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace test1
{
    public class ScreenSelect : Screen
    {
        private readonly List<string> _data = new();
        public ScreenSelect(int numberOfLinesOnRender, ILoggerFactory loggerFactory)
            : base(numberOfLinesOnRender, loggerFactory)
        {
            Logger = LoggerFactory.CreateLogger<ScreenSelect>();
            AddDataForRener();
            FullAmountOfLines = _data.Count;
        }

        private void AddDataForRener()
        {
            _data.Add("из ОЗУ (данные стираются после работы)");
            _data.Add("из SQL базы данных");
            _data.Add("из CVS таблицы");
        }

        protected override void Title()
        {
            Console.WriteLine("выберите от куда читать базу данных");
        }

        protected override List<string> DataForPageRender()
        {
            return _data;
        }

        protected override async Task ChoiceInput(int inputInt, ConsoleKey inputKay)
        {
            base.ChoiceInput(inputInt, inputKay);

            switch (inputInt)
            {
                case 1:
                    ScreenMainBD screenMainBD =
                        new(NumberOfLinesOnRender, new BaseDataContactsTemporary(LoggerFactory), LoggerFactory);
                    await screenMainBD.MainRender();
                    break;
                case 2:
                    await CallScreenFileSelection(new BaseDataContactsSQL(LoggerFactory));
                    break;
                case 3:
                    await CallScreenFileSelection(new BaseDataContactsCSV(LoggerFactory));
                    break;
                default:
                    break;
            }
        }

        private async Task CallScreenFileSelection(IDataContactInterface inInterfase)
        {
            ScreenFileSelection fileSelector = new(NumberOfLinesOnRender, inInterfase, LoggerFactory);
            await fileSelector.MainRender();
        }
    }
}