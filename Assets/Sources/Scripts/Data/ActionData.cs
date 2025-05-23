using Sirenix.OdinInspector;
using UnityEngine;

namespace Sources.Scripts.Game
{
    [CreateAssetMenu(menuName = "Data/Create ActionData", fileName = "ActionData", order = 0)]
    public class ActionData: ScriptableObject
    {
        [LabelText("Название")]public string name;
        [LabelText("Уровень")]public int level = 1;
        [LabelText("Тип")]public EActionType actionType;
        [PreviewField(70)] public Sprite sprite;
        [LabelText("Количество урона")]public int actionForce = 1;
        [LabelText("Получает урон от")]public EActionType missedActionType;
        [LabelText("Блокирует урон от")]public EActionType blockedActionType;
    }
}