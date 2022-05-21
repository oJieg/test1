using System.Collections.Generic;
using System;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace test1
{
    public class Program
    {
        static async Task Main(string[] args)
        {
           // TestAsyunc testAsyunc = new();
           //await testAsyunc.MainMetod();

            ILoggerFactory loggerFactory = new NLog.Extensions.Logging.NLogLoggerFactory();
            // ILogger logger = loggerFactory.CreateLogger<Program>();

            int numberOfLinesOnRender = 4;
            Screen screenChoise = new ScreenSelect(numberOfLinesOnRender, loggerFactory);
           await screenChoise.MainRender();
        }
    }
}
