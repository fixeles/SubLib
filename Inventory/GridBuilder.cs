using System;
using UnityEngine;

namespace Game.Scripts.UtilsSubmodule.Inventory
{
    public class GridBuilder : MonoBehaviour
    {
        [field: SerializeField] public Vector3[] Grid = new Vector3[0];
        [SerializeField] private Vector3 _spacing = Vector3.one;
        [SerializeField, Min(1)] private Vector2Int _size = Vector2Int.one;
        [SerializeField, Min(1)] private int _positionsCount = 1;
        [SerializeField] private Pivot _pivot;
        private Vector3 _center = Vector3.zero;

        private void OnValidate()
        {
            Build();
        }

        private void Build()
        {
            SetCenter();
            Grid = new Vector3[_positionsCount];

            // int layersCount = (int)(_positionsCount / (_size.x * _size.y));
            int currentLayer = 0;
            int currentGridIndex = 0;

            while (currentGridIndex < _positionsCount)
            {
                for (int x = 0; x < _size.x; x++)
                {
                    for (int z = 0; z < _size.y; z++)
                    {
                        Grid[currentGridIndex] = new(_center.x + x * _spacing.x,
                            _center.y + _spacing.y * currentLayer,
                            _center.z + z * _spacing.z);
                        currentGridIndex++;
                        if (currentGridIndex == _positionsCount) return;
                    }
                }

                currentLayer++;
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < Grid.Length; i++)
            {
                Gizmos.DrawSphere(transform.position + Grid[i], .2f);
            }
        }

        private void SetCenter()
        {
            _center = Vector3.zero;
            switch (_pivot)
            {
                case Pivot.MiddleX:
                    _center.x = -(_spacing.x * _size.x - _spacing.x) / 2;
                    return;

                case Pivot.MiddleZ:
                    _center.z = -(_spacing.z * _size.y - _spacing.z) / 2;
                    return;

                case Pivot.Middle:
                    _center.x = -(_spacing.x * _size.x - _spacing.x) / 2;
                    _center.z = -(_spacing.z * _size.y - _spacing.z) / 2;
                    return;

                default:
                    return;
            }
        }

        private enum Pivot
        {
            Zero,
            MiddleX,
            MiddleZ,
            Middle
        }
    }
}