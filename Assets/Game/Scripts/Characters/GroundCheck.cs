using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [Header("Ground Detection")]
    [SerializeField] private LayerMask _whatIsGround;
    [SerializeField] private float _groundCheckRadius = 0.2f;
    [SerializeField] private Transform _groundCheckPoint;

    private bool _isGrounded;
    private float _lastGroundedTime;
    private float _lastCheckTime;
    private float _checkCooldown = 0.1f;

    public bool IsGrounded
    {
        get
        {
            if (Time.time - _lastCheckTime >= _checkCooldown)
            {
                UpdateGroundCheck();
            }
            return _isGrounded;
        }
    }

    public float LastGroundedTime => _lastGroundedTime;

    private void UpdateGroundCheck()
    {
        bool wasGrounded = _isGrounded;
        _isGrounded = Physics2D.Raycast(_groundCheckPoint.position, Vector2.down, _groundCheckRadius, _whatIsGround);
        _lastCheckTime = Time.time;

        if (_isGrounded)
        {
            _lastGroundedTime = Time.time;
        }
    }

    public void ForceGroundCheck()
    {
        UpdateGroundCheck();
    }
}