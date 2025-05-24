using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sources.Scripts.Data;

namespace Sources.Scripts.Game
{
    [System.Serializable]
    public class EntityData
    {
        public bool isPlayer;
        [ShowIf("isPlayer")] public int lives;
        public List<ActionData> actions;
    }
}