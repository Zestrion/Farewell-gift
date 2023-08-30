using UnityEngine;

namespace Survivors.Grid
{
    [CreateAssetMenu(fileName = "GridSettings", menuName = "Survivor/GridSettings", order = 1)]
    internal class GridSettings : ScriptableObject
    {
        internal int ColumnsCount => gridCellsColumnsCount;
        internal int RowsCount => gridCellsRowsCount;
        internal int ExtraCount => extraCellsCount;

        internal bool DebugGrid => debugGrid;
        internal bool DebugGridMovement => debugGridMovement;

        [Tooltip("How many grid cell columns will be in the camera view")]
        [SerializeField] private int gridCellsColumnsCount;
        [Tooltip("How many grid cell rows will be in the camera view")]
        [SerializeField] private int gridCellsRowsCount;
        [Tooltip("How many grid cells will be around the generated cells?")]
        [SerializeField] private int extraCellsCount;

        [Header("Debug Settings")]
        [SerializeField] private bool debugGrid;
        [SerializeField] private bool debugGridMovement;
    }
}
