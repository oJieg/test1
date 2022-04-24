using System.Collections.Generic;
using System;
using System.IO;
using NLog;

namespace test1
{
    public class Program
    {
        static void Main(string[] args)
        {
            int numberOfLinesOnRender = 4;
            Screen screenChoise = new ScreenSelect(numberOfLinesOnRender);
            screenChoise.MainRender();
        }
    }
}