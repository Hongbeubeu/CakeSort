using System.Collections;
using Sirenix.OdinInspector;
using Ultimate.Core.Runtime.Singleton;
using UnityEngine;

public class GameController : Singleton<GameController>
{
    [SerializeField] private Spawner _spawner;
    [SerializeField] private BoardController _boardController;

    public Spawner Spawner => _spawner;
    public BoardController BoardController => _boardController;

    public override void Init()
    {
    }

    [Button(ButtonSizes.Gigantic)]
    public void StartGame()
    {
        BoardController.InitCells();
        Spawner.Spawn();
    }

    private IEnumerator SpawnCoroutine()
    {
        yield return new WaitForSeconds(GameManager.Instance.GameConfig.WaitTimeBeforeSpawnPlate);
    }
}