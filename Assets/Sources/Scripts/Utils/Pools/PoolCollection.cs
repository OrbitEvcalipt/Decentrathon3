using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace FunnyBlox.Utils
{
    public class PoolCollection : MonoBehaviour
    {
        [LabelText("Список пулов")] [SerializeField] [TableList(ShowIndexLabels = true)]
        private Element[] _poolElements;

        private static List<Pool> _pools;

        public static PoolCollection instance;

        void Awake()
        {
            if (!instance)
                instance = this;

            _pools = new List<Pool>();
            for (int i = 0, imax = _poolElements.Length; i < imax; i++)
            {
                CreatePool(i);
            }
        }

        public void CreatePool(int number)
        {
            Pool pool
                = new Pool(_poolElements[number]
                    , _poolElements[number]._parent == null ? transform : _poolElements[number]._parent);

            _pools.Add(pool);
        }

        public static Pool CreatePool(Element element, Transform parent)
        {
            Pool pool = new Pool(element, parent == null ? instance.transform : parent);
            _pools.Add(pool);

            return pool;
        }

        public static List<Pool> GetPools()
        {
            return _pools;
        }

        public static Pool GetPool(int number)
        {
            return _pools[number];
        }

        public static Pool GetPool(string poolName)
        {
            try
            {
                return _pools.Find(_pools => _pools._name == poolName);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static PoolElement Spawn(GameObject obj, Vector3 pos, Quaternion rot, float activeDuration = -1f)
        {
            return instance._Spawn(obj, null, pos, rot, activeDuration);
        }

        public static PoolElement Spawn(Component component, Vector3 pos, Quaternion rot, float activeDuration = -1f)
        {
            return instance._Spawn(component.gameObject, component, pos, rot, activeDuration);
        }

        public PoolElement _Spawn(GameObject obj, Component component, Vector3 pos, Quaternion rot,
            float activeDuration = -1f)
        {
            Pool pool = GetPoolID(obj);

            if (pool == null)
            {
                Element e = new Element();
                e._transform = obj.transform;
                e._name = obj.transform.name;
                e.Component = component;
                pool = CreatePool(e, null);
            }

            PoolElement pe = pool.GetElement;

            pe.Transform.position = pos;
            pe.Transform.rotation = rot;

            if (activeDuration > 0) StartCoroutine(UnspawnRoutine(pe, activeDuration));

            return pe;
        }

        private Pool GetPoolID(GameObject obj)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].Origin == obj) return _pools[i];
            }

            return null;
        }

        private IEnumerator UnspawnRoutine(PoolElement poolElement, float activeDuration)
        {
            if (activeDuration > 0) yield return new WaitForSeconds(activeDuration);
            Unspawn(poolElement);
        }

        public static void Unspawn(PoolElement poolElement)
        {
            instance._Unspawn(poolElement);
        }

        public void _Unspawn(PoolElement poolElement)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].FreeElement(poolElement)) break;
            }
        }
        
        public static void Unspawn(Transform poolElement)
        {
            instance._Unspawn(poolElement);
        }
        
        public void _Unspawn(Transform poolElement)
        {
            for (int i = 0; i < _pools.Count; i++)
            {
                if (_pools[i].FreeElement(poolElement)) break;
            }
        }
    }
}