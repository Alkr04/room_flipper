using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action PlayerExits;

    public static PlayerController Instance;

    private Rigidbody2D _rigidBody;
    private ConstantForce2D _constantForce2D;
    private BoxCollider2D _collider2D;
    private float _gravityMagnitude;

    private void Awake()
    {
        Instance = this;

        _rigidBody = GetComponent<Rigidbody2D> ();
        _constantForce2D = GetComponent<ConstantForce2D> ();
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Exit"))
        {
            Destroy(collision.collider.gameObject);
            PlayerExits?.Invoke();
        }
    }

    public bool IsTouchingGround()
    {
        return _collider2D.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
}