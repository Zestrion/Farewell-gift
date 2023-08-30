using System.Collections.Generic;
using UnityEngine;

namespace Survivors.Grid
{
    public class GridManager : Singleton<GridManager>
    {
        [SerializeField] private GridSettings gridSettings;
        [SerializeField] private GridMovementSystem movementSystem;
        [Tooltip("Grid will follow this target position")]
        [SerializeField] private Transform gridTargetToFollow;
        [SerializeField] private Vector3 gridOffsetFromTarget;

        private GridCellsHolder gridCellsHolder;
        private Transform gridTransform;
        private bool containsTargetToFollow;
        private bool isInitialized;

        protected override void Awake()
        {
            base.Awake();
            Init();
        }

        public void Init()
        {
            GenerateGrid();
            movementSystem.Init(gridCellsHolder);
            TryEnableGridDebug();
            gridTransform = transform;
            containsTargetToFollow = gridTargetToFollow != null;
            isInitialized = true;
        }

        public void AddToGridMovement(IGridMovable _gridMovable)
        {
            if(CheckInitialization())
            {
                movementSystem.AddObjectToGridMovement(_gridMovable);
                _gridMovable.SetDebugModeState(gridSettings.DebugGridMovement);
            }
        }

        public void RemoveFromGridMovement(IGridMovable _gridMovable)
        {
            if (CheckInitialization())
            {
                movementSystem.RemoveObjectFromGridMovement(_gridMovable);
            }
        }

        public List<IGridMovable> GetNearbyMovables(IGridMovable _movable)
        {
            if (CheckInitialization())
            {
                return movementSystem.GetNearbyMovables(_movable);
            }
            return null;
        }

        public T GetClosestMovable<T>(Vector3 _position)
            where T : MonoBehaviour
        {
            if (CheckInitialization())
            {
                return movementSystem.GetClosestMovable<T>(_position);
            }
            return null;
        }

        private void GenerateGrid()
        {
            GridGenerator _gridGenerator = new GridGenerator();
            GridGenerator.GenerationSettings _generationSettings = new GridGenerator.GenerationSettings()
            {
                RowsCount = gridSettings.RowsCount,
                ColumnsCount = gridSettings.ColumnsCount,
                ExtraCellsAroundCount = gridSettings.ExtraCount,
                ParentTransform = transform
            };
            List<GridCell> _generatedCells = _gridGenerator.GenerateGrid(_generationSettings);
            gridCellsHolder = new GridCellsHolder(_generatedCells, gridSettings.RowsCount, gridSettings.ColumnsCount, gridSettings.ExtraCount);
        }

        private bool CheckInitialization()
        {
            if(!isInitialized)
            {
                Debug.LogError("You are attempting to use an uninitialized GridManager. Please ensure you initialize it before use.");
            }
            return isInitialized;
        }

        private void TryEnableGridDebug()
        {
#if UNITY_EDITOR
            if(gridSettings.DebugGrid)
            {
                foreach(GridCell _cell in gridCellsHolder.GridCells)
                {
                    _cell.SetDebugModeState(true);
                }
            }
#endif
        }

        private void UpdateGridPosition()
        {
            if(containsTargetToFollow)
            {
                gridTransform.position = gridTargetToFollow.position + gridOffsetFromTarget;
            }
        }

        private void Update() {
            UpdateGridPosition();
        }
    }
}
