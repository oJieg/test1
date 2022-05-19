using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace test1
{
    internal class WaitingLoading
    {
        private int _condition = 0;
        private static string outText = "загрузка ";
        public WaitingLoading()
        {
            Console.Write(outText);
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
                    _condition =0;
                    break;
            }
        }

    }
}
