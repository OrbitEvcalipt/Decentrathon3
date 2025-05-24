using Sources.Scripts.Common;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            CommonData.levelNumber = SaveManager.LoadInt(CommonData.PLAYERPREFS_LEVEL_NUMBER, 0);
            
            EventsHandler.Initialize();
        }
    }
}