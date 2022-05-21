using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace test1
{
    public delegate Task<List<string>> AsyncCallBD();
    public class AsyncWaitingLoading
    {
        private int _condition = 0;
        private static string outText = "загрузка ";

        public async Task<List<string>> RunAsyncWaitingLoading(AsyncCallBD asyncCallBD)
        {
            clear();
            Console.Write(outText);
            List<string> result = new();
            Task<List<string>> resultTask = asyncCallBD();

            while (resultTask.Status != TaskStatus.RanToCompletion) 
            {
                await Task.Delay(100);
                WaitingLoadingRender();
            }

            result = await resultTask;
            clear();
            return result;
        }

        private void clear()
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            for (int y = 0; y < Console.WindowHeight; y++)
                Console.Write(new String(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
        }
        public void WaitingLoadingRender()
        {
            switch (_condition)
            {
                case 0:
                    Console.Write("\b" + @"\");
                    _condition = 1;
                    break;
                case 1:
                    Console.Write("\b" + "|");
                    _condition = 2;
                    break;
                case 2:
                    Console.Write("\b" + "/");
                    _condition = 3;
                    break;
                case 3:
                    Console.Write("\b" + "|");
                    _condition = 0;
                    break;
            }
        }
    }
}
