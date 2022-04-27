using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace test1
{
    public class ScreenSelect : Screen
    {
        private readonly List<string> _data = new();
        public ScreenSelect(int numberOfLinesOnRender, ILogger logger)
            : base(numberOfLinesOnRender, logger)
        {
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

        protected override void ChoiceInput(int inputInt, string inputString)
        {
            base.ChoiceInput(inputInt, inputString);

            switch (inputInt)
            {
                case 1:
                    ScreenMainBD screenMainBD = 
                        new(NumberOfLinesOnRender, new BaseDataContactsTemporary(Logger), Logger);
                    screenMainBD.MainRender();
                    break;
                case 2:
                    CallScreenFileSelection(new BaseDataContactsSQL(Logger));
                    break;
                case 3:
                    CallScreenFileSelection(new BaseDataContactsCSV(Logger));
                    break;
                default:
                    break;
            }
        }

        private void CallScreenFileSelection(IDataContactInterface inInterfase)
        {
            ScreenFileSelection fileSelector = new(NumberOfLinesOnRender, inInterfase, Logger);
            fileSelector.MainRender();
        }
    }
}