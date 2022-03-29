using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    public class ChoiceOfRecordingMethod
    {
        public static IDataContactInterface InputMenuChoice()
        {
            while (true)
            {
                switch (RenderMenuChoice())
                {
                    case 1: return InitializationBDName(new BaseDataContactsTemporary());
                    case 2: return InitializationBDName(new BaseDataContactsSQL());
                    case 3: return InitializationBDName(new BaseDataContactsCSV());
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
            Console.WriteLine("1-из ОЗУ(данные стираются после работы");
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

        private static IDataContactInterface InitializationBDName(IDataContactInterface inInterfase)
        {
            bool correctName = false;
            while (!correctName)
            {
                Console.WriteLine("Выберите имя:");
                string name = "";
                name += Console.ReadLine();
                correctName = inInterfase.TryInitializationDB(name);
            }
            return inInterfase;
        }
    }
}
