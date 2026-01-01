using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]private float _moveSpeed;
    [SerializeField] private float _walkSpeed;
    [SerializeField] private float _runSpeed;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _cameraLimit;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    private float _verticalVelocity;
    public bool isRunning;

    [Header("Water Movement")]
    [SerializeField] private float _waterGravity;
    [SerializeField] private float _swimUpSpeed;
    [HideInInspector] public bool isInWater = false;


    [Header("References")]
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _handAnimator;

    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private InputActionReference _jumpAction;
    [SerializeField] private InputActionReference _runAction;
    public bool IsJumpPressed => _jumpAction != null && _jumpAction.action.IsPressed();


    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _cameraPitch;


    private NeedsManager _needsManager;

    private SaveManager _saveManager;
    private bool _canMove = true;

    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Start()
    {
        _needsManager = NeedsManager.instance;
        _saveManager = SaveManager.instance;
        _saveManager.OnSaveRequested += Save;
        _saveManager.OnLoadCompleted += Load;
    }

    private void OnDisable()
    {
        _saveManager.OnSaveRequested -= Save;
        _saveManager.OnLoadCompleted -= Load;
    }

    private void Save()
    {
        _saveManager.playerInfo.position = transform.position;
    }
    private void Load()
    {
        _canMove = false;
        transform.position = _saveManager.playerInfo.position;
        Invoke(nameof(CanMove), 0.1f);
    }

    private void CanMove()
    {
        _canMove = true;
    }

    private void Update()
    {
        _moveInput = _moveAction.action.ReadValue<Vector2>();
        _lookInput = _lookAction.action.ReadValue<Vector2>();
        HandleMovement(_moveInput);
        HandleLook(_lookInput);
    }

    private void HandleMovement(Vector2 moveInput)
    {
        if (_canMove)
        {
            Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
            direction = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f) * direction;
            direction.y = 0f;
            direction.Normalize();
            if (_characterController.isGrounded)
            {
                _verticalVelocity = -1f;
                if (_jumpAction.action.triggered)
                {
                    _verticalVelocity = _jumpForce;
                }
            }
            else
            {
                if (!isInWater)
                {
                    _verticalVelocity += _gravity * Time.deltaTime;
                }
                else
                {
                    _verticalVelocity -= _waterGravity * Time.deltaTime;
                    if (IsJumpPressed)
                    {
                        _verticalVelocity = Mathf.Lerp(_verticalVelocity, _swimUpSpeed, 5f * Time.deltaTime);
                    }
                    _verticalVelocity = Mathf.Max(_verticalVelocity, -2f);
                }
            }

            if (_runAction.action.IsPressed() && _needsManager.Energy.CanRun)
            {
                _moveSpeed = _runSpeed;
                _needsManager.Running();
                isRunning = true;
            }
            else
            {
                _moveSpeed = _walkSpeed;
                isRunning = false;
            }

            Vector3 velocity = direction * _moveSpeed;
            velocity.y = _verticalVelocity;
            _characterController.Move(velocity * Time.deltaTime);
            _handAnimator.SetFloat("Velocity", velocity.magnitude);
        }
        else
        {
            _handAnimator.SetFloat("Velocity", 0);
        }
    }

    private void HandleLook(Vector2 lookInput)
    {
        transform.Rotate(Vector3.up * lookInput.x * _lookSpeed * Time.deltaTime);

        _cameraPitch -= lookInput.y * _lookSpeed * Time.deltaTime;  
        _cameraPitch = Mathf.Clamp(_cameraPitch, -_cameraLimit, _cameraLimit);
        _cameraTransform.localEulerAngles = new Vector3(_cameraPitch, 0f, 0f);
    }
}
