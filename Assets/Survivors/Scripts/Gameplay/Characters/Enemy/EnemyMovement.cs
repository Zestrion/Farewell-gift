using UnityEngine;
using Survivors.Grid;

namespace Survivors.Gameplay
{
    public class EnemyMovement : GridMovableBase
    {
        public override MonoBehaviour MonoBehaviour => enemyRef;
        [SerializeField] private Enemy enemyRef;
    }
}
