using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace meetMoltres
{
    static class Game
    {
        // 기호 상수 정의
        const int MIN_X = 0;
        const int MAX_X = 15;
        const int MIN_Y = 0;
        const int MAX_Y = 15;

        // 오브젝트를 그린다.
        public static void RenderObject(int x, int y, string icon, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.SetCursorPosition(x, y);
            Console.Write(icon);
        }

        // 플레이어를 이동한다.
        public static void MovePlayer(ConsoleKey key, Player player)
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
        public static void ExitWithError(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        // map 경로 설정
        public static string[] LoadMaps(int mapNumber)
        {
            // 경로를 구성한다.
            string mapFilePath = Path.Combine("..\\..\\..\\Assets", "Maps", $"Maps{mapNumber:D2}.txt");
            Console.WriteLine(mapFilePath);

            // 파일 존재 확인
            if (false == File.Exists(mapFilePath))
            {
                ExitWithError($"맵 파일이 없습니다. 맵 번호({mapNumber})");
            }
            // 파일의 내용을 불러온다.
            return File.ReadAllLines(mapFilePath);
        }

        // target이 있는 경우 이동 처리
        public static void MoveToLeftOfTarget(out int x, in int target) => x = Math.Max(MIN_X, target - 1);
        public static void MoveToRightOfTarget(out int x, in int target) => x = Math.Min(target + 1, MAX_X);
        public static void MoveToUpOfTarget(out int y, in int target) => y = Math.Max(MIN_Y, target - 1);
        public static void MoveToDownOfTarget(out int y, in int target) => y = Math.Min(target + 1, MAX_Y);

        

        public static void PushOut(Direction playerMoveDirection, ref int objX, ref int objY, int collidedObjX, int collidedObjY)
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

        public static void MoveRock(Player player, Rock rock)
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
        public static int CountRockOnGoal(Rock[] rocks, Trigger[] triggers)
        {
            int rockCount = rocks.Length;
            int triggerCount = triggers.Length;
            int result = 0;
            for (int rockId = 0; rockId < rockCount; ++rockId)
            {
                rocks[rockId].IsOnGoal = false;

                for (int triggerId = 0; triggerId < triggerCount; ++triggerId)
                {
                    if (CollisionHelper.IsCollided(rocks[rockId].X, triggers[triggerId].X, rocks[rockId].Y, triggers[triggerId].Y))
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
