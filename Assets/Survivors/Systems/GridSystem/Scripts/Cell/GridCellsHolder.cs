using System.Collections.Generic;

namespace Survivors.Grid
{
    internal class GridCellsHolder
    {
        internal List<GridCell> GridCells => gridCells;

        private readonly List<GridCell> gridCells;
        private readonly int rowsCount;
        private readonly int columnsCount;
        private readonly int extraCount;

        public GridCellsHolder(List<GridCell> _gridCells, int _rowsCount, int _columnsCount, int _extraArouncCameraCount)
        {
            gridCells = _gridCells;
            rowsCount = _rowsCount;
            columnsCount = _columnsCount;
            extraCount = _extraArouncCameraCount;
        }

        internal GridCell[] GetNearestCells(GridCell _gridCell)
        {
            // TODO: find nearest cells
            return gridCells.ToArray();
        }

        internal int GetColumnsCount()
        {
            return columnsCount + (extraCount * 2);
        }

        internal int GetRowsCount()
        {
            return rowsCount + (extraCount * 2);
        }
    }
}
