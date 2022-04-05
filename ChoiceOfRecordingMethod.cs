using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    public class ChoiceOfRecordingMethod
    {
        public static void InputMenuChoice()
        {
            while (true)
            {
                switch (RenderMenuChoice())
                {
                    case 1: metodRendera(new BaseDataContactsTemporary());
                    case 2: InitializationBDName(new BaseDataContactsSQL(),"db");
                    case 3: InitializationBDName(new BaseDataContactsCSV(),"csv");
                    default:
                        Console.Clear();
                        Console.WriteLine("Не верное значение, попробуйте заново");
                        break;
                }
            }
        }

        private static int? RenderMenuChoice()
        {
            Console.WriteLine("выберите от куда читать базу данных");
            Console.WriteLine("1-из ОЗУ (данные стираются после работы");
            Console.WriteLine("2-из SQL базы данных");
            Console.WriteLine("3-из CVS таблици ");
            try
            {
                return Int32.Parse(Console.ReadLine());
            }
            catch (Exception)
            {
                return -1;
            }
        }

        private static void InitializationBDName(IDataContactInterface inInterfase, string formatFile)
        {
            string nameFile = FileSelectionScreen.RenderScreen(formatFile);


            bool correctName = false;
            while (!correctName)
            {
                Console.WriteLine("Выберите имя:");
                correctName = inInterfase.TryInitializationDB(Console.ReadLine());
            }
            return inInterfase;
        }
    }
}
