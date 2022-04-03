using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace test1
{
    public static class FileSelectionScreen
    {
        public static string NameFile(string formatFile)
        {
            RenderListFile("csv");
            //Console.WriteLine(InquryNewFile());
            return "1";
        }

        private static bool InquryNewFile()
        {
            while (true)
            {
                RenderInquryNewFile();
                switch (Console.ReadLine())
                {
                    case "1":
                        return true;
                    case "2":
                        return false;
                }
            }
        }

        private static void RenderInquryNewFile()
        {
            Console.Clear();
            Console.WriteLine("Создать новый или откыть имеющийся 1-новый 2-старый");
        }

        private static string RenderListFile(string formateFile)
        {
            string[] listFile = Directory.GetFiles(@$"{Directory.GetCurrentDirectory()}\DataBase", $"*.{formateFile}");
            if(listFile.Count() == 0)
            {
                Console.WriteLine("Файлов не найдено, создайте новый");
            }

            int i=0;
            foreach (string nameFile in listFile)
            {
                i++;
                Console.WriteLine($"{i}-{Path.GetFileName(nameFile)}");
            }

            

            //Directory.GetFiles("");
        }

    }
}
