using UnityEngine;

public class PlayerData : ISaveable
{
    private int _minCakeLevel;
    private int _maxCakeLevel;

    public int MinCakeLevel
    {
        get => _minCakeLevel;
        set => _minCakeLevel = value;
    }

    public int MaxCakeLevel
    {
        get => _maxCakeLevel;
        set => _maxCakeLevel = value;
    }

    public string ToString()
    {
        return $"{nameof(MaxCakeLevel)}: {MinCakeLevel}; {nameof(MaxCakeLevel)}: {MaxCakeLevel}";
    }

    public void Save()
    {
        ES3.Save(Constants.MinCakeLevel, MinCakeLevel);
        ES3.Save(Constants.MaxCakeLevel, MaxCakeLevel);
        Debug.Log($"<color=blue>SAVED</color> - {ToString()}");
    }

    public void Load()
    {
        MinCakeLevel = ES3.Load<int>(Constants.MinCakeLevel, 0);
        MaxCakeLevel = ES3.Load<int>(Constants.MaxCakeLevel, GameManager.Instance.GameConfig.MaxCakeLevelDefault);
        Debug.Log($"<color=blue>LOADED</color> - {ToString()}");
    }
}