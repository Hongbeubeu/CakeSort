using Sirenix.OdinInspector;
using UnityEngine;

public class Cell : MonoBehaviour
{
	[SerializeField] private Plate _plate;
	public bool IsContainPlate => _plate != null;

	public Plate Plate => _plate;

	[Button(ButtonSizes.Large)]
	private bool HasPlate()
	{
		return _plate != null;
	}

	public void SetPlate(Plate plate = null)
	{
		_plate = plate;
		_plate.DestroyAction += OnPlateDestroy;
	}

	public void DestroyPlate()
	{
		if (_plate == null)
			return;
		_plate.Destroy();
		_plate = null;
	}

	private void RemovePlate()
	{
		_plate = null;
	}

	#region Actions

	private void OnPlateDestroy()
	{
		RemovePlate();
	}

	#endregion
}