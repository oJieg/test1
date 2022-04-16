﻿using System;
using System.Collections.Generic;

namespace test1
{
    public class ScreenChoise : Screen
    {

        public ScreenChoise(int numberOfLinesOnRender)
            : base(numberOfLinesOnRender)
        {
        }

        protected override void Title()
        {
            Console.WriteLine("выберите от куда читать базу данных");
        }

        protected override List<string> DataForPageRender()
        {
            List<string> data = new();

            data.Add("из ОЗУ (данные стираются после работы");
            data.Add("из SQL базы данных");
            data.Add("из CVS таблици ");
            return data;
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine("введите 1-3 или 0 или Escape для выхода");
        }

        protected override void ChoiseInpyt(int InputInt, string InputString)
        {
            base.ChoiseInpyt(InputInt, InputString);

            switch (InputInt)
            {
                case 1:
                    CallBD(new BaseDataContactsTemporary(), "");
                    break;
                case 2:
                    CallBD(new BaseDataContactsSQL(), "db");
                    break;
                case 3:
                    CallBD(new BaseDataContactsCSV(), "csv");
                    break;
                default:
                    //MessageForNotValidInput("Нет такого варианта");
                    break;
            }

        }

        private void CallBD(IDataContactInterface inInterfase, string formatFile)
        {
            ScreenFileSelection fileSelector = new ScreenFileSelection(2, formatFile);
            fileSelector.MainRender();
            if (fileSelector.GetNameFile(out string nameFile))
            {
                if (inInterfase.TryInitializationDB(nameFile))
                {
                    ScreenMainBD screenMainBD = new ScreenMainBD(_numberOfLinesOnRender, inInterfase);
                    screenMainBD.MainRender();
                }
                //Console.WriteLine($"{nameFile}вызов окна с передачей ему нужного интерфейка");
                //Console.ReadLine();
            }

        }

    }
}
