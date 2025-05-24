using System.Collections.Generic;
using FunnyBlox.Game;
using UnityEngine;

namespace FunnyBlox
{
  [CreateAssetMenu(fileName = "CurrencyManagerData", menuName = "Data/CurrencyManagerData", order = 52)]
  public class CurrencyManagerData : ScriptableObject
  {
    public List<Currency> Currencies;
  }
}