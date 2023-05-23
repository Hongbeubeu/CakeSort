using System.Collections.Generic;
using Ultimate.Core.Runtime.Pool;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPooler", menuName = "Data/ObjectPooler")]
public class ObjectPooler : ScriptableObject
{
    [SerializeField] private Cake[] _cakes;
    [SerializeField] private Plate _plate;

    #region Instantiate Cakes

    public Cake InstantiateRandomCake(int minLevel, int targetLevel, Transform parent)
    {
        if (targetLevel >= _cakes.Length)
            targetLevel = _cakes.Length - 1;
        var randomIndex = Random.Range(minLevel, targetLevel + 1);
        return FastPoolManager.GetPool(_cakes[randomIndex])
            .FastInstantiate<Cake>(Vector3.zero, Quaternion.identity, parent);
    }

    public List<Cake> InstantiateRandomCakes(int minLevel, int targetLevel, Transform parent, int quantity)
    {
        if (quantity <= 0)
        {
            Debug.LogError("Zero cake is not illegal");
            return null;
        }

        var result = new List<Cake>();
        for (var i = 0; i < quantity; i++)
        {
            var cake = InstantiateRandomCake(minLevel, targetLevel, parent);
            result.Add(cake);
        }

        return result;
    }

    public void DestroyCake(int level, GameObject gameObject)
    {
        if (level >= _cakes.Length)
        {
            Debug.LogError($"Not found Cake with index: {level} for Destroy");
            return;
        }

        FastPoolManager.GetPool(_cakes[level]).FastDestroy(gameObject);
    }

    #endregion Instantiate Cakes

    #region Instantiate Plates

    public Plate InstantiatePlate(Transform parent, Vector3 position)
    {
        return FastPoolManager.GetPool(_plate).FastInstantiate<Plate>(position, Quaternion.identity, parent);
    }

    public void DestroyPlate(GameObject gameObject)
    {
        FastPoolManager.GetPool(_plate).FastDestroy(gameObject);
    }

    #endregion
}