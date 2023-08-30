using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Grid
{
    internal class GridGenerator
    {
        internal struct GenerationSettings
        {
            public int RowsCount;
            public int ColumnsCount;
            public int ExtraCellsAroundCount;
            public Transform ParentTransform;
        }

        internal List<GridCell> GenerateGrid(GenerationSettings _generationSettings)
        {
            return GenerateCameraViewRects(_generationSettings.RowsCount,
                _generationSettings.ColumnsCount,
                _generationSettings.ExtraCellsAroundCount,
                _generationSettings.ParentTransform);
        }

        private List<GridCell> GenerateCameraViewRects(int _rows, int _columns, int _addRectsAround, Transform _cellsParent)
        {
            List<GridCell> _gridCells = new List<GridCell>();
            Camera _camera = Camera.main;

            int _newRowsCount = _rows + (_addRectsAround * 2);
            int _newColumnsCount = _columns + (_addRectsAround * 2);

            float _screenHeight = _camera.orthographicSize * 2f;
            float _screenWidth = _screenHeight * _camera.aspect;

            float _cellWidth = _screenWidth / _columns;
            float _cellHeight = _screenHeight / _rows;

            int _index = 0;

            for (int row = 0; row < _newRowsCount; row++)
            {
                for (int col = 0; col < _newColumnsCount; col++)
                {
                    float xPos = (col * _cellWidth - _screenWidth / 2f) - (_cellWidth * _addRectsAround);
                    float yPos = (row * _cellHeight - _screenHeight / 2f) - (_cellHeight * _addRectsAround);

                    Rect rect = new Rect(xPos, yPos, _cellWidth, _cellHeight);

                    _gridCells.Add(CreateCellFromRect(rect, _index, row, col, _cellsParent));
                    _index++;
                }
            }
            return _gridCells;
        }

        private GridCell CreateCellFromRect(Rect _rect, int _arrayIndex, int _row, int _column, Transform _cellParent)
        {
            GameObject _cellGameObject = new GameObject("GridCell");
            _cellGameObject.transform.parent = _cellParent;

            GridCell _cell = _cellGameObject.AddComponent<GridCell>();
            _cell.Init(_rect, _arrayIndex, _row, _column);
            return _cell;
        }
    }
}
