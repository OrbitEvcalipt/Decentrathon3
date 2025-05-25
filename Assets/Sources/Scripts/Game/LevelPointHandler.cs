using System.Collections.Generic;
using FunnyBlox.Utils;
using Sources.Scripts.Common;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class LevelPointHandler : MonoBehaviour
    {
        [SerializeField] private UnitsList unitsList;

        private List<UnitView> units = new List<UnitView>();

        public void Initialize(int lives)
        {
            int counter = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                UnitView unit = (UnitView)PoolCollection.Spawn(
                    unitsList.units[counter]
                    , transform.GetChild(i).position
                    , Quaternion.identity
                ).Component;

                unit.transform.parent = transform;
                unit.PlayAnimation(nameof(EUnitAnimation.Idle));
                units.Add(unit);

                counter++;
                if (counter >= lives) break;
            }
        }

        public void Clear()
        {
            foreach (UnitView unit in units)
            {
                PoolCollection.Unspawn(unit.transform);
            }

            units.Clear();
        }

        public void Action(EUnitAnimation unitAnimation)
        {
            foreach (UnitView unit in units)
            {
                unit.PlayAnimation(unitAnimation.ToString());
            }
        }

        public void UnitDied()
        {
            int index = Random.Range(0, units.Count);

            units[index].PlayAnimation(nameof(EUnitAnimation.Die));

            units[index].Despawn();
            units.RemoveAt(index);
        }
    }
}