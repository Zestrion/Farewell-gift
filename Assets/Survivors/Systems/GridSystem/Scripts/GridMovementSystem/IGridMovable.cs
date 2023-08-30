using UnityEngine;

namespace Survivors.Grid
{
    public interface IGridMovable
    {
        public Transform MovableTransform { get; }
        public MonoBehaviour MonoBehaviour { get; }
        public void SetDebugModeState(bool _enabled);
    }
}
