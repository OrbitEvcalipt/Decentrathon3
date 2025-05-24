using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.UI
{
    public class GUIScreenService : MonoBehaviour
    {
        [SerializeField] private GUICanvasGroup screenMenu;
        [SerializeField] private GUICanvasGroup screenBattle;
        [SerializeField] private GUICanvasGroup screenGameWin;
        [SerializeField] private GUICanvasGroup screenGameOver;

        private void OnEnable()
        {
            EventsHandler.OnInitialize += ShowMenu;
            EventsHandler.OnGameStart += OnGameStart;
            EventsHandler.OnGameWin += OnGameWin;
            EventsHandler.OnGameOver += OnGameOver;
            EventsHandler.OnClaimBattleResult += ShowMenu;
            
        }


        private void OnDisable()
        {
            EventsHandler.OnInitialize -= ShowMenu;
            EventsHandler.OnGameStart -= OnGameStart;
            EventsHandler.OnGameWin -= OnGameWin;
            EventsHandler.OnGameOver -= OnGameOver;
            EventsHandler.OnClaimBattleResult -= ShowMenu;
        }

        private void ShowMenu()
        {
            screenBattle.Hide();
            screenGameWin.Hide();
            screenGameOver.Hide();

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

            screenGameWin.Show();
        }

        private void OnGameOver()
        {
            screenBattle.Hide();

            screenGameOver.Show();
        }
    }
}