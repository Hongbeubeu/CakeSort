using UnityEngine;

[CreateAssetMenu(fileName = "BoardSettings", menuName = "Data/BoardSettings", order = 0)]
public class BoardSettings : ScriptableObject
{
    public Transform CellTemplate;
    public Vector2Int BoardSize;
    public float CellSize = 1;
}