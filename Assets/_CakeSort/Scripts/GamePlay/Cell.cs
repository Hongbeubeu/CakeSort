using UnityEngine;

public class Cell : MonoBehaviour
{
	private Plate _plate;
	public bool IsContainPlate => _plate != null;

	public void SetPlate(Plate plate = null)
	{
		_plate = plate;
	}
}