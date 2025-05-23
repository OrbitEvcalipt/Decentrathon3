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

            int enemyIndexAction = enemyView.GetRandomIndexInActive();
            ActionData enemyAction = enemyData.actions[enemyIndexAction];

            CalculateBattleResult(playerAction, enemyAction);

            playerView.UpdateButtons(playerIndexAction);
            enemyView.UpdateButtons(enemyIndexAction);
        }

        private void CalculateBattleResult(ActionData playerAction, ActionData enemyAction)
        {
            //enemy
            //--attack
            if (enemyAction.missedActionType == playerAction.actionType)
            {
                enemyData.lives -= playerAction.actionForce;
                Debug.Log("Enemy take damage " + playerAction.actionForce);
            }
            //--equally
            else if (playerAction.actionType == enemyAction.actionType)
            {
                int damage = enemyAction.actionForce - playerAction.actionForce;
                if (damage < 0)
                {
                    enemyData.lives -= damage;
                    Debug.Log("Enemy take damage " + damage);
                }
            }
            //--defense
            else if (playerAction.actionType == enemyAction.blockedActionType)
            {
                Debug.Log("Enemy take damage defenses");
                
            }

            enemyView.UpdateLivesText(enemyData.lives);

            //player
            //--attack
            if (playerAction.missedActionType == enemyAction.actionType)
            {
                playerData.lives -= enemyAction.actionForce;
                Debug.Log("Player take damage " + enemyAction.actionForce);
                
            }
            //--equally
            else if (enemyAction.actionType == playerAction.actionType)
            {
                int damage = playerAction.actionForce - enemyAction.actionForce;
                if (damage < 0)
                {
                    playerData.lives -= damage;
                    Debug.Log("Player take damage " + damage);
                    
                }
            }
            //--defense
            else if (playerAction.actionType == enemyAction.blockedActionType)
            {
                Debug.Log("Player take damage defenses");
            }

            playerView.UpdateLivesText(playerData.lives);
        }
    }
}