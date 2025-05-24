using System;
using System.Linq;
using FunnyBlox;
using Sources.Scripts.Common;
using Sources.Scripts.Data;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class BattleHandler : MonoBehaviour
    {
        [SerializeField] LevelDataList levelDataList;

        [SerializeField] private EntityData playerData;
        [SerializeField] private EntityView playerView;

        [SerializeField] private EntityData enemyData;
        [SerializeField] private EntityView enemyView;

        private int counterStepsBattle;

        private void OnEnable()
        {
            EventsHandler.OnGameStart += PrepareBattle;
        }

        private void OnDisable()
        {
            EventsHandler.OnGameStart -= PrepareBattle;
        }

        public void PrepareBattle()
        {
            playerData.lives = levelDataList.levels[0].enemyHealth;//TODO: сюда прокидывать жизнь игрока из глобала
            playerData.actions = levelDataList.levels[0].actions.ToList();
            playerView.Prepare(playerData.lives, playerData.actions);

            enemyData.lives = levelDataList.levels[0].enemyHealth;
            enemyData.actions = levelDataList.levels[0].actions.ToList();
            enemyView.Prepare(enemyData.lives, enemyData.actions);

            counterStepsBattle = playerData.actions.Count;
        }

        public void StartBattle()
        {
            int playerIndexAction = playerView.GetIndexInOn();
            if (playerIndexAction == -1) return;
            ActionData playerAction = playerData.actions[playerIndexAction];

            int enemyIndexAction = enemyView.GetRandomIndexInActive();
            ActionData enemyAction = enemyData.actions[enemyIndexAction];

            CalculateBattleResult(playerAction, enemyAction);

            playerView.UpdateButtons(playerIndexAction);
            enemyView.UpdateButtons(enemyIndexAction);

            ContinueBattle();
        }

        private void CalculateBattleResult(ActionData playerAction, ActionData enemyAction)
        {
            EBattleResult battleResult = CompareActionType(playerAction.actionType, enemyAction.actionType);
            switch (battleResult)
            {
                case EBattleResult.Draw:
                    int playerTakeDamage = playerAction.actionForce - enemyAction.actionForce;
                    playerData.lives -= playerTakeDamage;

                    int enemyTakeDamage = enemyAction.actionForce - playerAction.actionForce;
                    enemyData.lives -= enemyTakeDamage;
                    Debug.Log($"Player take damage {playerTakeDamage} and enemy take damage {enemyTakeDamage}");

                    break;
                case EBattleResult.PlayerWins:
                    enemyData.lives -= playerAction.actionForce;
                    Debug.Log($"Enemy take damage {playerAction.actionForce}");
                    break;
                case EBattleResult.EnemyWins:
                    playerData.lives -= enemyAction.actionForce;
                    Debug.Log($"Player take damage {enemyAction.actionForce}");
                    break;
            }

            playerView.UpdateLivesText(playerData.lives);
            enemyView.UpdateLivesText(enemyData.lives);

            if (enemyData.lives <= 0)
            {
                Debug.Log("Player wins");
                EventsHandler.GameWin();
            }
            else if (playerData.lives <= 0)
            {
                Debug.Log("Player lose");
                EventsHandler.GameOver();
            }
        }

        private EBattleResult CompareActionType(EActionType player, EActionType enemy)
        {
            if (player == enemy)
                return EBattleResult.Draw;

            // Победные комбинации: (player1 - player2 + 3) % 3 == 1
            // Примеры:
            // Stone (0) beats Scissors (1): (0 - 1 + 3) % 3 == 2 % 3 == 2 → PlayerWins
            // Scissors (1) beats Paper (2): (1 - 2 + 3) % 3 == 2 → PlayerWins
            // Paper (2) beats Stone (0): (2 - 0 + 3) % 3 == 5 % 3 == 2 → PlayerWins

            int result = ((int)player - (int)enemy + 3) % 3;

            return result == 1 ? EBattleResult.EnemyWins : EBattleResult.PlayerWins;
        }

        private void ContinueBattle()
        {
            counterStepsBattle--;
            if (counterStepsBattle <= 0)
            {
                Debug.Log("Continue battle");
                playerView.Prepare(playerData.lives, playerData.actions);
                enemyView.Prepare(enemyData.lives, enemyData.actions);
                counterStepsBattle = playerData.actions.Count;
            }
        }
    }
}