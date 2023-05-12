using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class ArrangeBoard : MonoBehaviour
{
    [SerializeField] private BoardSettings _settings;
    [SerializeField] private List<Transform> _cells = new();

    [Button(ButtonSizes.Large)]
    private void FindCell()
    {
        if (_settings == null)
        {
            Debug.LogError($"Missing Setting: <color=red> {typeof(BoardSettings)}</color>");
            return;
        }

        DestroyAllCell();
        for (var i = 0; i < _settings.BoardSize.x * _settings.BoardSize.y; i++)
        {
            var cell = Instantiate(_settings.CellTemplate, transform, false);
            _cells.Add(cell);
        }
    }

    [Button(ButtonSizes.Gigantic)]
    private void Arrange()
    {
        if (_settings == null)
        {
            Debug.LogError($"Missing Setting: <color=red> {typeof(BoardSettings)}</color>");
            return;
        }

        if (_settings.BoardSize.x * _settings.BoardSize.y != _cells.Count) FindCell();

        var minX = -(_settings.BoardSize.x / 2f - _settings.CellSize / 2f);
        var minY = -(_settings.BoardSize.y / 2f - _settings.CellSize / 2f);
        var currentPos = new Vector2(minX, minY);

        for (var row = 0; row < _settings.BoardSize.y; row++)
        {
            currentPos.x = minX;
            for (var col = 0; col < _settings.BoardSize.x; col++)
            {
                _cells[row * _settings.BoardSize.x + col].localPosition = currentPos;
                currentPos.x += _settings.CellSize;
            }

            currentPos.y += _settings.CellSize;
        }

        EditorUtility.SetDirty(gameObject);
    }

    [Button(ButtonSizes.Medium)]
    private void DestroyAllCell()
    {
        foreach (var t in _cells)
        {
            DestroyImmediate(t.gameObject);
        }

        EditorUtility.SetDirty(gameObject);
        _cells.Clear();
    }
}