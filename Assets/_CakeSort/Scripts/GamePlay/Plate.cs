using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Plate : MonoBehaviour
{
    [SerializeField] private int _minCakePerPlate;
    [SerializeField] private int _maxCakePerPlate;
    [SerializeField, ReadOnly] private List<Cake> _cakes = new();

    [Button(ButtonSizes.Gigantic)]
    public void RandomCake()
    {
        DestroyAllCakes();
        var numberOfCake = Random.Range(_minCakePerPlate, _maxCakePerPlate);
        _cakes = GameManager.Instance.ObjectPooler.InstantiateRandomCakes(2, transform, numberOfCake);
        ArrangeCake();
    }

    private void ArrangeCake()
    {
        _cakes = _cakes.OrderBy(c => c.ID).ToList();
        for (var i = 0; i < _cakes.Count; i++)
        {
            _cakes[i].RotateToIndex(i);
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