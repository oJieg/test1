﻿using System;

namespace test1
{
    public class ScreenChoise : Screen
    {

        public ScreenChoise(int numberOfLinesOnRender)
            : base(numberOfLinesOnRender)
        {
        }

        protected override void PageRender()
        {
            Console.WriteLine("выберите от куда читать базу данных");
            Console.WriteLine("1-из ОЗУ (данные стираются после работы");
            Console.WriteLine("2-из SQL базы данных");
            Console.WriteLine("3-из CVS таблици ");
            Console.WriteLine("0 - выход");
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine("введите 1-3 или 0 для выхода");
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
                Console.WriteLine($"{nameFile}вызов окна с передачей ему нужного интерфейка");
                Console.ReadLine();
            }

        }
    }
}
