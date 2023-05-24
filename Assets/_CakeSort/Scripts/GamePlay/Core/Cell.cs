using UnityEngine;

public class Cell : MonoBehaviour
{
    private Plate _plate;
    public bool IsContainPlate => _plate != null;

    public void SetPlate(Plate plate = null)
    {
        _plate = plate;
    }

    public void RemovePlate()
    {
        if (_plate == null)
            return;
        
        _plate.Destroy();
        _plate = null;
    }
}