using System;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D _rigidBody;
    private ConstantForce2D _constantForce2D;

    private void Start()
    {
        CameraController.OnRotation += rotation =>
        {
            _rigidBody.bodyType = RigidbodyType2D.Static;
            transform.rotation = rotation;
        };

        _rigidBody = GetComponent<Rigidbody2D> ();
        _constantForce2D = GetComponent<ConstantForce2D> ();

        CameraController.OnDownDirectionChanged += direction =>
        {
            var gravityForceAmount = _rigidBody.mass * Physics2D.gravity.magnitude;
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;
            _constantForce2D.force = direction switch
            {
                CameraController.Direction.Up => new Vector2(0, gravityForceAmount),
                CameraController.Direction.Down => new Vector2(0, -gravityForceAmount),
                CameraController.Direction.Left => new Vector2(gravityForceAmount, 0),
                CameraController.Direction.Right => new Vector2(-gravityForceAmount, 0),
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
        };
    }
}