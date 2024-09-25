using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    public static event Action PlayerDies;
    public static event Action PlayerExits;

    public static PlayerController Instance;

    private Rigidbody2D _rigidBody;
    private ConstantForce2D _constantForce2D;
    private BoxCollider2D _collider2D;
    private float _gravityMagnitude;

    private void Awake()
    {
        Instance = this;

        _rigidBody = GetComponent<Rigidbody2D>();
        _constantForce2D = GetComponent<ConstantForce2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _gravityMagnitude = Physics2D.gravity.magnitude;
    }

    private void Start()
    {
        CameraController.OnRotation += rotation =>
        {
            _rigidBody.bodyType = RigidbodyType2D.Static;
            transform.rotation = rotation;
        };

        CameraController.OnDownDirectionChanged += direction =>
        {

            var gravityForceAmount = _rigidBody.mass * _gravityMagnitude;
            _rigidBody.bodyType = RigidbodyType2D.Dynamic;

            _rigidBody.constraints =
                direction is CameraController.Direction.Up or CameraController.Direction.Down ?
                    RigidbodyConstraints2D.FreezePositionX : RigidbodyConstraints2D.FreezePositionY;

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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Window"))
        {
            PlayerDies?.Invoke();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Exit"))
        {
            PlayerExits?.Invoke();
        }
    }

    public bool IsTouchingGround()
    {
        return _collider2D.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
}