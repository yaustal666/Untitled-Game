using Reflex.Attributes;
using UnityEngine;

public class JumpAbility : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private InputReader _inputReader;

    [SerializeField] private Rigidbody2D _rb;

    [Header("Jump Physics")]
    [SerializeField] private float _jumpForce = 10f;
    [SerializeField] private float _jumpHoldForce = 5f;
    [SerializeField] private float _jumpCutMultiplier = 0.1f;

    [Header("Jump Timing")]
    [SerializeField] private float _coyoteTime = 0.1f;
    [SerializeField] private float _jumpBufferTime = 0.1f;

    [Header("Air Jumps")]
    [SerializeField] private int _maxAirJumps = 1;
    [SerializeField] private float _airJumpForce = 10f;

    private GroundCheck _groundCheck;

    private float _lastJumpPressedTime;
    private int _airJumpsRemaining;
    private bool _isJumping;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    private void OnEnable()
    {
        _inputReader.JumpPressed += OnJumpPressed;
        _inputReader.JumpCanceled += JumpCut;
    }

    private void OnDisable()
    {
        _inputReader.JumpPressed -= OnJumpPressed;
        _inputReader.JumpCanceled -= JumpCut;
    }

    private void Update()
    {
        HandleJumpTimers();
    }

    private void FixedUpdate()
    {
        HandleJump();
    }

    private void OnJumpPressed() {
        _lastJumpPressedTime = _jumpBufferTime;
    }

    private void HandleJumpTimers()
    {
        if (_lastJumpPressedTime > 0)
        {
            _lastJumpPressedTime -= Time.deltaTime;
        }
    }

    private void HandleJump()
    {
        bool canUseCoyoteTime = _groundCheck.IsGrounded || Time.time < _groundCheck.LastGroundedTime + _coyoteTime;
        bool hasJumpBuffer = _lastJumpPressedTime > 0f;

        if (canUseCoyoteTime && hasJumpBuffer)
        {
            ExecuteJump(_jumpForce);
            _lastJumpPressedTime = 0f;
            _airJumpsRemaining = _maxAirJumps;
            return;
        }

        if (!_groundCheck.IsGrounded && hasJumpBuffer && _airJumpsRemaining > 0)
        {
            ExecuteJump(_jumpForce);
            _lastJumpPressedTime = 0f;
            _airJumpsRemaining--;
            return;
        }

        if (_isJumping && _inputReader.IsJumpPressed && _rb.linearVelocity.y > 0)
        {
            _rb.AddForce(Vector2.up * _jumpHoldForce, ForceMode2D.Force);
        }

        _isJumping = !_groundCheck.IsGrounded && _rb.linearVelocity.y > 0;
    }

    private void ExecuteJump(float force)
    {
        _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, 0f);
        _rb.AddForce(Vector2.up * force, ForceMode2D.Impulse);
        _isJumping = true;
    }

    private void JumpCut()
    {
        if (_rb.linearVelocity.y > 0)
        {
            _rb.linearVelocity = new Vector2(_rb.linearVelocity.x, _rb.linearVelocity.y * _jumpCutMultiplier);
        }
    }
    public void SetEnabled(bool enabled)
    {
        if (enabled)
        {
            this.enabled = true;
            _groundCheck.enabled = true;
        }
        else
        {
            this.enabled = true;
            _groundCheck.enabled = true;
        }
    }
}