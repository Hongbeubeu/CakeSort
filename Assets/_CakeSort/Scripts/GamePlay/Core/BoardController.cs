using System.Collections.Generic;
using UnityEngine;

public class BoardController : MonoBehaviour
{
	[SerializeField] private BoardSettings _settings;
	[SerializeField] private Cell[] _cells;
	private readonly Dictionary<Vector2, Cell> _managedCells = new();

	public void InitCells()
	{
		ResetBoard();
		if (_managedCells.Count == 0)
		{
			var minX = -(_settings.BoardSize.x / 2f - _settings.CellSize / 2f);
			var minY = -(_settings.BoardSize.y / 2f - _settings.CellSize / 2f);
			var currentPos = new Vector2(minX, minY);
			var index = 0;
			for (var row = 0; row < _settings.BoardSize.y; row++)
			{
				currentPos.x = minX;
				for (var col = 0; col < _settings.BoardSize.x; col++)
				{
					_managedCells.Add(currentPos, _cells[index]);
					currentPos.x += _settings.CellSize;
					index++;
				}

				currentPos.y += _settings.CellSize;
			}
		}

		foreach (var cell in _managedCells)
		{
			cell.Value.RemovePlate();
		}
	}

	public bool CanPlacePlate(Vector2 position, out Vector2 gridPosition)
	{
		gridPosition = ConvertToGrid(position);
		if (!_managedCells.ContainsKey(gridPosition))
			return false;
		return !_managedCells[gridPosition].IsContainPlate;
	}

	public void AddPlate(Vector2 gridPosition, Plate plate)
	{
		if (!_managedCells.ContainsKey(gridPosition))
		{
			Debug.LogError($"Not exist grid {gridPosition}");
			return;
		}

		_managedCells[gridPosition].SetPlate(plate);
	}

	public void RemovePlateAt(Vector2 gridPosition)
	{
		if (!_managedCells.ContainsKey(gridPosition))
		{
			Debug.LogError($"Not exist grid {gridPosition}");
			return;
		}

		_managedCells[gridPosition].RemovePlate();
	}

	private void ResetBoard()
	{
		foreach (var cell in _managedCells)
		{
			cell.Value.RemovePlate();
		}
	}

	public void FindNeighbourHasCakeId(Plate plate, int id)
	{
		Vector2 currentPos = plate.transform.position;
		foreach (var t in _settings.Directions)
		{
			var pos = currentPos + t;
			if (!_managedCells.ContainsKey(pos) || !_managedCells[pos].IsContainPlate)
			{
				continue;
			}

			if (_managedCells[pos].Plate.HasCake(id))
			{
				_managedCells[pos].Plate.MoveCakeToTarget(plate, id);
			}
		}
	}

	#region Utils

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

	#endregion
}