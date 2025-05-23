using System.Collections.Generic;
using Sirenix.OdinInspector;

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