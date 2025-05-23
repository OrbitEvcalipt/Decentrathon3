using UnityEngine;

namespace Sources.Scripts.Game
{
    [CreateAssetMenu(menuName = "Data/Create LevelDataList", fileName = "LevelDataList", order = 0)]
    public class LevelDataList : ScriptableObject
    {
        public LevelData[] levels;
    }
}