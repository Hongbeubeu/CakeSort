using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
    [SerializeField] private BoardSettings _settings;
    private readonly Dictionary<Vector2, Cell> _cells = new();

    private void Start()
    {
        InitCells();
    }

    private void InitCells()
    {
        _cells.Clear();
        var minX = -(_settings.BoardSize.x / 2f - _settings.CellSize / 2f);
        var minY = -(_settings.BoardSize.y / 2f - _settings.CellSize / 2f);
        var currentPos = new Vector2(minX, minY);

        for (var row = 0; row < _settings.BoardSize.y; row++)
        {
            currentPos.x = minX;
            for (var col = 0; col < _settings.BoardSize.x; col++)
            {
                _cells.Add(currentPos, null);
                currentPos.x += _settings.CellSize;
            }

            currentPos.y += _settings.CellSize;
        }
    }

    public bool CanPlacePlate(Vector2 position, out Vector2 gridPosition)
    {
        gridPosition = ConvertToGrid(position);
        if (!_cells.ContainsKey(gridPosition))
            return false;
        return _cells[gridPosition] == null;
    }

    private Vector2 ConvertToGrid(Vector2 worldPosition)
    {
        var result = Vector2.zero;
        var minX = -(_settings.BoardSize.x / 2f);
        var minY = -(_settings.BoardSize.y / 2f);

        var deltaX = worldPosition.x - minX;
        var deltaY = worldPosition.y - minY;

        var delta = deltaX >= 0 ? _settings.CellSize / 2f : -_settings.CellSize / 2f;
        result.x = minX + (int) (deltaX / _settings.CellSize) + delta;

        delta = deltaY >= 0 ? _settings.CellSize / 2f : -_settings.CellSize / 2f;
        result.y = minY + (int) (deltaY / _settings.CellSize) + delta;

        return result;
    }
}