using System;
using System.Collections.Generic;

namespace test1
{
    public class ScreenSelect : Screen
    {
        private List<string> _data = new();
        public ScreenSelect(int numberOfLinesOnRender)
            : base(numberOfLinesOnRender)
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
                    ScreenMainBD screenMainBD = new ScreenMainBD(NumberOfLinesOnRender, new BaseDataContactsTemporary());
                    screenMainBD.MainRender();
                    break;
                case 2:
                    CallScreenFileSelection(new BaseDataContactsSQL());
                    break;
                case 3:
                    CallScreenFileSelection(new BaseDataContactsCSV());
                    break;
                default:
                    break;
            }
        }

        private void CallScreenFileSelection(IDataContactInterface inInterfase)
        {
            ScreenFileSelection fileSelector = new ScreenFileSelection(NumberOfLinesOnRender, inInterfase);
            fileSelector.MainRender();
        }
    }
}