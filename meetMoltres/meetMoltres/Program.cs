﻿using meetMoltres;

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

            int[] rockX = { 10, 12 };
            int[] rockY = { 10, 12 };
            int rockCount = rockX.Length;

            int wallX = 7;
            int wallY = 7;

            // 기호 상수 정의
            const int MIN_X = 0;
            const int MAX_X = 15;
            const int MIN_Y = 0;
            const int MAX_Y = 15;

            // 박스가 여러 개일 경우 어떤 박스인지 구분하기 위한 인덱스
            int pushedRockIndex = 0;

            // 플레이어의 이동방향
            Direction playerMoveDirection = Direction.None;

            while (true)
            {
                //---------render---------
                Console.Clear();

                //--플레이어 출력
                RenderObject(playerX, playerY, "¶", ConsoleColor.Red);

                //--바위 출력
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    RenderObject(rockX[rockId], rockY[rockId], "O", ConsoleColor.White);
                }

                //--벽 출력
                RenderObject(wallX, wallY, "‡", ConsoleColor.DarkYellow);

                //---------입력---------
                ConsoleKey key = Console.ReadKey().Key;

                //---------처리---------

                //플레이어 이동
                MovePlayer(key, ref playerX, ref playerY, ref playerMoveDirection);

                // 플레이어와 벽이 충돌했을 때
                if (IsCollided(playerX, wallX, playerY, wallY))
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            {
                                MoveToRightOfTarget(out playerX, in wallX);
                            }
                            break;
                        case Direction.Right:
                            {
                                MoveToLeftOfTarget(out playerX, in wallX);
                            }
                            break;
                        case Direction.Up:
                            {
                                MoveToDownOfTarget(out playerY, in wallY);
                            }
                            break;
                        case Direction.Down:
                            {
                                MoveToUpOfTarget(out playerY, in wallY);
                            }
                            break;
                        default:
                            {
                                ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            return;
                    }
                }


                // 플레이어와 바위가 충돌했을 때
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    if (IsCollided(playerX, rockX[rockId], playerY, rockY[rockId]))
                    {
                        switch (playerMoveDirection)
                        {
                            case Direction.Left:
                                {
                                    MoveToLeftOfTarget(out rockX[rockId], in playerX);
                                    playerX = rockX[rockId] + 1;
                                }
                                break;
                            case Direction.Right:
                                {
                                    MoveToRightOfTarget(out rockX[rockId], in playerX);
                                    playerX = rockX[rockId] - 1;
                                }
                                break;
                            case Direction.Up:
                                {
                                    MoveToUpOfTarget(out rockY[rockId], in playerY);
                                    playerY = rockY[rockId] + 1;
                                }
                                break;
                            case Direction.Down:
                                {
                                    MoveToDownOfTarget(out rockY[rockId], in playerY);
                                    playerY = rockY[rockId] - 1;
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

                // 바위와 벽이 충돌했을 때
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    if (false == IsCollided(rockX[rockId], wallX, rockY[rockId], wallY))
                    {
                        continue;
                        //switch (playerMoveDirection)
                        //{
                        //    case Direction.Left:
                        //        {
                        //            MoveToRightOfTarget(out rockX[rockId], in wallX);
                        //            playerX = rockX[rockId] + 1;
                        //        }
                        //        break;
                        //    case Direction.Right:
                        //        {
                        //            MoveToLeftOfTarget(out rockX[rockId], in wallX);
                        //            playerX = rockX[rockId] - 1;
                        //        }
                        //        break;
                        //    case Direction.Up:
                        //        {
                        //            MoveToDownOfTarget(out rockY[rockId], in wallY);
                        //            playerY = rockY[rockId] + 1;
                        //        }
                        //        break;
                        //    case Direction.Down:
                        //        {
                        //            MoveToUpOfTarget(out rockY[rockId], in wallY);
                        //            playerY = rockY[rockId] - 1;
                        //        }
                        //        break;
                        //    default:
                        //        {
                        //            ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                        //        }
                        //        return;
                        //}
                    }
                    OnCollision(playerMoveDirection, ref rockX[pushedRockIndex], ref rockY[pushedRockIndex], wallX, wallY);
                    OnCollision(playerMoveDirection, ref playerX, ref playerY, rockX[pushedRockIndex], rockY[pushedRockIndex]);
                }

                // 바위끼리 충돌 했을 때
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    if (pushedRockIndex == rockId)
                    {
                        continue;
                    }

                    if (false == IsCollided(rockX[pushedRockIndex], rockX[rockId], rockY[pushedRockIndex], rockY[rockId]))
                    {
                        continue;
                    }

                    OnCollision(playerMoveDirection, ref rockX[pushedRockIndex], ref rockY[pushedRockIndex], in rockX[rockId], in rockY[rockId]);
                    OnCollision(playerMoveDirection, ref playerX, ref playerY, in rockX[pushedRockIndex], in rockY[pushedRockIndex]);
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

                // 비정상 동작 시 에러 메시지 출력 후 종료
                void ExitWithError(string errorMessage)
                {
                    Console.Clear();
                    Console.WriteLine(errorMessage);
                    Environment.Exit(1);
                }

                // 충돌 판정
                bool IsCollided(int x1, int x2, int y1, int y2)
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

                // target이 있는 경우 이동 처리
                void MoveToLeftOfTarget(out int x, in int target) => x = Math.Max(MIN_X, target - 1);
                void MoveToRightOfTarget(out int x, in int target) => x = Math.Min(target + 1, MAX_X);
                void MoveToUpOfTarget(out int y, in int target) => y = Math.Max(MIN_Y, target - 1);
                void MoveToDownOfTarget(out int y, in int target) => y = Math.Min(target + 1, MAX_Y);

                // 충돌 처리

                void OnCollision(Direction playerMoveDirection, ref int objX, ref int objY, in int collidedObjX, in int collidedObjY)
                {
                    switch (playerMoveDirection)
                    {
                        case Direction.Left:
                            MoveToRightOfTarget(out objX, in collidedObjX);
                            break;
                        case Direction.Right:
                            MoveToLeftOfTarget(out objX, in collidedObjX);
                            break;
                        case Direction.Up:
                            MoveToDownOfTarget(out objY, in collidedObjY);
                            break;
                        case Direction.Down:
                            MoveToUpOfTarget(out objY, in collidedObjY);
                            break;
                        default:
                            ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            return;
                    }
                }
            }
        }
    }
}