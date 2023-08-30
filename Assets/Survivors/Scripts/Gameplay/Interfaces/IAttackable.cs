using UnityEngine;

namespace Survivors.Gameplay
{
    public interface IAttackable
    {
        public bool IsAlive { get; }
        public Transform Transform { get; }
        public void OnHit(Damage _damage);
    }
}
