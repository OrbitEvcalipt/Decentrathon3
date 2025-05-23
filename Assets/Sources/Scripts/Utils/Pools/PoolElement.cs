using System;
using UnityEngine;
using System.Collections;

namespace FunnyBlox.Utils
{
    public class PoolElement
    {
        public bool IsBusy { get; set; }

        public Transform Transform { get; set; }

        public Component Component { get; set; }
    }
}