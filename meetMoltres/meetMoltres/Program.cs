namespace LegendaryMoltres
{
    enum Direction // type for saving direction
    {
        None,
        Left,
        Right,
        Up,
        Down
    }
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

            int rockX = 10;
            int rockY = 10;

            int wallX = 7;
            int wallY = 7;

            Direction playerMoveDirection = Direction.None;

            while (true)
            {
                //---------render---------
                Console.Clear();

                //--플레이어 출력
                Console.ForegroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(playerX, playerY);
                Console.Write("P");

                //--바위 출력
                Console.ForegroundColor = ConsoleColor.White;
                Console.SetCursorPosition(rockX, rockY);
                Console.Write("O");

                //--벽 출력
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.SetCursorPosition(wallX, wallY);
                Console.Write("#");


                //---------입력---------
                ConsoleKey key = Console.ReadKey().Key;

                //---------처리---------
                if (key == ConsoleKey.LeftArrow)
                {
                    playerX = Math.Clamp(playerX - 1, 0, 15);
                    playerMoveDirection = Direction.Left;
                }
                if (key == ConsoleKey.RightArrow)
                {
                    playerX = Math.Clamp(playerX + 1, 0, 15);
                    playerMoveDirection = Direction.Right;
                }
                if (key == ConsoleKey.UpArrow)
                {
                    playerY = Math.Clamp(playerY - 1, 0, 15);
                    playerMoveDirection = Direction.Up;
                }
                if (key == ConsoleKey.DownArrow)
                {
                    playerY = Math.Clamp(playerY + 1, 0, 15);
                    playerMoveDirection = Direction.Down;
                }

                // 플레이어와 바위가 충돌했을 때
                if (playerX == rockX && playerY == rockY)
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            {
                                rockX = Math.Clamp(rockX - 1, 0, 15);
                                playerX = rockX + 1;
                            }
                            break;
                        case Direction.Right:
                            {
                                rockX = Math.Clamp(rockX + 1, 0, 15);
                                playerX = rockX - 1;
                            }
                            break;
                        case Direction.Up:
                            {
                                rockY = Math.Clamp(rockY - 1, 0, 15);
                                playerY = rockY + 1;
                            }
                            break;
                        case Direction.Down:
                            {
                                rockY = Math.Clamp(rockY + 1, 0, 15);
                                playerY = rockY - 1;
                            }
                            break;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            return;
                    }

                }

                // 플레이어와 벽이 충돌했을 때
                if (playerX == wallX && playerY == wallY)
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            {
                                playerX = playerX + 1;
                            }
                            break;
                        case Direction.Right:
                            {
                                playerX = playerX - 1;
                            }
                            break;
                        case Direction.Up:
                            {
                                playerY = playerY + 1;
                            }
                            break;
                        case Direction.Down:
                            {
                                playerY = playerY - 1;
                            }
                            break;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            return;
                    }
                }

                // 바위와 벽이 충돌했을 때
                if (rockX == wallX && rockY == wallY)
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            {
                                rockX = Math.Clamp(wallX + 1, 0, 15);
                                playerX = rockX + 1;
                            }
                            break;
                        case Direction.Right:
                            {
                                rockX = Math.Clamp(wallX - 1, 0, 15);
                                playerX = rockX - 1;
                            }
                            break;
                        case Direction.Up:
                            {
                                rockY = Math.Clamp(wallY + 1, 0, 15);
                                playerY = rockY + 1;
                            }
                            break;
                        case Direction.Down:
                            {
                                rockY = Math.Clamp(wallY - 1, 0, 15);
                                playerY = rockY - 1;
                            }
                            break;
                        default:
                            {
                                Console.Clear();
                                Console.WriteLine($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            return;
                    }
                }
            }
        }
    }
}