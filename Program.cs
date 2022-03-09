namespace test1
{
    public class Program
    {
        static void Main(string[] args)
        {
            BaseDataContacts newBook = new("Test1");
            RenderContact render = new(newBook, 4);
            MenuInput input = new(render, newBook);

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



//BaseDataContacts newBook = new();
//RenderContact render = new(newBook, 4);
//MenuInput input = new(render, newBook);

//int page = 1; //какую страницу рендерить дальше
//do
//{
//    render.RenderPage(page);
//    page = input.MainInput();
//}
//while (page != 0);