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
        public AsyncWaitingLoading()
        {
            //  Console.Write(outText);
        }
        public async Task<List<string>> RunAsyncWaitingLoading(AsyncCallBD asyncCallBD)
        {
            Console.Clear();
            List<string> result = new();
            Task<List<string>> resultTask = asyncCallBD();

            do
            {
                await Task.Delay(100);
                WaitingLoadingRender();
            }
            while (resultTask.Status != TaskStatus.RanToCompletion);

            result = await resultTask;
            Console.Clear();
            return result;

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
                    Console.Write("\b" + @"|");
                    _condition = 2;
                    break;
                case 2:
                    Console.Write("\b" + "/");
                    _condition = 0;
                    break;
            }
        }

    }
}
