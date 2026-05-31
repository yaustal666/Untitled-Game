using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{
    public event Action PausePressed;
    public event Action InventoryPressed;
    public event Action UnPausePressed;

    public event Action InteractPressed;

    public event Action AttackPressed;
    public event Action JumpPressed;
    public event Action JumpCanceled;

    #region Dev
    //private InputAction _switchPlayerModeAction;
    //public event Action SwitchPlayerMode;
    #endregion

    private InputActionMap _playerMap;
    private InputActionMap _uiMap;

    private InputAction _moveAction;
    private InputAction _attackAction;
    private InputAction _jumpAction;
    private InputAction _pauseAction;
    private InputAction _inventoryAction;
    private InputAction _interactAction;


    private InputAction _unPauseAction;

    private Vector2 _direction;
    public Vector2 Direction => _direction;
    public float HorizontalDirection => _direction.x;
    public float VerticalDirection => _direction.y;
    //public Vector2 FourWayDirection => GetFourWayDirection();

    public bool IsJumpPressed => _jumpAction.IsPressed();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        _playerMap = InputSystem.actions.FindActionMap("Player");
        _uiMap = InputSystem.actions.FindActionMap("UI");

        _moveAction = _playerMap.FindAction(GameConstants.Input_MoveAction);
        _attackAction = _playerMap.FindAction(GameConstants.Input_AttackAction);
        _jumpAction = _playerMap.FindAction(GameConstants.Input_JumpAction);
        _pauseAction = _playerMap.FindAction("Pause");
        _inventoryAction = _playerMap.FindAction("Inventory");
        _interactAction = _playerMap.FindAction("Interact");

        _attackAction.performed += (InputAction.CallbackContext context) => AttackPressed?.Invoke();
        _jumpAction.performed += (InputAction.CallbackContext context) => JumpPressed?.Invoke();
        _jumpAction.canceled += (InputAction.CallbackContext context) => JumpCanceled?.Invoke();
        _pauseAction.performed += (InputAction.CallbackContext context) => PausePressed?.Invoke();
        _inventoryAction.performed += (InputAction.CallbackContext context) => InventoryPressed?.Invoke();
        _interactAction.performed += (InputAction.CallbackContext context) => InteractPressed?.Invoke();

        _unPauseAction = _uiMap.FindAction("UnPause");
        _unPauseAction.performed += (InputAction.CallbackContext context) => UnPausePressed?.Invoke();
    }

    public void EnablePlayerActionMap(bool enabled)
    {
        if (enabled)
        {
            _playerMap.Enable();
        }
        else
        {
            _playerMap.Disable();
        }
    }

    public void EnableUIActionMap(bool enabled)
    {
        if (enabled)
        {
            _uiMap.Enable();
        }
        else
        {
            _uiMap.Disable();
        }
    }

    private void Update()
    {
        _direction = _moveAction.ReadValue<Vector2>();
    }

    //public Vector2 GetFourWayDirection()
    //{
    //    Vector2 fourWayDirection = Vector2.zero;

    //    if (Mathf.Abs(_direction.x) > Mathf.Abs(_direction.y))
    //    {
    //        fourWayDirection = new Vector2(Mathf.Sign(_direction.x), 0);
    //    }
    //    else if (Mathf.Abs(_direction.y) > 0)
    //    {
    //        fourWayDirection = new Vector2(0, Mathf.Sign(_direction.y));
    //    }

    //    return fourWayDirection;
    //}
}