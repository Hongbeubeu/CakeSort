using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig")]
public class GameConfig : ScriptableObject
{
    public int PiecePerPlate => Rotates.Length;
    public int MinPiecePerPlate = 1;
    public int MaxPiecePerPlate = 6;
    public float[] Rotates;
}