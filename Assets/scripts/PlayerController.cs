using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private static readonly int IsFalling = Animator.StringToHash("isFalling");
    private static readonly int OnImpact = Animator.StringToHash("onImpact");

    public static PlayerController Instance;

    private UIController _uiController;

    private Rigidbody2D _rigidBody;
    private ConstantForce2D _constantForce2D;
    private BoxCollider2D _collider2D;
    private Animator _animator;
    private float _gravityMagnitude;

    private bool _isFalling;

    private void Awake()
    {
        Instance = this;

        _uiController = GameObject.Find("Canvas").GetComponent<UIController>();

        _rigidBody = GetComponent<Rigidbody2D>();
        _constantForce2D = GetComponent<ConstantForce2D>();
        _collider2D = GetComponent<BoxCollider2D>();
        _animator = GetComponent<Animator>();

        _gravityMagnitude = Physics2D.gravity.magnitude;
    }

    private void Start()
    {
        CameraController.OnRotation += OnRotation;
        CameraController.OnDownDirectionChanged += OnDownDirectionChanged;
    }

    private void OnRotation(Quaternion rotation)
    {
        _rigidBody.bodyType = RigidbodyType2D.Static;
        transform.rotation = rotation;
    }

    private void OnDownDirectionChanged(CameraController.Direction direction)
    {
        var gravityForceAmount = _rigidBody.mass * _gravityMagnitude;
        _rigidBody.bodyType = RigidbodyType2D.Dynamic;

        _rigidBody.constraints = direction is CameraController.Direction.Up or CameraController.Direction.Down ? RigidbodyConstraints2D.FreezePositionX : RigidbodyConstraints2D.FreezePositionY;

        _constantForce2D.force = direction switch
        {
            CameraController.Direction.Up => new Vector2(0, gravityForceAmount),
            CameraController.Direction.Down => new Vector2(0, -gravityForceAmount),
            CameraController.Direction.Left => new Vector2(gravityForceAmount, 0),
            CameraController.Direction.Right => new Vector2(-gravityForceAmount, 0),
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
        };
    }

    private void OnDestroy()
    {
        CameraController.OnRotation -= OnRotation;
        CameraController.OnDownDirectionChanged -= OnDownDirectionChanged;
    }

    private void Update()
    {
        if (IsTouchingGround() && _isFalling)
        {
            _animator.SetTrigger(OnImpact);
        }

        _isFalling = !IsTouchingGround();

        _animator.SetBool(IsFalling, _isFalling);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Window"))
        {
            _uiController.ShowMenuPanel();
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Exit"))
        {
            SceneController.NextLevel();
        }
    }

    public bool IsTouchingGround()
    {
        return _collider2D.IsTouchingLayers(LayerMask.GetMask("Platform"));
    }
}