using System;
using UnityEngine;
using System.Collections.Generic;
using Object = UnityEngine.Object;

namespace FunnyBlox.Utils
{
  public class Pool
  {
    public GameObject Origin;

    /// <summary>
    /// Список элементов пула
    /// </summary>
    public List<PoolElement> PoolElements { get; private set; }

    /// <summary>
    /// Название пула
    /// </summary>
    public string _name { get; private set; }

    /// <summary>
    /// Трансформ оригинал
    /// </summary>
    private Transform _elementTransform;

    /// <summary>
    /// Выключать объект после его создания в списке
    /// </summary>
    private bool _disableAfterCreate;

    /// <summary>
    /// Компонент на элементе
    /// </summary>
    private Type _componentType;

    /// <summary>
    /// Родитель элемента (корневой пул)
    /// </summary>
    private Transform _parentElements;

    /// <summary>
    /// хранилище кушированного класса
    /// </summary>
    private PoolElement _poolElement;

    /// <summary>
    /// Конструктор пула элементов
    /// </summary>
    /// <param name="element"></param>
    /// <param name="parent"></param>
    public Pool(Element element, Transform parent)
    {
      Origin = element._transform.gameObject;
      PoolElements = new List<PoolElement>(element._countElements);

      _name = element._name == "" ? element._transform.name : element._name;

      _elementTransform = element._transform;
      _disableAfterCreate = element._disabledAfterCreate;
      if (element.Component)
        _componentType = element.Component.GetType();

      _parentElements = parent;

      for (int i = 0; i < element._countElements; ++i)
      {
        InstantiateElement();
      }
    }

    /// <summary>
    /// Инстансим объект в сцену
    /// </summary>
    /// <returns></returns>
    private PoolElement InstantiateElement()
    {
      _poolElement = new PoolElement();
      _poolElement.Transform = Object.Instantiate(
        _elementTransform
        , new Vector3(-1000f, -1000f, -1000f)
        , Quaternion.identity
        , _parentElements);
      _poolElement.Transform.name = string.Format("{0}_{1:000}", _elementTransform.name, PoolElements.Count);

      if (_disableAfterCreate)
        _poolElement.Transform.gameObject.SetActive(false);
      else
        _poolElement.Transform.localPosition = new Vector3(1000f, 1000f, 0f);


      if (_componentType != null)
      {
        var matches = _poolElement.Transform.gameObject.GetComponents(_componentType);
        _poolElement.Component = matches[0];
      }


      PoolElements.Add(_poolElement);

      return _poolElement;
    }

    /// <summary>
    /// Возвращает элемент из пула
    /// </summary>
    /// <returns></returns>
    public PoolElement GetElement
    {
      get
      {
        _poolElement = PoolElements.Find(PoolElements => !PoolElements.IsBusy);

        if (_poolElement == null) _poolElement = InstantiateElement();

        _poolElement.IsBusy = true;

        if (_disableAfterCreate) _poolElement.Transform.gameObject.SetActive(true);

        return _poolElement;
      }
    }

    /// <summary>
    /// Возвращает элемент обратно в пул
    /// </summary>
    /// <param name="element">Трансформ</param>
    /// /// <param name="reposition">Убирать за пределы экрана</param>
    public bool FreeElement(Transform element)
    {
      return FreeElement(element, true, false);
    }

    public bool FreeElement(Transform element, bool reposition, bool disable)
    {
      _poolElement = PoolElements.Find(PoolElements => PoolElements.Transform == element);

      if (_poolElement != null)
      {
        FreeElement(_poolElement, reposition, disable);
        return true;
      }

      return false;
    }

    public bool FreeElement(PoolElement poolElement)
    {
      if (PoolElements.Contains(poolElement))
      {
        FreeElement(poolElement, true, true);
        return true;
      }
      else
        return false;
    }

    private void FreeElement(PoolElement element, bool reposition, bool disable)
    {
      element.Transform.SetParent(_parentElements);
      element.IsBusy = false;
      if (reposition) element.Transform.localPosition = new Vector3(1000f, 1000f, 0f);
      if (_disableAfterCreate && disable) element.Transform.gameObject.SetActive(false);
    }
  }
}