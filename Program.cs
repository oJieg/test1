

using System;
using System.Collections.Generic;

namespace test1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            BaseDataContacts newBook = new BaseDataContacts();
            RenderContact render = new RenderContact(newBook, 4);
            MenuInput input = new MenuInput(render, newBook);

            newBook.TestAddContact(6); // что бы не пустое было
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