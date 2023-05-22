using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "Data/Settings/GameSettings", order = 0)]
public class GameSettings : ScriptableObject
{
    public CakeSettings CakeSettings;
    public PlateSettings PlateSettings;
}