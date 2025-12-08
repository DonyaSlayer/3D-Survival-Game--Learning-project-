using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField]private float _moveSpeed;
    [SerializeField] private float _lookSpeed;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private float _cameraLimit;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravity;
    private float _verticalVelocity;


    [Header("References")]
    [SerializeField] private CharacterController _characterController;

    [Header("Input")]
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private InputActionReference _moveAction;
    [SerializeField] private InputActionReference _lookAction;
    [SerializeField] private InputActionReference _jumpAction;


    private Vector2 _moveInput;
    private Vector2 _lookInput;
    private float _cameraPitch;


    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
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
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        direction = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y,0f) * direction;
        direction.y = 0f;
        direction.Normalize();
        if(_characterController.isGrounded)
        {
            _verticalVelocity = -1f;
            if (_jumpAction.action.triggered)
            {
                _verticalVelocity = _jumpForce;
            }
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }
        
        Vector3 velocity = direction * _moveSpeed;
        velocity.y = _verticalVelocity;
        _characterController.Move(velocity * Time.deltaTime);


    }

    private void HandleLook(Vector2 lookInput)
    {
        transform.Rotate(Vector3.up * lookInput.x * _lookSpeed * Time.deltaTime);

        _cameraPitch -= lookInput.y * _lookSpeed * Time.deltaTime;  
        _cameraPitch = Mathf.Clamp(_cameraPitch, -_cameraLimit, _cameraLimit);
        _cameraTransform.localEulerAngles = new Vector3(_cameraPitch, 0f, 0f);
    }
}
