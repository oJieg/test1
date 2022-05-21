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
            ILoggerFactory loggerFactory = new NLog.Extensions.Logging.NLogLoggerFactory();

            int numberOfLinesOnRender = 4;
            Screen screenChoise = new ScreenSelect(numberOfLinesOnRender, loggerFactory);
            await screenChoise.MainRender();
        }
    }
}
