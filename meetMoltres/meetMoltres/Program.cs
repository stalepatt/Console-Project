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
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Clear();

            // 스테이지 파일 불러오기
            string[] lines = Game.LoadMaps(01);
            for (int i = 0; i < lines.Length; ++i)
            {
                Console.WriteLine(lines[i]);
            }
            Player player = new Player();

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
                    Game.RenderObject(triggers[triggerId].X, triggers[triggerId].Y, "@", ConsoleColor.Blue);
                }

                //--플레이어 출력
                Game.RenderObject(player.X, player.Y, "¶", ConsoleColor.Red);

                //--바위 출력
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    Game.RenderObject(rocks[rockId].X, rocks[rockId].Y, "O", ConsoleColor.White);
                }

                //--벽 출력
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    Game.RenderObject(walls[wallId].X, walls[wallId].Y, "‡", ConsoleColor.DarkYellow);
                }

                //---------입력---------
                ConsoleKey key = Console.ReadKey().Key;

                //---------처리---------

                //플레이어 이동
                Game.MovePlayer(key, player);

                // 플레이어와 벽이 충돌했을 때
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    if (false == CollisionHelper.IsCollided(player.X, walls[wallId].X, player.Y, walls[wallId].Y))
                    {
                        continue;
                    }
                    CollisionHelper.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref player.X, ref player.Y, walls[wallId].X, walls[wallId].Y);
                    });
                }

                // 플레이어와 바위가 충돌했을 때
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    if (false == CollisionHelper.IsCollided(player.X, rocks[rockId].X, player.Y, rocks[rockId].Y))
                    {
                        continue;
                    }
                    CollisionHelper.OnCollision(() =>
                    {
                        Game.MoveRock(player, rocks[rockId]);
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

                    if (false == CollisionHelper.IsCollided(rocks[pushedRockIndex].X, rocks[rockId].X, rocks[pushedRockIndex].Y, rocks[rockId].Y))
                    {
                        continue;
                    }

                    CollisionHelper.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref rocks[pushedRockIndex].X, ref rocks[pushedRockIndex].Y, rocks[rockId].X, rocks[rockId].Y);
                        Game.PushOut(player.MoveDirection, ref player.X, ref player.Y, rocks[pushedRockIndex].X, rocks[pushedRockIndex].Y);
                    });
                }

                // 바위와 벽이 충돌했을 때

                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    if (false == CollisionHelper.IsCollided(rocks[pushedRockIndex].X, walls[wallId].X, rocks[pushedRockIndex].Y, walls[wallId].Y))
                    {
                        continue;

                    }
                    CollisionHelper.OnCollision(() =>
                    {
                        Game.PushOut(player.MoveDirection, ref rocks[pushedRockIndex].X, ref rocks[pushedRockIndex].Y, walls[wallId].X, walls[wallId].Y);
                        Game.PushOut(player.MoveDirection, ref player.X, ref player.Y, rocks[pushedRockIndex].X, rocks[pushedRockIndex].Y);
                    });
                    break;
                }

                // 바위가 트리거 위로 올라왔는지 확인
                int rockOnGoalCount = Game.CountRockOnGoal(rocks, triggers);               
                
            }            
        }
    }
}