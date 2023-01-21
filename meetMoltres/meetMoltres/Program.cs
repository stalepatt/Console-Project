class Sokoban
{
    static void Main()
    {
        // Console Initial Settings
        Console.ResetColor();
        Console.CursorVisible = false;
        Console.Title = "LegendaryMoltres";
        Console.BackgroundColor = ConsoleColor.Gray;
        Console.Clear();


        
        int playerX = 5;
        int playerY = 5;
        
        while (true)
        {
            //---------render---------
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(playerX, playerY);
            Console.Write("P");

            //---------입력---------
            ConsoleKey key = Console.ReadKey().Key;
            
            //---------처리---------
            if (key == ConsoleKey.LeftArrow)
            {
                playerX = Math.Clamp(playerX - 1, 0, 15);
            }
            if (key == ConsoleKey.RightArrow)
            {
                playerX = Math.Clamp(playerX + 1, 0, 15);
            }
            if (key == ConsoleKey.UpArrow)
            {
                playerY = Math.Clamp(playerY - 1, 0, 15);
            }
            if (key == ConsoleKey.DownArrow)
            {
                playerY = Math.Clamp(playerY + 1, 0, 15);
            }
        }



    }
}