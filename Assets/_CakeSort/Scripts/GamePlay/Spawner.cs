using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Vector3[] _spawnerPositions;
    [SerializeField] private List<Plate> _plates = new(3);

    private void Start()
    {
        Spawn();
    }

    public void RemovePlate(Plate plate)
    {
        if (_plates.Contains(plate))
        {
            _plates.Remove(plate);
        }
    }

    public bool IsTouchOnPlate(Vector2 position, out Plate plate)
    {
        plate = null;
        foreach (var p in _plates.Where(p => p.IsTouchOnPlate(position)))
        {
            plate = p;
            return true;
        }

        return false;
    }

    [Button(ButtonSizes.Gigantic)]
    public void Spawn()
    {
        DestroyPlates();
        foreach (var t in _spawnerPositions)
        {
            var plate = GameManager.Instance.ObjectPooler.InstantiatePlate(transform, t);
            _plates.Add(plate);
            plate.RandomCake();
        }
    }

    private void DestroyPlates()
    {
        foreach (var t in _plates)
        {
            t.Destroy();
        }

        _plates.Clear();
    }
}