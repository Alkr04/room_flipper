using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static Action<Quaternion> OnRotation;

    [SerializeField] private float rotationSpeed = 5f;

    private Camera _camera;
    private Quaternion _targetRotation;
    private bool _isRotating;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (!_isRotating)
        {
            HandleInput();
        }
        else
        {
            if (Mathf.Abs(_camera.transform.eulerAngles.z - _targetRotation.eulerAngles.z) < 0.1f)
            {
                _isRotating = false;
                _camera.transform.rotation = _targetRotation;
            }
            else
            {
                var rotation = Quaternion.Slerp(
                    _camera.transform.rotation,
                    _targetRotation,
                    Time.deltaTime * rotationSpeed);

                _camera.transform.rotation = rotation;
                OnRotation?.Invoke(rotation);
            }
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            _isRotating = true;
            _targetRotation = Quaternion.Euler(_camera.transform.eulerAngles + Vector3.forward * 90);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            _isRotating = true;
            _targetRotation = Quaternion.Euler(_camera.transform.eulerAngles - Vector3.forward * 90);
        }
    }
}