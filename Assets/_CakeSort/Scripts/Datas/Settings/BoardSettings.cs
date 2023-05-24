using UnityEngine;

[CreateAssetMenu(fileName = "BoardSettings", menuName = "Data/BoardSettings", order = 0)]
public class BoardSettings : ScriptableObject
{
	[SerializeField] private Transform _cellTemplate;
	[SerializeField] private Vector2Int _boardSize;
	[SerializeField] private float _cellSize = 1;
	[SerializeField] private Vector2[] _directions;

	public Transform CellTemplate => _cellTemplate;
	public Vector2Int BoardSize => _boardSize;
	public float CellSize => _cellSize;
	public Vector2[] Directions => _directions;
}