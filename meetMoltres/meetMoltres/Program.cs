using meetMoltres;
using System.Text;
using System;
using System.Runtime.CompilerServices;

namespace LegendaryMoltres
{
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

            Player player = new Player
            {
                X = 5,
                Y = 5,
                MoveDirection = Direction.None
            };

            Rock[] rocks = new Rock[2]
            {
                new Rock {X = 10, Y = 10, IsOnGoal = false},
                new Rock {X = 12, Y = 12, IsOnGoal = false}
            };
            int rockCount = rocks.Length;

            Wall[] walls = new Wall[2]
            {
                new Wall {X = 4, Y = 4},
                new Wall {X = 7, Y = 7}
            };

            int wallCount = walls.Length;


            Trigger[] triggers = new Trigger[2]
            {
                new Trigger {X = 9, Y = 9},
                new Trigger {X = 6, Y = 6}
            };
            int triggerCount = triggers.Length;

            // 기호 상수 정의
            const int MIN_X = 0;
            const int MAX_X = 15;
            const int MIN_Y = 0;
            const int MAX_Y = 15;

            // 여러 개의 바위 중 어떤 바위인지 구분하기 위한 인덱스
            int pushedRockIndex = 0;

            // 바위가 트리거 위에 올라와있는지 저장
            bool[] isRockOnGoal = new bool[triggerCount];
                       
            while (true)
            {
                //---------render---------
                Console.Clear();

                //--트리거 출력
                for (int triggerId = 0; triggerId < triggerCount; ++triggerId)
                {
                    RenderObject(triggers[triggerId].X, triggers[triggerId].Y, "@", ConsoleColor.Blue);
                }

                //--플레이어 출력
                RenderObject(player.X, player.Y, "¶", ConsoleColor.Red);

                //--바위 출력
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    RenderObject(rocks[rockId].X, rocks[rockId].Y, "O", ConsoleColor.White);
                }

                //--벽 출력
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    RenderObject(walls[wallId].X, walls[wallId].Y, "‡", ConsoleColor.DarkYellow);
                }

                //---------입력---------
                ConsoleKey key = Console.ReadKey().Key;

                //---------처리---------

                //플레이어 이동
                MovePlayer(key, player);

                // 플레이어와 벽이 충돌했을 때
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    if (false == IsCollided(player.X, walls[wallId].X, player.Y, walls[wallId].Y))
                    {
                        continue;
                    }
                    OnCollision(() =>
                    {
                        PushOut(player.MoveDirection, ref player.X, ref player.Y, walls[wallId].X, walls[wallId].Y);
                    });
                }

                // 플레이어와 바위가 충돌했을 때
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    if (false == IsCollided(player.X, rocks[rockId].X, player.Y, rocks[rockId].Y))
                    {
                        continue;
                    }
                    OnCollision(() =>
                    {
                        MoveRock(player, rocks[rockId]);
                    });

                    // 어떤 박스를 밀었는지 저장
                    pushedRockIndex = rockId;
                    break;
                }

                // 바위끼리 충돌 했을 때
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    if (pushedRockIndex == rockId)
                    {
                        continue;
                    }

                    if (false == IsCollided(rocks[pushedRockIndex].X, rocks[rockId].X, rocks[pushedRockIndex].Y, rocks[rockId].Y))
                    {
                        continue;
                    }

                    OnCollision(() =>
                    {
                        PushOut(player.MoveDirection, ref rocks[pushedRockIndex].X, ref rocks[pushedRockIndex].Y, rocks[rockId].X, rocks[rockId].Y);
                        PushOut(player.MoveDirection, ref player.X, ref player.Y, rocks[pushedRockIndex].X, rocks[pushedRockIndex].Y);
                    });
                }

                // 바위와 벽이 충돌했을 때

                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    if (false == IsCollided(rocks[pushedRockIndex].X, walls[wallId].X, rocks[pushedRockIndex].Y, walls[wallId].Y))
                    {
                        continue;

                    }
                    OnCollision(() =>
                    {
                        PushOut(player.MoveDirection, ref rocks[pushedRockIndex].X, ref rocks[pushedRockIndex].Y, walls[wallId].X, walls[wallId].Y);
                        PushOut(player.MoveDirection, ref player.X, ref player.Y, rocks[pushedRockIndex].X, rocks[pushedRockIndex].Y);
                    });
                    break;
                }




                // 바위가 트리거 위로 올라왔는지 확인
                int rockOnGoalCount = CountRockOnGoal(rocks, triggers);

                // 오브젝트를 그린다.
                void RenderObject(int x, int y, string icon, ConsoleColor color)
                {
                    Console.ForegroundColor = color;
                    Console.SetCursorPosition(x, y);
                    Console.Write(icon);
                }

                // 플레이어를 이동한다.
                void MovePlayer(ConsoleKey key, Player player)
                {
                    if (key == ConsoleKey.LeftArrow)
                    {
                        MoveToLeftOfTarget(out player.X, in player.X);
                        player.MoveDirection = Direction.Left;
                    }
                    if (key == ConsoleKey.RightArrow)
                    {
                        MoveToRightOfTarget(out player.X, in player.X);
                        player.MoveDirection = Direction.Right;
                    }
                    if (key == ConsoleKey.UpArrow)
                    {
                        MoveToUpOfTarget(out player.Y, in player.Y);
                        player.MoveDirection = Direction.Up;
                    }
                    if (key == ConsoleKey.DownArrow)
                    {
                        MoveToDownOfTarget(out player.Y, in player.Y);
                        player.MoveDirection = Direction.Down;
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
                void OnCollision(Action action)
                {
                    action();
                }

                void PushOut(Direction playerMoveDirection, ref int objX, ref int objY, int collidedObjX, int collidedObjY)
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

                void MoveRock(Player player, Rock rock)
                {
                    switch (player.MoveDirection)
                    {
                        case Direction.Left:
                            {
                                MoveToLeftOfTarget(out rock.X, in player.X);
                            }
                            break;
                        case Direction.Right:
                            {
                                MoveToRightOfTarget(out rock.X, in player.X);
                            }
                            break;
                        case Direction.Up:
                            {
                                MoveToUpOfTarget(out rock.Y, in player.Y);
                            }
                            break;
                        case Direction.Down:
                            {
                                MoveToDownOfTarget(out rock.Y, in player.Y);
                            }
                            break;
                        default:
                            {
                                ExitWithError($"[Error] 플레이어의 이동 방향이 잘못되었습니다.");
                            }
                            break;
                    }
                }
                int CountRockOnGoal(Rock[] rocks, Trigger[] triggers)
                {
                    int result = 0;
                    for (int rockId = 0; rockId < rockCount; ++rockId)
                    {
                        rocks[rockId].IsOnGoal = false;

                        for (int triggerId = 0; triggerId < triggerCount; ++triggerId)
                        {
                            if (IsCollided(rocks[rockId].X, triggers[triggerId].X, rocks[rockId].Y, triggers[triggerId].Y))
                            {
                                ++result;
                                rocks[rockId].IsOnGoal = true;
                                break;
                            }
                        }
                    }
                    return result;
                }
            }
        }
    }
}