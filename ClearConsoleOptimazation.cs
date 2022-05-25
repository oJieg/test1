using System;

namespace test1
{
    public static class ClearConsoleOptimazation
    {
        public static void Clear()
        {
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = false;
            for (int y = 0; y < Console.WindowHeight; y++)
                Console.Write(new String(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, 0);
            Console.CursorVisible = true;
        }
    }
}
