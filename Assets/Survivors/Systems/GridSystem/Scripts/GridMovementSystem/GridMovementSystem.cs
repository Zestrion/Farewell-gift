using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Grid
{
    internal class GridMovementSystem : MonoBehaviour
    {
        internal class GridMovableData
        {
            public int GridCellIndex;
        }

        [SerializeField] private float movablesCellsUpdateInterval = 0.1f;

        private Dictionary<IGridMovable, GridMovableData> movablesDataDictionary;
        private List<IGridMovable> movables;
        private List<IGridMovable> nearbyMovables;

        private GridCellsHolder gridCellsHolder;
        private float nextCellsUpdateTime;

        private const int WITHOUT_GRID_CELL_INDEX = -1;

        internal void Init(GridCellsHolder _gridCellsHolder)
        {
            movablesDataDictionary = new Dictionary<IGridMovable, GridMovableData>();
            movables = new List<IGridMovable>();
            nearbyMovables = new List<IGridMovable>();
            gridCellsHolder = _gridCellsHolder;
        }

        internal void AddObjectToGridMovement(IGridMovable _movable)
        {
            if(movablesDataDictionary.ContainsKey(_movable))
            {
                Debug.LogError("Movable already added to the dictionary");
            }
            else
            {
                movables.Add(_movable);
                GridMovableData _movableData = new GridMovableData()
                {
                    GridCellIndex = WITHOUT_GRID_CELL_INDEX
                };
                movablesDataDictionary.Add(_movable, _movableData);
                CheckMovablesAndDataCount();
                UpdateMovableCell(_movable, _movableData);
            }
        }

        internal void RemoveObjectFromGridMovement(IGridMovable _movable)
        {
            if (movablesDataDictionary.ContainsKey(_movable))
            {
                GridCell _movableGridCell = TryGetGridCellByMovable(_movable);
                if(_movableGridCell != null)
                {
                    _movableGridCell.Movables.Remove(_movable);
                }
                movablesDataDictionary[_movable] = null;
                movablesDataDictionary.Remove(_movable);
                movables.Remove(_movable);
                CheckMovablesAndDataCount();
            }
            else
            {
                Debug.LogError("Movable to remove not found!");
            }
        }

        internal List<IGridMovable> GetNearbyMovables(IGridMovable _movable)
        {
            GridCell _gridCell = TryGetGridCellByMovable(_movable);
            if (_gridCell != null)
            {
                return GetNearbyMovables(_gridCell, _movable.MovableTransform.position);
            }
            return null;
        }

        internal T GetClosestMovable<T>(Vector3 _position)
            where T : MonoBehaviour
        {
            // Try find closest enemy in cell
            List<IGridMovable> _nearbyMovables = GetNearbyMovablesByPosition(_position);
            List<T> _requiredTypeMovables;
            if (_nearbyMovables != null)
            {
                _requiredTypeMovables = GetMovablesOfTypeFromArray<T>(_nearbyMovables);
                if (_requiredTypeMovables.Count > 0)
                {
                    return GetClosestMovableFromArray(_requiredTypeMovables, _position);
                }
            }
            else
            {
                _requiredTypeMovables = new List<T>();
            }

            // Try find closest enemy in grid
            _requiredTypeMovables.Clear();
            for (int i = 0; i < gridCellsHolder.GridCells.Count; i++)
            {
                _requiredTypeMovables.AddRange(GetMovablesOfTypeFromArray<T>(gridCellsHolder.GridCells[i].Movables));
            }
            if(_requiredTypeMovables.Count > 0)
            {
                return GetClosestMovableFromArray(_requiredTypeMovables, _position);
            }

            return null;
        }

        private List<T> GetMovablesOfTypeFromArray<T>(List<IGridMovable> _movables)
            where T : MonoBehaviour
        {
            List<T> _t = new List<T>();
            for (int i = 0; i < _movables.Count; i++)
            {
                if (_movables[i].MonoBehaviour is T)
                {
                    _t.Add(_movables[i].MonoBehaviour as T);
                }
            }
            return _t;
        }

        private T GetClosestMovableFromArray<T>(List<T> _objectsToUse, Vector3 _position)
            where T : MonoBehaviour
        {
            T _closest = null;
            float _sqrMagnitude = float.MaxValue;
            float _tempSqrMagnitude = float.MaxValue;
            for (int i = 0; i < _objectsToUse.Count; i++)
            {
                _tempSqrMagnitude = (_position - _objectsToUse[i].transform.position).sqrMagnitude;
                if (i == 0 || _sqrMagnitude > _tempSqrMagnitude)
                {
                    _closest = _objectsToUse[i];
                    _sqrMagnitude = _tempSqrMagnitude;
                }
            }
            return _closest;
        }

        private List<IGridMovable> GetNearbyMovablesByPosition(Vector3 _position)
        {
            GridCell _gridCell = TryGetGridCellByPosition(_position);
            if(_gridCell != null)
            {
                return GetNearbyMovables(_gridCell, _position);
            }
            return null;
        }

        private GridCell TryGetGridCellByPosition(Vector3 _position)
        {
            return GetAllCells().FirstOrDefault(_gridCell => _gridCell.ContainsPosition(_position));
        }

        private GridCell TryGetGridCellByMovable(IGridMovable _movable)
        {
            if (!movablesDataDictionary.ContainsKey(_movable))
            {
                Debug.LogError("Can't find nearby objects. Movable not added to the GridMovementSystem.");
                return null;
            }

            GridMovableData _movableData = movablesDataDictionary[_movable];

            // Check if movable is in the grid cell
            if (!MovableDataContainsGridCell(_movableData))
            {
                return null;
            }
            else
            {
                return gridCellsHolder.GridCells[_movableData.GridCellIndex];
            }
        }

        private List<IGridMovable> GetNearbyMovables(GridCell _gridCell, Vector3 _position)
        {
            TryUpdateMovablesCells();

            nearbyMovables.Clear();
            nearbyMovables.AddRange(_gridCell.Movables);

            bool _nearRightEdge = false;
            bool _nearLeftEdge = false;

            if (NearRightEdge())
            {
                AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex + 1].Movables);
                _nearRightEdge = true;
            }

            if (NearLeftEdge())
            {
                AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex - 1].Movables);
                _nearLeftEdge = true;
            }

            if (NearTopEdge())
            {
                AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex + gridCellsHolder.GetColumnsCount()].Movables);

                if(UseTopRightDiagonal())
                {
                    AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex + (gridCellsHolder.GetColumnsCount() + 1)].Movables);
                }
                if (UseTopLeftDiagonal())
                {
                    AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex + (gridCellsHolder.GetColumnsCount() - 1)].Movables);
                }
            }

            if (NearBottomEdge())
            {
                AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount()].Movables);

                if (UseBottomRightDiagonal())
                {
                    AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() + 1].Movables);
                }
                if (UseBottomLeftDiagonal())
                {
                    AddToNearbyMovables(gridCellsHolder.GridCells[_gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() - 1].Movables);
                }
            }

            return nearbyMovables;

            bool NearRightEdge() => _gridCell.NearRightEdge(_position)
                && gridCellsHolder.GridCells.Count - 1 >= _gridCell.ArrayIndex + 1
                && gridCellsHolder.GridCells[_gridCell.ArrayIndex + 1].RowIndex
                    == _gridCell.RowIndex;

            bool NearLeftEdge() => _gridCell.NearLeftEdge(_position)
                && _gridCell.ArrayIndex - 1 >= 0
                && gridCellsHolder.GridCells[_gridCell.ArrayIndex - 1].RowIndex
                    == _gridCell.RowIndex;


            bool NearTopEdge() => _gridCell.NearTopEdge(_position)
                && gridCellsHolder.GridCells.Count - 1 >= _gridCell.ArrayIndex + gridCellsHolder.GetColumnsCount()
                && gridCellsHolder.GridCells[_gridCell.ArrayIndex + gridCellsHolder.GetColumnsCount()].ColumnIndex
                    == _gridCell.ColumnIndex;

            bool UseTopRightDiagonal() => _nearRightEdge
                    && gridCellsHolder.GridCells.Count - 1 >= _gridCell.ArrayIndex + (gridCellsHolder.GetColumnsCount() + 1)
                    && gridCellsHolder.GridCells[_gridCell.ArrayIndex + (gridCellsHolder.GetColumnsCount() + 1)].ColumnIndex
                    == _gridCell.ColumnIndex + 1;

            bool UseTopLeftDiagonal() => _nearLeftEdge
                    && gridCellsHolder.GridCells.Count - 1 >= _gridCell.ArrayIndex + (gridCellsHolder.GetColumnsCount() - 1)
                    && gridCellsHolder.GridCells[_gridCell.ArrayIndex + (gridCellsHolder.GetColumnsCount() - 1)].ColumnIndex
                    == _gridCell.ColumnIndex - 1;


            bool NearBottomEdge() => _gridCell.NearBottomEdge(_position)
                && _gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() >= 0
                && gridCellsHolder.GridCells[_gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount()].ColumnIndex
                    == _gridCell.ColumnIndex;

            bool UseBottomRightDiagonal() => _nearRightEdge
                    && _gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() + 1 >= 0
                    && gridCellsHolder.GridCells[_gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() + 1].ColumnIndex
                    == _gridCell.ColumnIndex + 1;

            bool UseBottomLeftDiagonal() => _nearLeftEdge
                    && _gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() - 1 >= 0
                    && gridCellsHolder.GridCells[_gridCell.ArrayIndex - gridCellsHolder.GetColumnsCount() - 1].ColumnIndex
                    == _gridCell.ColumnIndex - 1;

            void AddToNearbyMovables(List<IGridMovable> _movablesToAdd) => nearbyMovables.AddRange(_movablesToAdd);
        }

        private void TryUpdateMovablesCells()
        {
            if(Time.time > nextCellsUpdateTime)
            {
                for (int i = 0; i < movables.Count; i++)
                {
                    GridMovableData _data = movablesDataDictionary[movables[i]];
                    UpdateMovableCell(movables[i], _data);
                }
                nextCellsUpdateTime = Time.time + movablesCellsUpdateInterval;
            }
        }

        private void UpdateMovableCell(IGridMovable _movable, GridMovableData _data)
        {
            // Check the current grid cell
            if (!MovableInSameCell(_data))
            {
                // Try to get only the nearest cells
                GridCell[] _cellsToSearch = TryGetNearestCells(_data);
                // Set all if nearest cells are not found
                _cellsToSearch = _cellsToSearch != null ? _cellsToSearch : GetAllCells();

                // Remove movable from the current cell.
                GridCell _currentMovableCell = TryGetMovableGridCell(_data);
                if (_currentMovableCell != null)
                {
                    _currentMovableCell.RemoveMovable(_movable);
                }

                // Attempt to find a new cell
                Vector3 _movablePosition = _movable.MovableTransform.position;
                GridCell _newMovableCell = _cellsToSearch.FirstOrDefault(_gridCell => _gridCell.ContainsPosition(_movablePosition));

                if (_newMovableCell != null)
                {
                    _data.GridCellIndex = _newMovableCell.ArrayIndex;
                    _newMovableCell.AddMovable(_movable);
                }
                else
                {
                    _data.GridCellIndex = WITHOUT_GRID_CELL_INDEX;
                }
            }

            bool MovableInSameCell(GridMovableData _dataToUse) => MovableDataContainsGridCell(_dataToUse) && gridCellsHolder.GridCells[_data.GridCellIndex].ContainsPosition(_movable.MovableTransform.position);
            GridCell TryGetMovableGridCell(GridMovableData _dataToUse) => MovableDataContainsGridCell(_dataToUse) ? gridCellsHolder.GridCells[_dataToUse.GridCellIndex] : null;
            GridCell[] TryGetNearestCells(GridMovableData _dataToUse) => MovableDataContainsGridCell(_dataToUse) ? gridCellsHolder.GetNearestCells(gridCellsHolder.GridCells[_data.GridCellIndex]) : null;
        }

        private GridCell[] GetAllCells()
        {
            return gridCellsHolder.GridCells.ToArray();
        }

        private bool MovableDataContainsGridCell(GridMovableData _dataToCheck)
        {
            return _dataToCheck.GridCellIndex != WITHOUT_GRID_CELL_INDEX;
        }
        
        private void CheckMovablesAndDataCount()
        {
            if (movables.Count != movablesDataDictionary.Count)
            {
                IGridMovable[] _correctKeysArray = new IGridMovable[movablesDataDictionary.Count]; ;
                movablesDataDictionary.Keys.CopyTo(_correctKeysArray, 0);

                movables.Clear();
                movables = _correctKeysArray.ToList();
            }
        }

        private void Update()
        {
            TryUpdateMovablesCells();
        }
    }
}
