using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cake : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private int _indexInPlate;
    public int ID => _id;

    public int IndexInPlate
    {
        get => _indexInPlate;
        set => _indexInPlate = value;
    }

    [Button]
    public void RotateToIndex(int targetIndex)
    {
        targetIndex = Mathf.Clamp(targetIndex, 0, GameManager.Instance.GameConfig.Rotates.Length - 1);
        IndexInPlate = targetIndex;
        var targetAngle = new Vector3(0, 0, GameManager.Instance.GameConfig.Rotates[targetIndex]);
        transform.DORotate(targetAngle, 0.5f, RotateMode.LocalAxisAdd).SetEase(Ease.OutBack);
    }

    public void Destroy()
    {
        transform.SetParent(null);
        GameManager.Instance.ObjectPooler.DestroyCake(_id, gameObject);
    }
}