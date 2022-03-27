using System.Collections.Generic;
using System;

namespace test1
{
    public class Program
    {
        static void Main(string[] args)
        {
            IDataContactInterface contactBD = ChoiceOfRecordingMethod.InputMenuChoice();

            RenderContact render = new(contactBD, 4);
            MenuInput input = new(render, contactBD);

            int page = 1; //какую страницу рендерить дальше
            do
            {
                render.RenderPage(page);
                page = input.MainInput();
            }
            while (page != 0);
        }

    }
}