using System.Collections.Generic;
using Sources.Scripts.Common;
using Sources.Scripts.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Scripts.Game
{
    public class LevelService : MonoBehaviour
    {
        [SerializeField] private List<TMP_Text> levelNumberLabels;
        [SerializeField] private Slider sublevelProgressbar;


        private void OnEnable()
        {
            EventsHandler.OnInitialize += OnInitialize;
            EventsHandler.OnAfterInitializeBattle += OnAfterInitializeBattle;
            EventsHandler.OnNextSublevel += OnNextSublevel;
            EventsHandler.OnGameWin += OnGameWin;
        }

        private void OnDisable()
        {
            EventsHandler.OnInitialize -= OnInitialize;
            EventsHandler.OnAfterInitializeBattle -= OnAfterInitializeBattle;
            EventsHandler.OnNextSublevel -= OnNextSublevel;
            EventsHandler.OnGameWin -= OnGameWin;
        }

        private void OnInitialize()
        {
            CommonData.levelNumber = SaveManager.LoadInt(CommonData.PLAYERPREFS_LEVEL_NUMBER, 0);
        }

        private void OnAfterInitializeBattle(int maxValue)
        {
            sublevelProgressbar.maxValue = maxValue;
            sublevelProgressbar.value = 0;
            
            levelNumberLabels.ForEach(label => label.text = $"Level {CommonData.levelNumber}");
        }

        private void OnNextSublevel(int subLevelNumber)
        {
            sublevelProgressbar.value = subLevelNumber;
        }

        private void OnGameWin()
        {
            CommonData.levelNumber++;
            SaveManager.Save(CommonData.PLAYERPREFS_LEVEL_NUMBER, CommonData.levelNumber);

            levelNumberLabels.ForEach(label => label.text = $"Level {CommonData.levelNumber}");

        }
    }
}