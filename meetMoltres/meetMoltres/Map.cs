using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ParseMap(lines, out Player player, out Rock[] rocks, out Wall[] walls, out Trigger[] triggers, out DisappearingWall[] disappearingWalls, out Trainer[] trainers);
            }
        }
        private static MapKind _nextMap;
        public static void SetNextMap(MapKind nextMap)
        {
            _nextMap = nextMap;
            _isSetNextMap = true;
        }
        // map 경로 설정
        private static string[] LoadMap(MapKind map)
        {
            // 경로를 구성한다.
            string mapFilePath = Path.Combine("..\\..\\..\\Assets", $"{map}.txt");

            // 파일 존재 확인
            if (false == File.Exists(mapFilePath))
            {
                Game.ExitWithError($"맵 파일이 없습니다. 맵 이름({map})");
            }
            // 파일의 내용을 불러온다.
            return File.ReadAllLines(mapFilePath);

        }

        private static void ParseMap(string[] map, out Player player, out Rock[] rocks, out Wall[] walls, out Trigger[] triggers, out DisappearingWall[] disappearingWalls, out Trainer[] trainers)
        {

            Debug.Assert(map != null);
            String[] mapMetaData = map[map.Length - 1].Split(" ");
            player = null;
            rocks = new Rock[int.Parse(mapMetaData[0])];
            walls = new Wall[int.Parse(mapMetaData[1])];
            triggers = new Trigger[int.Parse(mapMetaData[2])];
            disappearingWalls = new DisappearingWall[int.Parse(mapMetaData[3])];
            trainers = new Trainer[int.Parse(mapMetaData[4])];

            int rockIndex = 0;
            int wallIndex = 0;
            int triggerIndex = 0;
            int disappearingWallIndex = 0;
            int trainerIndex = 0;

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
                        case '╫':
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

        public static bool _isSetNextMap = false;
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
            string[] lines = LoadMap(0);
            //for (int i = 0; i < lines.Length; ++i)
            //{
            //    Console.WriteLine(lines[i]);
            //}

        }

        private static void RenderMap02()
        {
            Console.WriteLine("Map02");
        }
        private static void UpdateMap01()
        {
            if (Input.IsKeyDown(ConsoleKey.Y))
            {
                SetNextMap(MapKind.Map02);
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
