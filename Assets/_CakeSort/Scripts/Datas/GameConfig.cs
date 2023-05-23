using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Data/GameConfig")]
public class GameConfig : ScriptableObject
{
    [SerializeField] private float _waitTimeBeforeSpawnPlate;
    [SerializeField] private int _maxCakeLevelDefault;
    [SerializeField] private int _maxTypeOfCakePerTable;
    [SerializeField] private int _maxCakeLevel;

    public float WaitTimeBeforeSpawnPlate => _waitTimeBeforeSpawnPlate;
    public int MaxCakeLevelDefault => _maxCakeLevelDefault;
    public int MaxTypeOfCakePerTable => _maxTypeOfCakePerTable;
    public int MaxCakeLevel => _maxCakeLevel;
}