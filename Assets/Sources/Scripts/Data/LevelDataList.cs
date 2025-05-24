using Sources.Scripts.Game;
using UnityEngine;

namespace Sources.Scripts.Data
{
    [CreateAssetMenu(menuName = "Data/Create LevelDataList", fileName = "LevelDataList", order = 0)]
    public class LevelDataList : ScriptableObject
    {
        public LevelData[] levels;
    }
}