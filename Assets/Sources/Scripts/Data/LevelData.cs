using System.Collections.Generic;
using FunnyBlox.Game;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.Scripts.Data
{
    [System.Serializable]
    public class RewardedCurrency
    {
        [LabelText("Название")] public string name;
        [LabelText("Количество")] public int amount;
    }
    
    [System.Serializable]
    public class SubLevel
    {
        [LabelText("Номер подуровня")]public int number;
        [LabelText("Количество жизней")]public int enemyHealth;
        [LabelText("Список действий")]public int amountActions;
        [LabelText("Список действий")]public ActionData[] actions;
        [LabelText("Вознаграждение за прохождение")]public List<RewardedCurrency> RewardedCurrencies;
    }
    
    [CreateAssetMenu(menuName = "Data/Create LevelData", fileName = "LevelData", order = 0)]
    public class LevelData : ScriptableObject
    {
        [LabelText("Номер уровня")]public int number;
        [LabelText("Вознаграждение за прохождение")]public List<RewardedCurrency> RewardedCurrencies;
        [LabelText("Список подуровней")]public SubLevel[] subLevels;
    }
}