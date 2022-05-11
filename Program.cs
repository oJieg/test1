using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Extensions.Logging;

namespace test1
{
    public class Program
    {
        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new NLog.Extensions.Logging.NLogLoggerFactory();
           // ILogger logger = loggerFactory.CreateLogger<Program>();

            int numberOfLinesOnRender = 4;
            Screen screenChoise = new ScreenSelect(numberOfLinesOnRender, loggerFactory);
            screenChoise.MainRender();
        }
    }
}

//int a = 1;
//int b = 2;

//int ExpectedResult = 3;

//ValidationInputClass s = new ValidationInputClass();
//int outResultat = s.TestAdd(a, b);

//Assert.Equal(ExpectedResult, outResultat);