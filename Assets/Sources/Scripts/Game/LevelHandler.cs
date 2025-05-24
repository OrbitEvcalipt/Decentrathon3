using System.Collections.Generic;
using DG.Tweening;
using Sources.Scripts.Common;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class LevelHandler : MonoBehaviour
    {
        [SerializeField] private LevelPointHandler playerPoint;
        [SerializeField] private LevelPointHandler[] battlePoints;

        private int _currentPointIndex = 0;

        private void OnEnable()
        {
            EventsHandler.OnGameStart += Initialize;
            EventsHandler.OnNextSublevel += OnNextSublevel;
            EventsHandler.OnClaimBattleResult += ClearLevel;
        }

        private void OnDisable()
        {
            EventsHandler.OnGameStart -= Initialize;
            EventsHandler.OnNextSublevel -= OnNextSublevel;
            EventsHandler.OnClaimBattleResult -= ClearLevel;
        }

        private void Initialize()
        {
            
        }

        private void OnNextSublevel(int index, int playerLives, int enemyLives)
        {
            _currentPointIndex = index;
            if (_currentPointIndex == 0)
            {
                playerPoint.Initialize(playerLives);
            }
            else
            {
                playerPoint.Action(EUnitAnimation.Run);
                playerPoint.transform.DOMove(battlePoints[_currentPointIndex].transform.position, 1.5f)
                    .SetEase(Ease.Linear)
                    .OnComplete(OnIdle);
                battlePoints[_currentPointIndex - 1].Clear();
            }

            battlePoints[_currentPointIndex].Initialize(enemyLives);
        }

        private void OnIdle()
        {
            playerPoint.Action(EUnitAnimation.Idle);
        }

        private void OnAttack()
        {
            playerPoint.Action(EUnitAnimation.Attack);
            battlePoints[_currentPointIndex].Action(EUnitAnimation.Attack);
        }

        private void OnEnemyDied()
        {
            battlePoints[_currentPointIndex].UnitDied();
        }

        private void ClearLevel()
        {
            playerPoint.Clear();
            foreach (LevelPointHandler point in battlePoints)
            {
                point.Clear();
            }
        }
    }
}