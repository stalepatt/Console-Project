namespace meetMoltres
{
    public static class Input
    {
        private static ConsoleKey _key;
        public static void Process()
        {
            _key = default;
            if (Console.KeyAvailable)
            {
                _key = Console.ReadKey().Key;
            }
        }
        public static bool IsKeyDown(ConsoleKey key)
        {
            if (_key == key)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
