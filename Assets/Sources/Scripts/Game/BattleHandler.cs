using System.Linq;
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


        public void PrepareBattle()
        {
            playerData.lives = playerData.lives;
            playerData.actions = levelDataList.levels[0].actions.ToList();
            playerView.Prepare(playerData.lives, playerData.actions);

            enemyData.lives = levelDataList.levels[0].enemyHealth;
            enemyData.actions = levelDataList.levels[0].actions.ToList();
            enemyView.Prepare(enemyData.lives, enemyData.actions);
        }

        public void StartBattle()
        {
            int playerIndexAction = playerView.GetIndexInOn();
            if (playerIndexAction == -1) return;
            ActionData playerAction = playerData.actions[playerIndexAction];
            playerView.UpdateButtons(playerIndexAction);

            int enemyIndexAction = enemyView.GetRandomIndexInActive();
            ActionData enemyAction = enemyData.actions[playerIndexAction];
            enemyView.UpdateButtons(enemyIndexAction);
        }
    }
}