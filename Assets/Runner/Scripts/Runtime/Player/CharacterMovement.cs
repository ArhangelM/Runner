using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Runner.Scripts.Runtime.Player
{
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private Rigidbody _rigidbody;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _speedRun = 10f;
        [Header("Jump Settings")]
        [SerializeField] private float _jumpForce = 5f;
        [SerializeField] private float _groundCheckDistance = 1.1f;
        [Header("Camera Settings")]
        [SerializeField] private float _sensitivity = 8f;
        [SerializeField] private Vector2 _pitchBounds = new Vector2(-80f, 80f);
        [Header("Effects")]
        [SerializeField] private GameObject _spawnPosition;
        [SerializeField] private GameObject _effect;

        private MyInputSystem _inputSystem;
        private MyInputSystem.CharacterActions _characterActions;
        private Vector2 _moveInput;

        private Camera _mainCamera;
        private float _cameraPitch;

        private bool _isRunning = false;

        private void Awake()
        {
            _mainCamera = Camera.main;
            Cursor.lockState = CursorLockMode.Locked;
            if (_mainCamera != null)
            {
                _mainCamera.transform.SetParent(transform);
                _mainCamera.transform.localPosition = new Vector3(0, 1, 0);
                _mainCamera.transform.localRotation = Quaternion.identity;
            }

            InitInputSystem();
        }

        private void OnEnable() => _inputSystem.Enable();

        private void FixedUpdate()
        {
            Moving();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + Vector3.down * _groundCheckDistance);
        }

        private void OnDisable() => _inputSystem.Disable();

        private void InitInputSystem()
        {
            _inputSystem = new MyInputSystem();
            _characterActions = _inputSystem.Character;

            _characterActions.Move.performed += Move;
            _characterActions.Jump.performed += Jump;
            _characterActions.Run.performed += Run;
            _characterActions.Look.performed += Look;
        }

        private void Look(InputAction.CallbackContext context)
        {
            var delta = context.ReadValue<Vector2>();
            var mouseX = delta.x * _sensitivity * Time.deltaTime;
            var mouseY = delta.y * _sensitivity * Time.deltaTime;

            transform.Rotate(Vector3.up * mouseX);

            _cameraPitch -= mouseY;
            _cameraPitch = Mathf.Clamp(_cameraPitch, _pitchBounds.x, _pitchBounds.y);

            _mainCamera.transform.localRotation = Quaternion.Euler(_cameraPitch, 0f, 0f);
        }

        private void Run(InputAction.CallbackContext context)
        {
            _isRunning = context.ReadValue<float>() > 0;
        }

        private void Move(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        private void Jump(InputAction.CallbackContext context)
        {
            if (Physics.Raycast(transform.position, Vector3.down, _groundCheckDistance))
            {
                _rigidbody.AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
                Instantiate(_effect, _spawnPosition.transform.position, _effect.transform.rotation, _spawnPosition.transform);
            }
        }

        private void Moving()
        {
            float speed = _isRunning ? _speedRun : _speed;
            var moveDir = new Vector3(_moveInput.x * speed, 0, _moveInput.y * speed);
            moveDir = transform.TransformDirection(moveDir);
            _rigidbody.linearVelocity = new Vector3(moveDir.x, _rigidbody.linearVelocity.y, moveDir.z);
        }
    }
}
