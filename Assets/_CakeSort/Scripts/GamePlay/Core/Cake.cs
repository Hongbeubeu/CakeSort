using DG.Tweening;
using UnityEngine;

public class Cake : MonoBehaviour
{
	[SerializeField] private int _id;
	[SerializeField] private int _indexInPlate;
	[SerializeField] private SpriteRenderer _spriteRenderer;
	[SerializeField] private int _rootSortingOrder;
	private CakeSettings _settings;
	public int ID => _id;

	public int IndexInPlate
	{
		get => _indexInPlate;
		set => _indexInPlate = value;
	}

	private void Awake()
	{
		_settings = GameManager.Instance.GameSettings.CakeSettings;
	}

	public void SetOrderInLayer(int order)
	{
		_spriteRenderer.sortingOrder = order;
	}

	public void ResetOrderInLayer()
	{
		_spriteRenderer.sortingOrder = _rootSortingOrder;
	}

	public void SetRotation(Vector3 angle)
	{
		transform.rotation = Quaternion.Euler(angle);
	}

	public void DoRotate(Vector3 from, Vector3 angle)
	{
		transform.DOKill();
		transform.rotation = Quaternion.Euler(from);
		transform.DORotate(angle, _settings.CakeRotateDuration, RotateMode.FastBeyond360)
			.SetRelative(true);
	}

	public void MoveToPlate(Plate plate)
	{
		DoMoveAndRotate(plate.transform.position, GameManager.Instance.GameSettings.PlateSettings.Angles[plate.LastIndex]);
	}

	public void DoMoveAndRotate(Vector3 targetPosition, Vector3 targetAngle)
	{
		transform.DOKill();
		transform.DOMove(targetPosition, 0.5f);
		transform.DORotate(targetAngle, 0.5f);
	}

	public void Destroy()
	{
		transform.SetParent(null);
		GameManager.Instance.ObjectPooler.DestroyCake(_id, gameObject);
	}
}