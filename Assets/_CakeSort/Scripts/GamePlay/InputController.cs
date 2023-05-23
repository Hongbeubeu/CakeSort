using System;
using Lean.Touch;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    private Plate _holdingPlate;

    private void OnEnable()
    {
        LeanTouch.OnFingerDown += OnFingerDown;
        LeanTouch.OnFingerUpdate += OnFingerUpdate;
        LeanTouch.OnFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        LeanTouch.OnFingerDown -= OnFingerDown;
        LeanTouch.OnFingerUpdate -= OnFingerUpdate;
        LeanTouch.OnFingerUp -= OnFingerUp;
    }

    private void OnFingerUpdate(LeanFinger finger)
    {
        if (finger.IsOverGui)
        {
            return;
        }

        if (_holdingPlate == null)
            return;
        var worldPosition = GetWorldPosition(finger);
        _holdingPlate.DoMove(worldPosition);
    }

    private void OnFingerDown(LeanFinger finger)
    {
        if (finger.IsOverGui)
        {
            return;
        }

        var worldPosition = GetWorldPosition(finger);
        if (GameController.Instance.Spawner.IsTouchOnPlate(worldPosition, out _holdingPlate))
        {
            _holdingPlate.SetOrderInLayer(10);
        }
    }

    private void OnFingerUp(LeanFinger finger)
    {
        if (_holdingPlate == null)
            return;

        _holdingPlate.ResetOrderInLayer();

        if (GameController.Instance.BoardController.CanPlacePlate(GetWorldPosition(finger), out var gridPosition))
        {
            _holdingPlate.SetPosition(gridPosition);
            GameController.Instance.BoardController.AddPlate(gridPosition, _holdingPlate);
            GameController.Instance.Spawner.RemovePlate(_holdingPlate);
            _holdingPlate = null;
            return;
        }

        _holdingPlate.ResetToSpawnPosition();
        _holdingPlate = null;
    }

    private Vector2 GetWorldPosition(LeanFinger finger)
    {
        Vector2 worldPosition = _camera.ScreenToWorldPoint(finger.ScreenPosition);
        return worldPosition;
    }
}