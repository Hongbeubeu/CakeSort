using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Plate : MonoBehaviour
{
    [SerializeField, ReadOnly] private List<Cake> _cakes = new();
    private PlateSettings _settings;

    private void Start()
    {
        _settings = GameManager.Instance.GameSettings.PlateSettings;
    }

    [Button(ButtonSizes.Gigantic)]
    public void RandomCake()
    {
        DestroyAllCakes();
        var numberOfCake = Random.Range(_settings.MinPiecePerPlate, _settings.MaxPiecePerPlate);
        _cakes = GameManager.Instance.ObjectPooler.InstantiateRandomCakes(5, transform, numberOfCake);
        ArrangeCake();
    }

    private void ArrangeCake()
    {
        _cakes = _cakes.OrderBy(c => c.ID).ToList();
        var randomStartIndex = Random.Range(0, _settings.PiecePerPlate);
        var angle = _settings.Angles[randomStartIndex];
        for (var i = 0; i < _cakes.Count; i++)
        {
            angle.z = -i * 60;
            _cakes[i].DoRotate(_settings.Angles[randomStartIndex], angle);
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