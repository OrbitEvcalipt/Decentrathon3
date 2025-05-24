using FunnyBlox.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class UnitView : MonoBehaviour
    {
        private Animator animator;

        private void Initialize()
        {
            animator = GetComponent<Animator>();
        }

        public void Despawn()
        {
            PoolCollection.Unspawn(transform);
        }

        public void PlayAnimation(string animationName)
        {
            if(!animator) Initialize();
            animator.Play(animationName);
        }
        
    }
}