using UnityEngine;

[CreateAssetMenu(fileName = "PlateSettings", menuName = "Data/Settings/PlateSettings", order = 0)]
public class PlateSettings : ScriptableObject
{
    public int MinPiecePerPlate = 1;
    public int MaxPiecePerPlate = 6;
    public Vector3[] Angles;

    public int PiecePerPlate => Angles.Length;
}