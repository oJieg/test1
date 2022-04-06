using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace test1
{
    public class FileSelectionScreen : Screen
    {
        private string _formatFile;
        private string[] listNameFile;
        private int amountOfNameFile;
        private string nameFile;
        public FileSelectionScreen(int numberOfLinesOnRender, string formatFile)
            : base(numberOfLinesOnRender)
        {
            _formatFile = formatFile;
        }

        public string GetNameFile()
        {
            return nameFile;
        }

        protected override void Update()
        {
            ListNameFile();
            amountOfNameFile = listNameFile.Length;
        }
        protected override void PageRender()
        {
            int i = 0;
            foreach (string nameFile in listNameFile)
            {
                i++;
                Console.WriteLine($"{i}-{Path.GetFileName(nameFile)}");
            }
            
            
           
        }

        protected override void ChoiseMenuRender()
        {
            Console.WriteLine($"выберети файл от 1 до {amountOfNameFile}, для создания нового файла выберете 0");
        }

        protected override void ChoiseInpyt()
        {
           if( Console.ReadLine() == "0")
            {
                ExitScreen();
            }
           else
            {
                MessageForNotValidInput("nope");
            }
        }





        //public static string NameFile(string formatFile) //formatFile без точки
        //{
        //    while (true)
        //    {

        //        string[] listNameFile = ListNameFile(formatFile);
        //        int amountOfNameFile = listNameFile.Length;
        //        RenderListFile(listNameFile);
        //        int selectionNumberName = FileSelection(amountOfNameFile);

        //        if (selectionNumberName == -1)
        //        {
        //            CreateFile(formatFile);
        //        }
        //        else if (selectionNumberName >= 0 && selectionNumberName < amountOfNameFile)
        //        {
        //            return listNameFile[selectionNumberName];
        //        }
        //        else
        //        {
        //            Console.WriteLine("не верное значение, попробуйте еще раз, нажмите Enter для продолжения");
        //            Console.ReadLine();
        //        }
        //    }
        //}

        //+
        private void ListNameFile()
        {
            if (!Directory.Exists(@"\DataBase"))
            {
                Directory.CreateDirectory($@"{Directory.GetCurrentDirectory()}\DataBase");
            }
            listNameFile = Directory.GetFiles(@$"{Directory.GetCurrentDirectory()}\DataBase", $"*.{_formatFile}");
        }

        private static void RenderListFile(string[] listNameFile)
        {
            Console.Clear();
            int i = 0;
            foreach (string nameFile in listNameFile)
            {
                i++;
                Console.WriteLine($"{i}-{Path.GetFileName(nameFile)}");
            }
        }

        private static int FileSelection(int amountOfNameFile)
        {
            Console.WriteLine($"выберети файл от 1 до {amountOfNameFile}, для создания нового файла выберете 0");
            try
            {
                return Convert.ToInt32(Console.ReadLine()) - 1;
            }
            catch (Exception)
            {
                return -2;
            }
        }

        private static void CreateFile(string formatFile)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("введите имя файла: ");
                string? nameFile = Console.ReadLine();
                if (!ValidationImputClass.TryValidatoinNameFile(nameFile)
                    || File.Exists($@"{Directory.GetCurrentDirectory()}\DataBase\{nameFile}.{formatFile}"))
                {
                    Console.WriteLine("недопустимые символы или такой файл уже есть, попробуйте еще раз, для продолжения нажмте Enter");
                    Console.ReadLine();
                    continue;
                }

                try
                {
                    File.Create($@"{Directory.GetCurrentDirectory()}\DataBase\{nameFile}.{formatFile}");
                    return;
                }
                catch (Exception)
                {
                    //логи
                }
            }
        }
    }
}
