using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;

namespace meetMoltres
{
    public enum SceneKind
    {
        Title,
        InGame
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

            SoundPlayer sdplayer = new SoundPlayer("openingSound.wav");
            int sdPlayTimeSecond = 12;
            SoundPlayer sdplayer2 = new SoundPlayer("titleSound.wav");
            sdplayer.Load();
            sdplayer2.Load();
            Stopwatch stopwatch = new Stopwatch();
            Console.ForegroundColor = ConsoleColor.Red;
            Scene.RenderTitle();            
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
            if (Input.IsKeyDown(ConsoleKey.A))
            {
                Console.Clear();
                Scene.RenderTitle();
            }
            if (Input.IsKeyDown(ConsoleKey.Spacebar))
            {
                SetNextScene(SceneKind.InGame);
            }

        }
        public static void InitInGame()
        {
            Console.Clear();
        }
        public static void RenderInGame()
        {
            //Map.Render();
        }
        public static void UpdateInGame()
        {
            //if (Map.IsSetNextMap())
            //{
            //    Map.ChangeMap();
            //}
            //Map.Update();

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
    }
}
