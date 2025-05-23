using Sirenix.OdinInspector;
using UnityEngine;

namespace FunnyBlox.Utils
{
  [System.Serializable]
  public class Element
  {
    [VerticalGroup("Основное")] [LabelText("Имя пула")]
    public string _name;

    [VerticalGroup("Основное")] [LabelText("Трансформ")]
    public Transform _transform;

    [VerticalGroup("Основное")] [LabelText("Компонент")]
    public Component Component;

    [VerticalGroup("Дополнительное")] [LabelText("Парент объектов пула")]
    public Transform _parent;

    [VerticalGroup("Дополнительное")] [LabelText("Количество элементов в пуле")]
    public int _countElements;

    [VerticalGroup("Дополнительное")] [LabelText("Отключать после создания")]
    public bool _disabledAfterCreate;
  }
}