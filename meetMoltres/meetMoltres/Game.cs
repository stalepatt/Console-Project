using System.Diagnostics;

namespace meetMoltres
{
    static class Game
    {
        public static void Init()
        {
            // Console Initial Settings
            Console.SetWindowSize(500, 500);
            Console.ResetColor();
            Console.CursorVisible = false;
            Console.Title = "LegendaryMoltres";
            Console.BackgroundColor = ConsoleColor.Gray;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Clear();
            
        }

        public static void Run()
        {
            while (true)
            {
                if (Scene.IsSceneChange())
                {
                    Scene.ChangeScene();
                }
                Render();
                ProcessInput();
                Upadate();
            }
        }
        private static void Render()
        {

            switch (Scene.GetCurrentScene())
            {
                case SceneKind.Title:
                    Scene.RenderTitle();
                    break;
                case SceneKind.InGame:
                    Scene.RenderInGame(); 
                    break;

            }
        }
        private static void Upadate()
        {
            switch (Scene.GetCurrentScene())
            {
                case SceneKind.Title:
                    Scene.UpdateTitle(); break;
                case SceneKind.InGame:
                    Scene.UpdateInGame(); break;

            }
        }
        private static void ProcessInput()
        {
            Input.Process();
        }

        // 기호 상수 정의
        public const int MIN_X = 0;
        public const int MAX_X = 20;
        public const int MIN_Y = 0;
        public const int MAX_Y = 20;

        // 오브젝트를 그린다.
        public static void RenderObject(int x, int y, string icon, ConsoleColor color)
        {
            
            Console.SetCursorPosition(x, y);
            Console.ForegroundColor = color;
            Console.Write(icon);
        }
               
        // 비정상 동작 시 에러 메시지 출력 후 종료
        public static void ExitWithError(string errorMessage)
        {
            Console.Clear();
            Console.WriteLine(errorMessage);
            Environment.Exit(1);
        }

        public static string[] LoadImage(string nameImage)
        {
            // 경로를 구성한다.
            string ImageFilePath = Path.Combine("..\\..\\..\\Assets", "Maps", $"{nameImage}Image.txt");
            // 파일 존재 확인
            if (false == File.Exists(ImageFilePath))
            {
                ExitWithError($"맵 파일이 없습니다. 이미지 이름({ImageFilePath})");
            }
            // 파일의 내용을 불러온다.
            return File.ReadAllLines(ImageFilePath);
        }
        public static string[] Image = null;

        // map 경로 설정
        public static string[] LoadMaps(int mapNumber)
        {
            // 경로를 구성한다.
            string mapFilePath = Path.Combine("..\\..\\..\\Assets", "Maps", $"Maps{mapNumber:D2}.txt");

            // 파일 존재 확인
            if (false == File.Exists(mapFilePath))
            {
                ExitWithError($"맵 파일이 없습니다. 맵 번호({mapNumber})");
            }
            // 파일의 내용을 불러온다.
            return File.ReadAllLines(mapFilePath);
        }

        public static void ParseMaps(string[] map, out Player player, out Rock[] rocks, out Wall[] walls, out Trigger[] triggers, out DisappearingWall[] disappearingWalls, out Trainer[] trainers, out Ladder[] ladders)
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
                        case ObjectSymbol.DisappearingWall:
                            disappearingWalls[disappearingWallIndex] = new DisappearingWall { X = x, Y = y };
                            ++disappearingWallIndex;
                            break;                       
                        case ObjectSymbol.Trainer:
                            trainers[trainerIndex] = new Trainer { X = x, Y = y };
                            ++trainerIndex;
                            break;
                        case 'I':
                            break;
                        case ObjectSymbol.Ladder:
                            ladders[ladderIndex] = new Ladder { X = x, Y = y };
                            ++ladderIndex;
                            break;                            
                        case ' ':
                            break;
                        default:
                            ExitWithError("맵 파일이 잘못되었습니다.");
                            break;

                    }
                }
            }
        }
                
        public static int CountRockOnTrigger(Rock[] rocks, Trigger[] triggers)
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
