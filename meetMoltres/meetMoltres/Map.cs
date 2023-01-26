using System.Diagnostics;

namespace meetMoltres
{
    public enum MapKind
    {
        Map01,
        Map02
    }
    public static class Map
    {
        private static MapKind _currentMap = MapKind.Map01;
        private static string[] lines = LoadMap(_currentMap);
        public static void ChangeMap()
        {
            if (_isSetNextMap)
            {
                _isSetNextMap = false;
                _currentMap = _nextMap;
                _nextMap = default;

                LoadMap(_currentMap);

            }
        }
        private static MapKind _nextMap;
        public static void SetNextMap(MapKind nextMap)
        {
            _nextMap = nextMap;
            _isSetNextMap = true;

        }
        static Player player;
        static Rock[] rocks;
        static Wall[] walls;
        static Trigger[] triggers;
        static DisappearingWall[] disappearingWalls;
        static Trainer[] trainers;
        static Ladder[] ladders;

        static int rockCount = rocks.Length;
        static int wallCount = walls.Length;
        static int triggerCount = triggers.Length;
        static int disappearingWallCount = disappearingWalls.Length;
        static int trainerCount = trainers.Length;
        static int ladderCount = ladders.Length;

        // map 경로 설정
        private static string[] LoadMap(MapKind map)
        {
            // 경로를 구성한다.
            string mapFilePath = Path.Combine("..\\..\\..\\Assets", "Maps", $"{map.ToString()}.txt");

            // 파일 존재 확인
            if (false == File.Exists(mapFilePath))
            {
                Game.ExitWithError($"맵 파일이 없습니다. 맵 이름({map})");
            }
            string[] result = File.ReadAllLines(mapFilePath);
            // 파일의 내용을 불러온다.
            ParseMap(result, out player, out rocks, out walls, out triggers, out disappearingWalls, out trainers, out ladders);
            return result;

        }

        private static void ParseMap(string[] map, out Player player, out Rock[] rocks, out Wall[] walls, out Trigger[] triggers, out DisappearingWall[] disappearingWalls, out Trainer[] trainers, out Ladder[] ladders)
        {

            Debug.Assert(map != null);
            String[] mapMetaData = map[map.Length - 1].Split(" ");
            player = null;
            rocks = new Rock[int.Parse(mapMetaData[0])];
            walls = new Wall[int.Parse(mapMetaData[1])];
            triggers = new Trigger[int.Parse(mapMetaData[2])];
            disappearingWalls = new DisappearingWall[int.Parse(mapMetaData[3])];
            trainers = new Trainer[int.Parse(mapMetaData[4])];
            ladders = new Ladder[int.Parse(mapMetaData[5])];

            int rockIndex = 0;
            int wallIndex = 0;
            int triggerIndex = 0;
            int disappearingWallIndex = 0;
            int trainerIndex = 0;
            int ladderIndex = 0;   

            for (int y = 0; y < map.Length - 1; ++y)
            {
                for (int x = 0; x < map[y].Length; ++x)
                {
                    switch (map[y][x])
                    {
                        case ObjectSymbol.Player:
                            player = new Player { X = x, Y = y };
                            break;

                        case ObjectSymbol.Rock:
                            rocks[rockIndex] = new Rock { X = x, Y = y };
                            ++rockIndex;
                            break;
                        case ObjectSymbol.Wall:
                            walls[wallIndex] = new Wall { X = x, Y = y };
                            ++wallIndex;
                            break;
                        case ObjectSymbol.Trigger:
                            triggers[triggerIndex] = new Trigger { X = x, Y = y };
                            ++triggerIndex;
                            break;
                        case '§':
                            disappearingWalls[disappearingWallIndex] = new DisappearingWall { X = x, Y = y };
                            ++disappearingWallIndex;
                            break;
                        case 'T':
                            trainers[trainerIndex] = new Trainer { X = x, Y = y };
                            ++trainerIndex;
                            break;
                        case 'I':
                            break;
                        case '≡':
                            ladders[ladderIndex] = new Ladder { X = x, Y = y };
                            ++ladderIndex;
                            break;
                        case ' ':
                            break;
                        default:
                            Game.ExitWithError("맵 파일이 잘못되었습니다.");
                            break;

                    }
                }
            }

        }

        public static bool _isSetNextMap = true;
        public static bool IsSetNextMap()
        {
            return _isSetNextMap;
        }
        public static void Update()
        {
            switch (_currentMap)
            {
                case MapKind.Map01:

                    break;
                case MapKind.Map02:
                    break;
            }

        }
        public static void Render()
        {
            switch (_currentMap)
            {
                case MapKind.Map01:
                    RenderMap01();
                    UpdateMap01();
                    break;
                case MapKind.Map02:
                    RenderMap02();
                    UpdateMap02();
                    break;
            }
        }
        private static void RenderMap01()
        {
            //바위가 트리거 위로 올라왔는지 확인
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
        }

        private static void RenderMap02()
        {
            Console.WriteLine("Map02");
        }
        private static void UpdateMap01()
        {
            // 여러 개의 바위 중 어떤 바위인지 구분하기 위한 인덱스
            int pushedRockIndex = 0;
            //// 바위가 트리거 위에 올라와있는지 저장
            bool[] isRockOnTrigger = new bool[rockCount];
            int rockOnTriggerCount = Game.CountRockOnTrigger(rocks, triggers);

            if (Input.IsKeyDown(ConsoleKey.Y))
            {
                SetNextMap(MapKind.Map02);
            }

            ////플레이어 이동
            player.ex_X = player.X;
            player.ex_Y = player.Y;
            Movement.MovePlayer(Input._key, player);
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
            //바위끼리 충돌 했을 때
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
                if (false == CollisionHelper.IsCollided(player.X, disappearingWalls[disappearingWallId].X, player.Y, disappearingWalls[disappearingWallId].Y))
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

        }
 

        private static void UpdateMap02()
        {
            if (Input.IsKeyDown(ConsoleKey.T))
            {
                Scene.SetNextScene(SceneKind.Title);
            }
        }
    }
}
