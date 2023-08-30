using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Grid
{
    internal class GridCell : MonoBehaviour
    {
        private enum CellPositionType
        {
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight,
        }

        internal int ArrayIndex => arrayIndex;
        internal int RowIndex => rowIndex;
        internal int ColumnIndex => columnIndex;
        internal List<IGridMovable> Movables => movables;

        [Header("Used only for debugging!")]
        [SerializeField] private List<Transform> MovablesTransforms;

        private int arrayIndex;
        private int rowIndex;
        private int columnIndex;

        private Rect rect;
        private bool initialized;
        private bool useDebugMode;
        private Transform cellTransform;
        private List<IGridMovable> movables;

        private const float NEAR_EDGE_DETECTION_OFFSET = 2;

        internal void Init(Rect _rect, int _arrayIndex, int _rowIndex, int _columnIndex)
        {
            rect = _rect;
            arrayIndex = _arrayIndex;
            rowIndex = _rowIndex;
            columnIndex = _columnIndex;

            initialized = true;
            cellTransform = transform;
            movables = new List<IGridMovable>();
            MovablesTransforms = new List<Transform>();
            PositionObjectAtCenter(_rect);
        }

        internal void SetDebugModeState(bool _enabled)
        {
            useDebugMode = _enabled;
        }

        internal bool ContainsPosition(Vector3 _position)
        {
            Vector3 _bottomLeft = GetPosition(CellPositionType.BottomLeft);
            Vector3 _topRight = GetPosition(CellPositionType.TopRight);

            return _position.x >= _bottomLeft.x && _position.x <= _topRight.x
                && _position.z >= _bottomLeft.z && _position.z <= _topRight.z;
        }

        internal bool NearRightEdge(Vector3 _position)
        {
            Vector3 _topRight = GetPosition(CellPositionType.TopRight);
            _topRight.x -= NEAR_EDGE_DETECTION_OFFSET;
            return _topRight.x <= _position.x;
        }

        internal bool NearLeftEdge(Vector3 _position)
        {
            Vector3 _topLeft = GetPosition(CellPositionType.TopLeft);
            _topLeft.x += NEAR_EDGE_DETECTION_OFFSET;
            return _topLeft.x >= _position.x;
        }


        internal bool NearTopEdge(Vector3 _position)
        {
            Vector3 _topLeft = GetPosition(CellPositionType.TopLeft);
            _topLeft.z -= NEAR_EDGE_DETECTION_OFFSET;
            return _topLeft.z <= _position.z;
        }


        internal bool NearBottomEdge(Vector3 _position)
        {
            Vector3 _bottomLeft = GetPosition(CellPositionType.BottomLeft);
            _bottomLeft.z += NEAR_EDGE_DETECTION_OFFSET;
            return _bottomLeft.z >= _position.z;
        }

        internal void AddMovable(IGridMovable _movable)
        {
            if(movables.Contains(_movable))
            {
                Debug.LogError("Grid cell already contains movable!");
            }
            else
            {
                movables.Add(_movable);
                MovablesTransforms.Add(_movable.MovableTransform);
            }
        }

        internal void RemoveMovable(IGridMovable _movable)
        {
            if (movables.Contains(_movable))
            {
                movables.Remove(_movable);
                MovablesTransforms.Remove(_movable.MovableTransform);
            }
            else
            {
                Debug.LogError("Grid cell can't remove movable!");
            }
        }

        private Vector3 GetPosition(CellPositionType _positionType)
        {
            switch (_positionType)
            {
                case CellPositionType.BottomLeft:
                    return cellTransform.position + new Vector3(rect.width / 2f * -1f, 0f, rect.height / 2f * -1f);
                case CellPositionType.BottomRight:
                    return cellTransform.position + new Vector3(rect.width / 2f, 0f, rect.height / 2f * -1f);
                case CellPositionType.TopLeft:
                    return cellTransform.position + new Vector3(rect.width / 2f * -1f, 0f, rect.height / 2f);
                case CellPositionType.TopRight:
                    return cellTransform.position + new Vector3(rect.width / 2f, 0f, rect.height / 2f);
            }
            Debug.LogError("Grid cell position not found, position type: " + _positionType);
            return Vector3.zero;
        }

        private void PositionObjectAtCenter(Rect _rect)
        {
            Vector3 _bottomLeft = new Vector3(_rect.xMin, 0f, _rect.yMin);
            Vector3 _bottomRight = new Vector3(_rect.xMax, 0f, _rect.yMin);
            Vector3 _topLeft = new Vector3(_rect.xMin, 0f, _rect.yMax);
            Vector3 _topRight = new Vector3(_rect.xMax, 0f, _rect.yMax);
            Vector3 _center = (_bottomLeft + _bottomRight + _topLeft + _topRight) / 4f;

            gameObject.transform.position = _center;
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!initialized || !useDebugMode)
            {
                return;
            }
            DrawRect();
        }

        private void DrawRect()
        {
            Gizmos.color = Color.red;

            Vector3 _bottomLeft = GetPosition(CellPositionType.BottomLeft);
            Vector3 _bottomRight = GetPosition(CellPositionType.BottomRight);
            Vector3 _topLeft = GetPosition(CellPositionType.TopLeft);
            Vector3 _topRight = GetPosition(CellPositionType.TopRight);

            Gizmos.DrawLine(_bottomLeft, _bottomRight);
            Gizmos.DrawLine(_bottomRight, _topRight);
            Gizmos.DrawLine(_topRight, _topLeft);
            Gizmos.DrawLine(_topLeft, _bottomLeft);
        }

#endif

    }
}
