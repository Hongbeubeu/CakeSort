using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class Plate : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField, ReadOnly] private List<Cake> _cakes = new();
    [SerializeField] private int _rootSortingOrder;
    private PlateSettings _settings;
    private Vector3 spawnPosition;


    private void Awake()
    {
        _settings = GameManager.Instance.GameSettings.PlateSettings;
    }

    public void SetOrderInLayer(int order)
    {
        _spriteRenderer.sortingOrder = order;
        foreach (var c in _cakes)
        {
            c.SetOrderInLayer(order + 1);
        }
    }

    public void ResetOrderInLayer()
    {
        _spriteRenderer.sortingOrder = _rootSortingOrder;
        foreach (var c in _cakes)
        {
            c.ResetOrderInLayer();
        }
    }

    [Button(ButtonSizes.Gigantic)]
    public void RandomCake()
    {
        DestroyAllCakes();
        var numberOfCake = Random.Range(_settings.MinPiecePerPlate, _settings.MaxPiecePerPlate);
        _cakes = GameManager.Instance.ObjectPooler.InstantiateRandomCakes(5, transform, numberOfCake);
        ArrangeCake();
        spawnPosition = transform.position;
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

    public void Destroy()
    {
        DestroyAllCakes();
        GameManager.Instance.ObjectPooler.DestroyPlate(gameObject);
    }

    public bool IsTouchOnPlate(Vector2 position)
    {
        return Mathf.Abs(position.x - transform.position.x) <=
               GameManager.Instance.GameSettings.BoardSettings.CellSize / 2f &&
               Mathf.Abs(position.y - transform.position.y) <=
               GameManager.Instance.GameSettings.BoardSettings.CellSize / 2f;
    }

    public void SetPosition(Vector2 position)
    {
        transform.position = position;
    }

    public void DoMove(Vector2 worldPosition)
    {
        transform.position = worldPosition;
    }

    public void ResetToSpawnPosition()
    {
        transform.DOKill();
        transform.DOMove(spawnPosition, 0.5f);
    }
}