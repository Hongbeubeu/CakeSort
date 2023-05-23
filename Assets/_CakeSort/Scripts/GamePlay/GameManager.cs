using System;
using Sirenix.OdinInspector;
using Ultimate.Core.Runtime.Singleton;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerData PlayerData;

    public ObjectPooler ObjectPooler;
    public GameConfig GameConfig;
    public GameSettings GameSettings;

    public override void Init()
    {
        PlayerData = new PlayerData();
    }

    public void OnEnable()
    {
        PlayerData.Load();
    }

    public void OnDisable()
    {
        PlayerData.Save();
    }

    #region Cheats

    [Button(ButtonSizes.Gigantic)]
    private void CheatMaxCakeLevel(int maxLevel)
    {
        maxLevel %= GameConfig.MaxCakeLevel;
        var minLevel = Mathf.Clamp(maxLevel - GameConfig.MaxTypeOfCakePerTable, 0,
            GameConfig.MaxCakeLevel - GameConfig.MaxTypeOfCakePerTable);
        PlayerData.MinCakeLevel = minLevel;
        PlayerData.MaxCakeLevel = maxLevel;
    }

    [Button(ButtonSizes.Medium)]
    private void CheatResetMaxLevel()
    {
        PlayerData.MinCakeLevel = 0;
        PlayerData.MaxCakeLevel = GameConfig.MaxCakeLevelDefault;
    }

    #endregion
}