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


        //---------------BATTLE PHASE---------------
        public static event Action<int> OnBattlePhase_AfterInitialize;

        public static void BattlePhase_AfterInitialize(int index)
        {
            OnBattlePhase_AfterInitialize?.Invoke(index);
        }

        public static event Action OnBattlePhase_Attack;

        public static void BattlePhase_Attack()
        {
            OnBattlePhase_Attack?.Invoke();
        }

        public static event Action<int, int, int> OnBattlePhase_NextSublevel;

        public static void BattlePhase_NextSublevel(int index, int playerLives, int enemyLives)
        {
            OnBattlePhase_NextSublevel?.Invoke(index, playerLives, enemyLives);
        }
    }
}