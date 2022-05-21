using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test1
{
    public class TestAsyunc
    {
        public async Task MainMetod() 
        {
            Task timeRandom = PrintAsync("redi");
            AsyncWaitingLoading waitingLoading = new();

            do
            {
                await Task.Delay(100);
                waitingLoading.WaitingLoadingRender();
            }
            while (timeRandom.Status != TaskStatus.RanToCompletion);
        }
        private async Task PrintAsync(string text)
        {
            Random random = new Random();
            int time = random.Next(1000,10000);
            await Task.Delay(time);
            Console.WriteLine();
            Console.WriteLine($"{text} time = {time}" );
        }
    }
}
//Task one = PrintAsync("1");
//Task two = PrintAsync("2");
//Task three = PrintAsync("3");
//List<Task> tasks = new List<Task>() { one, two, three };
//Console.WriteLine("xxxxx");
//while (one.Status != TaskStatus.RanToCompletion)
//{
//    await Task.Delay(100);
//    Console.WriteLine("z");
//}

//await Task.WhenAll(tasks);
//Console.WriteLine("zzzz");