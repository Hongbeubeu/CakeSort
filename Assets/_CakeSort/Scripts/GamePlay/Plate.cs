using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Cake> _cakes = new();

    [Button(ButtonSizes.Gigantic)]
    public void RandomCake()
    {
        DestroyAllCakes();
        var numberOfCake = Random.Range(GameManager.Instance.GameConfig.MinPiecePerPlate,
            GameManager.Instance.GameConfig.MaxPiecePerPlate);
        _cakes = GameManager.Instance.ObjectPooler.InstantiateRandomCakes(2, transform, numberOfCake);
        ArrangeCake();
    }

    private void ArrangeCake()
    {
        _cakes = _cakes.OrderBy(c => c.ID).ToList();
        var randomStartIndex = Random.Range(0, GameManager.Instance.GameConfig.PiecePerPlate);
        var index = randomStartIndex;
        foreach (var cake in _cakes)
        {
            index %= GameManager.Instance.GameConfig.PiecePerPlate;
            cake.SetStartRotationIndex(randomStartIndex);
            cake.RotateToIndex(index);
            index++;
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    private void DestroyAllCakes()
    {
        foreach (var cake in _cakes)
        {
            cake.Destroy();
        }

        _cakes.Clear();
    }
}