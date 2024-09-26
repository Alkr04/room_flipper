using System;
using UnityEngine;

namespace Controller
{
    public class PlayerController : MonoBehaviour
    {
        private static readonly int RotationStarted = Animator.StringToHash("onRotationStarted");
        private static readonly int IsFalling = Animator.StringToHash("isFalling");
        private static readonly int OnImpact = Animator.StringToHash("onImpact");

        public static PlayerController Instance;

        [SerializeField] private float velocityToFall;
        [SerializeField] private float velocityToFallOver;

        private Rigidbody2D _rigidBody;
        private ConstantForce2D _constantForce2D;
        private BoxCollider2D _collider2D;
        private Animator _animator;
        private float _gravityMagnitude;

        private bool _isFalling;

        private AudioSource _audioSource;

        private void Awake()
        {
            Application.targetFrameRate = 60;

            Instance = this;

            _rigidBody = GetComponent<Rigidbody2D>();
            _constantForce2D = GetComponent<ConstantForce2D>();
            _collider2D = GetComponent<BoxCollider2D>();
            _animator = GetComponent<Animator>();

            _gravityMagnitude = Physics2D.gravity.magnitude;

            _audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            CameraController.OnRotationStarted += OnRotationStarted;
            CameraController.OnRotation += OnRotation;
            CameraController.OnDownDirectionChanged += OnDownDirectionChanged;
        }

        private void OnRotationStarted()
        {
            _animator.SetTrigger(RotationStarted);
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
            CameraController.OnRotationStarted -= OnRotationStarted;
            CameraController.OnRotation -= OnRotation;
            CameraController.OnDownDirectionChanged -= OnDownDirectionChanged;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Platform"))
            {
                if (other.relativeVelocity.sqrMagnitude > velocityToFallOver * velocityToFallOver)
                {
                    _audioSource.Play();
                    _animator.SetTrigger(OnImpact);
                }
            }
        }

        private void Update()
        {
            if (IsTouchingGround())
            {
                _isFalling = false;
                _animator.SetBool(IsFalling, _isFalling);
            }
            else if (!_isFalling && _rigidBody.velocity.sqrMagnitude > velocityToFall * velocityToFall)
            {
                if (!IsTouchingGround())
                {
                    _isFalling = true;
                    _animator.SetBool(IsFalling, _isFalling);
                }
            }
        }

        private bool IsTouchingGround()
        {
            return _collider2D.IsTouchingLayers(LayerMask.GetMask("Platform"));
        }

        public bool IsReady()
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("player_stand"))
            {
                return false;
            }
            if (!IsTouchingGround())
            {
                return false;
            }

            return true;
        }
    }
}