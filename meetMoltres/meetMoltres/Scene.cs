using System.Diagnostics;
using System.Media;
using System.Numerics;

namespace meetMoltres
{
    public enum SceneKind
    {
        Title,
        InGame
    }
    public enum SoundKind
    {
        battle,
        boss,
        cave,
        opening,
        title
    }
    public static class Scene
    {
        private static SceneKind _currentScene = SceneKind.Title;
        public static SceneKind GetCurrentScene()
        {
            return _currentScene;
        }

        public static void InitTitle()
        {
            
            SoundPlayer sdplayer = new SoundPlayer(LoadSound(SoundKind.opening));
            int sdPlayTimeSecond = 12;
            SoundPlayer sdplayer2 = new SoundPlayer(LoadSound(SoundKind.title));
            sdplayer.Load();
            sdplayer2.Load();
            Stopwatch stopwatch = new Stopwatch();
            Console.ForegroundColor = ConsoleColor.Red;
            Scene.RenderTitle();
            Console.WriteLine("Please Wait...");
            Console.WriteLine(" ");
            Console.WriteLine("--------플레이 방법--------");
            Console.WriteLine("키보드 방향키를 입력하여 이동합니다.");
            Console.WriteLine("'O'바위를 이동시켜 'Θ'에 올려놓습니다");
            Console.WriteLine("'≡'사다리로 이동하면 재도전 / 다음 스테이지 / 이전 스테이지로 넘어갑니다");
            sdplayer.Play();
            stopwatch.Start();
            while (true)
            {
                if (stopwatch.Elapsed.Seconds >= sdPlayTimeSecond)
                {

                    sdplayer2.PlayLooping();
                    stopwatch.Reset();
                    break;
                }
            }
            Scene.RenderTitle();
            Console.WriteLine("press spacebar to start");
            Console.WriteLine("주의사항 : 실행은 전체화면으로, 창을 옮길 시 게임이 종료됩니다.");
        }
        private static string[] _image = null;
        public static void RenderTitle()
        {
            Console.SetCursorPosition(0, 0);
            _image = Game.LoadImage("title");
            for (int imageId = 0; imageId < _image.Length; ++imageId)
            {
                Console.WriteLine(_image[imageId]);
            }
            
        }
        public static void ReleaseTitle()
        {
            
        }
        public static void UpdateTitle()
        {
            if (Input.IsKeyDown(ConsoleKey.Spacebar))
            {
                SetNextScene(SceneKind.InGame);
            }

        }
        public static void InitInGame()
        {
            Console.Clear();
            SoundPlayer sdplayer3 = new SoundPlayer(LoadSound(SoundKind.cave));
            sdplayer3.Load();
            sdplayer3.PlayLooping();
        }
        public static void RenderInGame()
        {
            Map.Render();
        }
        public static void UpdateInGame()
        {
            if (Map.IsSetNextMap())
            {
                Map.ChangeMap();
            }
            Map.Update();

        }
        public static void ReleaseInGame()
        {

        }
        private static bool _isSceneChanged = true;
        public static bool IsSceneChange()
        {
            return _isSceneChanged;
        }

        public static SceneKind _nextScene;
        public static void SetNextScene(SceneKind nextScene)
        {
            _nextScene = nextScene;
            _isSceneChanged = true;
        }
        public static void ChangeScene()
        {
            if (_isSceneChanged)
            {
                _isSceneChanged = false;
                switch (_currentScene)
                {
                    case SceneKind.Title:
                        ReleaseTitle();
                        break;
                    case SceneKind.InGame
                        :
                        ReleaseInGame();
                        break;
                }
                _currentScene = _nextScene;
                _nextScene = default;

                switch (_currentScene)
                {
                    case SceneKind.Title:
                        InitTitle();
                        break;
                    case SceneKind.InGame:
                        InitInGame();
                        break;
                }
            }
        }
        public static string LoadSound(SoundKind sound)
        {
            // 경로를 구성한다.
            string soundFilePath = Path.Combine("..\\..\\..\\Assets", "Sounds", $"{sound.ToString()}Sound.wav");

            // 파일 존재 확인
            if (false == File.Exists(soundFilePath))
            {
                Game.ExitWithError($"사운드 파일이 없습니다. 사운드 이름({sound})");
            }
            return soundFilePath;

        }
    }
}
