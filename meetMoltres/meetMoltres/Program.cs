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

            // 각 오브젝트 위치 좌표
            int playerX = 5;
            int playerY = 5;

            int rockX = 10;
            int rockY = 10;

            int wallX = 7;
            int wallY = 7;

            // 기호 상수 정의
            const int MIN_X = 0;
            const int MAX_X = 15;
            const int MIN_Y = 0;
            const int MAX_Y = 15;

            Direction playerMoveDirection = Direction.None;

            while (true)
            {
                //---------render---------
                Console.Clear();

                //--플레이어 출력
                RenderObject(playerX, playerY, "P", ConsoleColor.Red);

                //--바위 출력
                RenderObject(rockX, rockY, "O", ConsoleColor.White);

                //--벽 출력
                RenderObject(wallX, wallY, "#", ConsoleColor.DarkYellow);

                //---------입력---------
                ConsoleKey key = Console.ReadKey().Key;

                //---------처리---------
                MovePlayer(key, ref playerX, ref playerY, ref playerMoveDirection);
                
                // 플레이어와 바위가 충돌했을 때
                if (playerX == rockX && playerY == rockY)
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            {
                                rockX = Math.Clamp(rockX - 1, MIN_X, MAX_X);
                                playerX = rockX + 1;
                            }
                            break;
                        case Direction.Right:
                            {
                                rockX = Math.Clamp(rockX + 1, MIN_X, MAX_X);
                                playerX = rockX - 1;
                            }
                            break;
                        case Direction.Up:
                            {
                                rockY = Math.Clamp(rockY - 1, MIN_Y, MAX_Y);
                                playerY = rockY + 1;
                            }
                            break;
                        case Direction.Down:
                            {
                                rockY = Math.Clamp(rockY + 1, MIN_Y, MAX_Y);
                                playerY = rockY - 1;
                            }
                            break;
                        default:
                            {
                                ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
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
                                ++playerX;
                            }
                            break;
                        case Direction.Right:
                            {
                                --playerX;
                            }
                            break;
                        case Direction.Up:
                            {
                                ++playerY;
                            }
                            break;
                        case Direction.Down:
                            {
                                --playerY;
                            }
                            break;
                        default:
                            {
                                ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            return;
                    }
                }

                // 바위와 벽이 충돌했을 때
                if (false == IsCollided(rockX, wallX, rockY, wallY))
                {
                    continue;
                }
                else
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            {
                                MoveToRightOfTarget(out rockX, in wallX);
                                playerX = rockX + 1;
                            }
                            break;
                        case Direction.Right:
                            {
                                MoveToLeftOfTarget(out rockX, in wallX);
                                playerX = rockX - 1;
                            }
                            break;
                        case Direction.Up:
                            {
                                rockY = Math.Clamp(wallY + 1, MIN_Y, MAX_Y);
                                playerY = rockY + 1;
                            }
                            break;
                        case Direction.Down:
                            {
                                rockY = Math.Clamp(wallY - 1, MIN_Y, MAX_Y);
                                playerY = rockY - 1;
                            }
                            break;
                        default:
                            {
                                ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            return;
                    }
                }
            }

            // 오브젝트를 그린다.
            void RenderObject(int x, int y, string icon, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.SetCursorPosition(x, y);
                Console.Write(icon);
            }

            // 플레이어를 이동한다.
            void MovePlayer(ConsoleKey key, ref int x, ref int y, ref Direction moveDirection)
            {
                if (key == ConsoleKey.LeftArrow)
                {
                    x = Math.Clamp(x - 1, 0, 15);
                    moveDirection = Direction.Left;
                }
                if (key == ConsoleKey.RightArrow)
                {
                    x = Math.Clamp(x + 1, 0, 15);
                    moveDirection = Direction.Right;
                }
                if (key == ConsoleKey.UpArrow)
                {
                    y = Math.Clamp(y - 1, 0, 15);
                    moveDirection = Direction.Up;
                }
                if (key == ConsoleKey.DownArrow)
                {
                    y = Math.Clamp(y + 1, 0, 15);
                    moveDirection = Direction.Down;
                }
            }

            void ExitWithError(string errorMessage)
            {
                Console.Clear();
                Console.WriteLine(errorMessage);
                Environment.Exit(1);
            }

            bool IsCollided (int x1, int x2, int y1, int y2)
            {
                if (x1 == x2 && y1 == y2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            void MoveToLeftOfTarget(out int x, in int target) => x = Math.Max(MIN_X, target - 1);
            void MoveToRightOfTarget(out int x, in int target) => x = Math.Min(target + 1, MAX_X);
            void MoveToUpOfTarget(out int y, in int target) => y = Math.Max(MIN_Y, target - 1);
            void MoveToOfTarget(out int y, in int target) => y = Math.Min(target + 1, MAX_Y);
        }
    }
}