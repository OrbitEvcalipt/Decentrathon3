using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class GameEventsService:MonoBehaviour
    {
        public void OnGameStart()
        {
            EventsHandler.GameStart();
        }
    }
}