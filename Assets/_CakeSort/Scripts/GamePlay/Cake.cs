using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Cake : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private int _indexInPlate;
    [SerializeField] private Ease _ease;
    [SerializeField] private RotateMode _mode;
    [SerializeField] private bool _isRelative;
    [SerializeField] private float _duration;
    public int ID => _id;

    public int IndexInPlate
    {
        get => _indexInPlate;
        set => _indexInPlate = value;
    }

    [Button]
    public void SetStartRotationIndex(int startIndex)
    {
        transform.DOKill();
        startIndex %= GameManager.Instance.GameConfig.Rotates.Length;
        var targetAngle = new Vector3(0, 0, GameManager.Instance.GameConfig.Rotates[startIndex]);
        transform.eulerAngles = targetAngle;
    }

    [Button]
    public void RotateToIndex(int targetIndex)
    {
        targetIndex %= GameManager.Instance.GameConfig.Rotates.Length;
        IndexInPlate = targetIndex;
        var targetAngle = new Vector3(0, 0, GameManager.Instance.GameConfig.Rotates[targetIndex]);
        transform.DORotate(targetAngle, _duration, _mode).SetEase(_ease).SetOptions(false);
    }

    [Button]
    public void Rotate(Vector3 from, Vector3 to)
    {
        transform.DOKill();
        transform.eulerAngles = from;
        transform.DORotate(to, _duration, _mode).SetEase(_ease).SetOptions(false);
    }

    public void Destroy()
    {
        transform.SetParent(null);
        GameManager.Instance.ObjectPooler.DestroyCake(_id, gameObject);
    }
}