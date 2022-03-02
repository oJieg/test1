

using System;

namespace test1
{
    class Test
    {
        protected readonly string _testOutText;
        public Test(string textOut)
        {
            _testOutText = textOut;
        }

        public void PrintTestText()
        {
            Console.WriteLine(_testOutText);
        }

    }

    class TestEmplou : Test
    {
        public int _count {get; private set;}

        public TestEmplou(string textOut, int count)
            :base(textOut)
        { 
            _count = count;
        }

        public void EditCount(int newCount)
        {
            _count=newCount;
        }

        public void PrintTestTextCount()
        {
            for(int i = 0; i < _count; i++)
            {
                Console.WriteLine(_testOutText);
            }
            
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("test");
            var classText = new Test("test text in class");
            classText.PrintTestText();

            var classText2 = new TestEmplou("emplou test", 10);
            classText2.PrintTestText();
            Console.WriteLine(classText2._count);
            classText2.PrintTestTextCount();

        }
    }
}