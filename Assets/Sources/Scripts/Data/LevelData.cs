using UnityEngine;

namespace Sources.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/Create LevelData", fileName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        public int number;
        public int enemyHealth;
        public int amountActions;
        public ActionData[] actions;
        public int rewardedValue;
    }
}