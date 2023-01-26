using meetMoltres;
using System.Media;
using System.Security.Cryptography.X509Certificates;

namespace LegendaryMoltres
{
    static class Sokoban
    {
        static void Main()
        {
            Game.Init();

            // 맵 파일 불러오기
            string[] lines = Game.LoadMaps(01);
            for (int i = 0; i < lines.Length; ++i)
            {
                Console.WriteLine(lines[i]);
            }

            //--사운드 출력
            SoundPlayer sdplayer = new SoundPlayer("caveSound.wav");
            sdplayer.Load();
            sdplayer.PlayLooping();

            // 맵 파일 파싱하여 초기 데이터 구성
            Player player;
            Rock[] rocks;
            Wall[] walls;
            Trigger[] triggers;
            DisappearingWall[] disappearingWalls;
            Trainer[] trainers;
            Ladder[] ladders;
            Game.ParseMaps(lines, out player, out rocks, out walls, out triggers, out disappearingWalls, out trainers, out ladders);

            int rockCount = rocks.Length;
            int wallCount = walls.Length;
            int triggerCount = triggers.Length;
            int disappearingWallCount = disappearingWalls.Length;
            int trainerCount = trainers.Length;
            int ladderCount = ladders.Length;

            // 여러 개의 바위 중 어떤 바위인지 구분하기 위한 인덱스
            int pushedRockIndex = 0;

            // 바위가 트리거 위에 올라와있는지 저장
            bool[] isRockOnTrigger = new bool[rockCount];
                        
            while (true)
            {
                //---------render---------
                
                // 바위가 트리거 위로 올라왔는지 확인
                int rockOnTriggerCount = Game.CountRockOnTrigger(rocks, triggers);

                //--트리거 출력
                for (int triggerId = 0; triggerId < triggerCount; ++triggerId)
                {
                    Game.RenderObject(triggers[triggerId].X, triggers[triggerId].Y, "Θ", ConsoleColor.Blue);
                }


                
                //--바위 출력
                for (int rockId = 0; rockId < rockCount; ++rockId)
                {
                    ConsoleColor rockColor = rocks[rockId].IsOnGoal ? ConsoleColor.DarkRed : ConsoleColor.White;
                    Game.RenderObject(rocks[rockId].X, rocks[rockId].Y, "O", rockColor);
                }

                //--벽 출력
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    Game.RenderObject(walls[wallId].X, walls[wallId].Y, "▒", ConsoleColor.DarkYellow);
                }
                
                //--사라지는 벽 출력
                for (int disappearingWallId = 0; disappearingWallId < disappearingWallCount; ++disappearingWallId)
                {
                    
                    if (rockOnTriggerCount == triggerCount)
                    {
                        Game.RenderObject(disappearingWalls[disappearingWallId].X, disappearingWalls[disappearingWallId].Y, " ", ConsoleColor.DarkBlue);
                        break;
                    }
                    Game.RenderObject(disappearingWalls[disappearingWallId].X, disappearingWalls[disappearingWallId].Y, "§", ConsoleColor.DarkBlue);
                }

                //--사다리 출력
                for (int ladderId = 0; ladderId < ladderCount; ++ladderId)
                {
                    Game.RenderObject(ladders[ladderId].X, ladders[ladderId].Y, "≡", ConsoleColor.DarkGray);
                }

                //--플레이어 출력
                Game.RenderObject(player.ex_X, player.ex_Y, " ", ConsoleColor.Black);
                Game.RenderObject(player.X, player.Y, "¶", ConsoleColor.Red);

                //--트레이너 출력
                for (int trainerId = 0; trainerId < trainerCount; ++trainerId)
                {
                    Game.RenderObject(trainers[trainerId].X, trainers[trainerId].Y, "T", ConsoleColor.Black);
                }

                
                
                //---------입력---------
                ConsoleKey key = Console.ReadKey().Key;

                //---------처리---------

                //플레이어 이동
                player.ex_X = player.X;
                player.ex_Y = player.Y;
                Movement.MovePlayer(key, player);
                
                // 플레이어와 벽이 충돌했을 때
                for (int wallId = 0; wallId < wallCount; ++wallId)
                {
                    if (false == CollisionHelper.IsCollided(player.X, walls[wallId].X, player.Y, walls[wallId].Y))
                    {
                        continue;
                    }
                    CollisionHelper.OnCollision(() =>
                    {
                        Movement.PushOut(player.MoveDirection, ref player.X, ref player.Y, walls[wallId].X, walls[wallId].Y);
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
                        Movement.MoveRock(player, rocks[rockId]);
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
                        Movement.PushOut(player.MoveDirection, ref rocks[pushedRockIndex].X, ref rocks[pushedRockIndex].Y, rocks[rockId].X, rocks[rockId].Y);
                        Movement.PushOut(player.MoveDirection, ref player.X, ref player.Y, rocks[pushedRockIndex].X, rocks[pushedRockIndex].Y);
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
                        Movement.PushOut(player.MoveDirection, ref rocks[pushedRockIndex].X, ref rocks[pushedRockIndex].Y, walls[wallId].X, walls[wallId].Y);
                        Movement.PushOut(player.MoveDirection, ref player.X, ref player.Y, rocks[pushedRockIndex].X, rocks[pushedRockIndex].Y);
                    });
                    break;
                }

                // 플레이어와 사라지는 벽 충돌

                for (int disappearingWallId = 0; disappearingWallId < disappearingWallCount; ++disappearingWallId) 
                {
                    if (rockOnTriggerCount == triggerCount)
                    {
                        break;
                    }
                    if (false == CollisionHelper.IsCollided(player.X, disappearingWalls[disappearingWallId].X, player.Y, disappearingWalls[disappearingWallId].Y ))
                    {  
                        continue; 
                    }
                    CollisionHelper.OnCollision(() =>
                    {
                        Movement.PushOut(player.MoveDirection, ref player.X, ref player.Y, disappearingWalls[disappearingWallId].X, disappearingWalls[disappearingWallId].Y);
                    });
                    break;
                }

                // 플레이어와 트레이너 충돌
                for (int trainerId = 0; trainerId < trainerCount; ++trainerId)
                {
                    if (false == CollisionHelper.IsCollided(player.X, trainers[trainerId].X, player.Y, trainers[trainerId].Y))
                    {
                        continue;
                    }
                    CollisionHelper.OnCollision(() =>
                    {
                        Movement.PushOut(player.MoveDirection, ref player.X, ref player.Y, trainers[trainerId].X, trainers[trainerId].Y);
                    });
                    break;
                }
                
                // 플레이어와 사다리 상호작용
                for (int ladderId = 0; ladderId < ladderCount; ++ladderId)
                {
                    ladders[ladderId].Name = (LadderName)ladderId;                     
                    if (false == CollisionHelper.IsCollided(player.X, ladders[ladderId].X, player.Y, ladders[ladderId].Y))
                    {
                        continue;
                    }
                    
                }
            }
        }
    }
}