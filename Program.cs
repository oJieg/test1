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
            ILogger logger = loggerFactory.CreateLogger<Program>();

            int numberOfLinesOnRender = 4;
            Screen screenChoise = new ScreenSelect(numberOfLinesOnRender, logger);
            screenChoise.MainRender();
        }
    }
}