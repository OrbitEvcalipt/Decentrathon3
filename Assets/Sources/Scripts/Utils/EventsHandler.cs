using System;

namespace Sources.Scripts.Utils
{
    public class EventsHandler
    {
        public static event Action OnGameStart;
        public static void GameStart()
        {
            OnGameStart?.Invoke();
        }

        
        public static event Action OnGameWin;
        public static void GameWin()
        {
            OnGameWin?.Invoke();
        }

        public static event Action OnGameOver;

        public static void GameOver()
        {
            OnGameOver?.Invoke();
        }
    }
}