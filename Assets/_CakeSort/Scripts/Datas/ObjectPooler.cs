using System.Collections.Generic;
using Ultimate.Core.Runtime.Pool;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPooler", menuName = "Data/ObjectPooler")]
public class ObjectPooler : ScriptableObject
{
    [SerializeField] private Cake[] _cakes;

    public Cake InstantiateRandomCake(int targetLevel, Transform parent)
    {
        if (targetLevel >= _cakes.Length)
            targetLevel = _cakes.Length - 1;
        var randomIndex = Random.Range(0, targetLevel);
        return FastPoolManager.GetPool(_cakes[randomIndex])
            .FastInstantiate<Cake>(Vector3.zero, Quaternion.identity, parent);
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

    public List<Cake> InstantiateRandomCakes(int targetLevel, Transform parent, int quantity)
    {
        if (quantity <= 0)
        {
            Debug.LogError("Zero cake is not illegal");
            return null;
        }

        var result = new List<Cake>();
        for (var i = 0; i < quantity; i++)
        {
            var cake = InstantiateRandomCake(targetLevel, parent);
            result.Add(cake);
        }

        return result;
    }
}