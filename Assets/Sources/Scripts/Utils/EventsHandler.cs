using System;

namespace Sources.Scripts.Utils
{
    public class EventsHandler
    {
        public static event Action OnInitialize;
        public static void Initialize()
        {
            OnInitialize?.Invoke();
        }
            
        public static event Action OnGameStart;
        public static void GameStart()
        {
            OnGameStart?.Invoke();
        }
        
        public static event Action<int> OnAfterInitializeBattle;
        public static void AfterInitializeBattle(int index)
        {
            OnAfterInitializeBattle?.Invoke(index);
        }
        
        public static event Action<int> OnNextSublevel;
        public static void NextSublevel(int index)
        {
            OnNextSublevel?.Invoke(index);
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
        
        
        public static event Action OnClaimBattleResult;
        public static void ClaimBattleResult()
        {
            OnClaimBattleResult?.Invoke();
        }
    }
}