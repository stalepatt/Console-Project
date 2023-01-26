using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meetMoltres
{
    public static class Input
    {
        public static ConsoleKey _key;
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
