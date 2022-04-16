using System.Collections.Generic;
using System;

namespace test1
{
    public class Program
    {
        static void Main(string[] args)
        {
            Screen screenChoise = new ScreenChoise(3);
            screenChoise.MainRender();
        }
    }
}