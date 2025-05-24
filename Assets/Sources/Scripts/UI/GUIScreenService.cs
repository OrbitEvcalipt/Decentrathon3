using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.UI
{
    public class GUIScreenService : MonoBehaviour
    {

        [SerializeField] private GUICanvasGroup screenMenu;
        [SerializeField] private GUICanvasGroup screenBattle;
    
        private void OnEnable()
        {
            EventsHandler.OnGameStart += OnGameStart;
            EventsHandler.OnGameWin += OnGameWin;
            EventsHandler.OnGameOver += OnGameOver;
        }

        private void OnDisable()
        {
            EventsHandler.OnGameStart -= OnGameStart;
            EventsHandler.OnGameWin -= OnGameWin;
            EventsHandler.OnGameOver -= OnGameOver;
        }

        private void Start()
        {
            screenBattle.Hide();
      
            screenMenu.Show();
        }

        private void OnGameStart()
        {
            screenMenu.Hide();
      
            screenBattle.Show();
        }

        private void OnGameWin()
        {
            screenBattle.Hide();
      
            screenMenu.Show();
        }
    
        private void OnGameOver()
        {
            screenBattle.Hide();
      
            screenMenu.Show();
        }
    }
}