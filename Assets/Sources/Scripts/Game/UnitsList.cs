using UnityEngine;

namespace Sources.Scripts.Game
{
    [CreateAssetMenu(menuName = "Data/Create UnitsList", fileName = "UnitsList", order = 50)]
    public class UnitsList : ScriptableObject
    {
        public string unitGang;
        public UnitView[] units;
    }
}