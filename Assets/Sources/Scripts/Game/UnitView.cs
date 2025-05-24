using FunnyBlox.Utils;
using UnityEngine;

namespace Sources.Scripts.Game
{
    public class UnitView : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public void Despawn()
        {
            PoolCollection.Unspawn(transform);
        }

        public void PlayAnimation(string animationName)
        {
            animator.Play(animationName);
        }
    }
}