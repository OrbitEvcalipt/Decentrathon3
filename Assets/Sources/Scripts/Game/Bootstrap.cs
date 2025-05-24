using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class Bootstrap : MonoBehaviour
    {
        private void Start()
        {
            EventsHandler.Initialize();
        }
    }
}