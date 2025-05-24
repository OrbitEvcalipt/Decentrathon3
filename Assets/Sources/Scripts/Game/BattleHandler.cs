using System.Linq;
using FunnyBlox.Game;
using Sources.Scripts.Common;
using Sources.Scripts.Data;
using Sources.Scripts.Utils;
using UnityEngine;
using Random = System.Random;

namespace Sources.Scripts.Game
{
    public class BattleHandler : MonoBehaviour
    {
        [SerializeField] LevelDataList levelDataList;

        [SerializeField] private EntityData playerData;
        [SerializeField] private EntityView playerView;

        [SerializeField] private EntityData enemyData;
        [SerializeField] private EntityView enemyView;

        [Space] [SerializeField] private int _currentLevel;
        [SerializeField] private int _currentSubLevel;
        [SerializeField] private int _counterStepsBattle;

        private void OnEnable()
        {
            EventsHandler.OnGameStart += InitializeBattle;
        }

        private void OnDisable()
        {
            EventsHandler.OnGameStart -= InitializeBattle;
        }

        /// <summary>
        /// Из меню запустили бой
        /// </summary>
        private void InitializeBattle()
        {
            CommonData.playerHealth = 3;

            Random rng = new Random();

            //Номер уровня
            _currentLevel = CommonData.levelNumber;
            //если больше количества уровней в списке
            if (_currentLevel >= levelDataList.levels.Length)
                _currentLevel = rng.Next(levelDataList.levels.Length);

            _currentSubLevel = 0;
            PrepareBattle();

            EventsHandler.AfterInitializeBattle(levelDataList.levels[CommonData.levelNumber].subLevels.Length);
        }

        /// <summary>
        /// Подготовка данных в подуровне
        /// </summary>
        private void PrepareBattle()
        {
            Random rng = new Random();

            playerData.lives = CommonData.playerHealth; //TODO: сюда прокидывать жизнь игрока из глобала
            playerData.actions = levelDataList.levels[_currentLevel]
                .subLevels[_currentSubLevel]
                .actions.OrderBy(action => rng.Next()).ToList();
            playerView.Prepare(playerData.lives, playerData.actions);

            enemyData.lives = levelDataList.levels[_currentLevel]
                .subLevels[_currentSubLevel]
                .enemyHealth;
            enemyData.actions = levelDataList.levels[_currentLevel]
                .subLevels[_currentSubLevel]
                .actions.OrderBy(action => rng.Next()).ToList();
            enemyView.Prepare(enemyData.lives, enemyData.actions);

            _counterStepsBattle = playerData.actions.Count;

            EventsHandler.NextSublevel(_currentSubLevel,playerData.lives, enemyData.lives);
        }

        /// <summary>
        /// Запуск активности с кнопки
        /// </summary>
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

        /// <summary>
        /// Считаем результат активности игрока и врага
        /// </summary>
        /// <param name="playerAction"></param>
        /// <param name="enemyAction"></param>
        private void CalculateBattleResult(ActionData playerAction, ActionData enemyAction)
        {
            Debug.Log($"Player: {playerAction.actionType}({playerAction.actionForce}) ----" +
                      $" Enemy: {enemyAction.actionType}({enemyAction.actionForce})");

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
                Invoke("CheckWinState", 0.1f);
            }
            else if (playerData.lives <= 0)
            {
                Debug.Log("Player lose");
                EventsHandler.GameOver();
            }
        }

        /// <summary>
        /// Расчёт победителя
        /// </summary>
        /// <param name="player"></param>
        /// <param name="enemy"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Проверка оставшегося количества активностей
        /// </summary>
        private void ContinueBattle()
        {
            _counterStepsBattle--;
            if (_counterStepsBattle <= 0)
            {
                // Если активности закончились, наполняем список вновь
                Debug.Log("Continue battle");

                Random rng = new Random();
                playerData.actions = playerData.actions.OrderBy(action => rng.Next()).ToList();
                playerView.Prepare(playerData.lives, playerData.actions);

                enemyData.actions = enemyData.actions.OrderBy(action => rng.Next()).ToList();
                enemyView.Prepare(enemyData.lives, enemyData.actions);

                _counterStepsBattle = playerData.actions.Count;
            }
        }

        private void CheckWinState()
        {
            //выдаём награду за текущий подуровень
            foreach (RewardedCurrency currency in levelDataList.levels[CommonData.levelNumber]
                         .subLevels[_currentSubLevel].RewardedCurrencies)
            {
                CurrencyManager.instance.UpdateAmount(
                    currency.name,
                    currency.amount);
            }

            //увеличиваем подуровень и сверяемся, что ещё есть подуровни
            _currentSubLevel++;
            if (_currentSubLevel < levelDataList.levels[CommonData.levelNumber].subLevels.Length)
            {
                PrepareBattle();
                return;
            }

            //выдаём награду за уровень
            foreach (RewardedCurrency currency in levelDataList.levels[CommonData.levelNumber].RewardedCurrencies)
            {
                CurrencyManager.instance.UpdateAmount(
                    currency.name,
                    currency.amount);
            }

            //победа, выходим в меню 
            Debug.Log("Player wins");
            EventsHandler.GameWin();
        }
    }
}