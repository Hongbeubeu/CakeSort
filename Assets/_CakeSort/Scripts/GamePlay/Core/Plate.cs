using System;
using System.Collections;
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

    public int LastIndex => (_cakes[^1].IndexInPlate + 1) % _settings.MaxPiecePerPlate;
    public bool IsFullOfCake => _cakes.Count == _settings.MaxPiecePerPlate;
    public bool IsCompleted => CheckComplete();
    public int NumberTypeOfCake => CountNumberTypeOfCake();
    public int NumberOfEmptySlot => _settings.MaxPiecePerPlate - _cakes.Count;

    public Action DestroyAction;

    private void Awake()
    {
        _settings = GameManager.Instance.GameSettings.PlateSettings;
    }

    private void OnEnable()
    {
        Reset();
    }

    private void Reset()
    {
        transform.localScale = Vector3.one;
        var color = _spriteRenderer.color;
        color.a = 1;
        _spriteRenderer.color = color;
    }

    private int CountNumberTypeOfCake()
    {
        var id = _cakes[0].ID;
        var count = 1;
        foreach (var t in _cakes)
        {
            if (t.ID == id) continue;
            id = t.ID;
            count++;
        }

        return count;
    }

    private int CountNumberOfCakeWithType(int id)
    {
        return _cakes.Count(cake => cake.ID == id);
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
        _cakes = GameManager.Instance.ObjectPooler.InstantiateRandomCakes(
            GameManager.Instance.PlayerData.MinCakeLevel,
            GameManager.Instance.PlayerData.MaxCakeLevel,
            transform,
            numberOfCake
        );
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
            _cakes[i].IndexInPlate = randomStartIndex + i;
            _cakes[i].DoRotate(_settings.Angles[randomStartIndex], angle);
        }
    }

    [Button(ButtonStyle.FoldoutButton)]
    private void DestroyAllCakes()
    {
        foreach (var cake in _cakes)
        {
            cake?.Destroy();
        }

        _cakes.Clear();
    }

    public void Destroy(bool hasAnim = true)
    {
        DestroyAllCakes();
        if (hasAnim)
        {
            transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.InBack).OnComplete(ReturnPool);
            var targetColor = _spriteRenderer.color;
            targetColor.a = 0;
            DOVirtual.Color(_spriteRenderer.color,
                    targetColor,
                    0.3f,
                    value => { _spriteRenderer.color = value; })
                .SetEase(Ease.InBack);
            return;
        }

        ReturnPool();
    }

    private void ReturnPool()
    {
        DestroyAction.Invoke();
        GameManager.Instance.ObjectPooler.DestroyPlate(gameObject);
    }

    public bool IsTouchOnPlate(Vector2 position)
    {
        return Mathf.Abs(position.x - transform.position.x) <=
               GameManager.Instance.GameSettings.BoardSettings.CellSize / 2f &&
               Mathf.Abs(position.y - transform.position.y) <=
               GameManager.Instance.GameSettings.BoardSettings.CellSize / 2f;
    }

    public void UpdatePosition(Vector2 worldPosition)
    {
        transform.position = worldPosition;
    }

    public void ResetToSpawnPosition()
    {
        transform.DOKill();
        var duration = Vector2.Distance(transform.position, spawnPosition) / 20f;
        transform.DOMove(spawnPosition, duration).OnComplete(ResetOrderInLayer);
    }

    public void MoveToPositionOnBoard(Vector2 position)
    {
        transform.DOKill();
        var duration = Vector2.Distance(transform.position, spawnPosition) / 50f;
        transform.DOMove(position, duration).OnComplete(OnPlacedToBoard);
    }

    private void OnPlacedToBoard()
    {
        FindCakeNearBy();
    }

    private void FindCakeNearBy()
    {
        var ids = new List<int>();
        var id = -1;
        foreach (var cake in _cakes)
        {
            if (id == cake.ID) continue;
            id = cake.ID;
            ids.Add(id);
        }

        foreach (var i in ids)
        {
            GameController.Instance.BoardController.FindNeighbourHasCakeId(this, i);
        }
    }

    public void AddCake(Cake cake)
    {
        cake.transform.SetParent(transform);
        cake.MoveToPlate(this);
        cake.IndexInPlate = LastIndex;
        _cakes.Add(cake);
        StartCoroutine(IEArrangeCake());
        if (!IsFullOfCake)
            return;
        StartCoroutine(IECheckComplete());
    }

    private IEnumerator IEArrangeCake()
    {
        yield return new WaitForSeconds(0.5f);
        ArrangeCake();
    }

    private IEnumerator IECheckComplete()
    {
        yield return new WaitForSeconds(0.5f);
        if (!IsCompleted) yield break;
        Destroy();
    }

    public bool HasCake(int id)
    {
        return _cakes.Any(t => t.ID == id);
    }

    private bool CheckComplete()
    {
        var id = _cakes[0].ID;
        var count = 0;
        for (var i = 0; i < _cakes.Count; i++)
        {
            if (_cakes[i].ID != id)
                return false;
            count++;
        }

        return count == _settings.MaxPiecePerPlate;
    }

    private bool IsEmpty()
    {
        return _cakes.Count == 0;
    }

    public void MoveCakeToTarget(Plate plate, int id)
    {
        var removedCakes = new List<Cake>();
        foreach (var t in _cakes.Where(t => t.ID == id))
        {
            if (plate.IsFullOfCake)
                continue;
            plate.AddCake(t);
            removedCakes.Add(t);
        }

        foreach (var t in removedCakes)
        {
            _cakes.Remove(t);
        }
        
        if (IsEmpty())
        {
            Destroy();
            return;
        }

        ArrangeCake();
        // FindCakeNearBy();
    }
}