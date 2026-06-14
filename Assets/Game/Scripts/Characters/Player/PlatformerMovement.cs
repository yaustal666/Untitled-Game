using Reflex.Attributes;
using UnityEngine;

public class PlatformerMovement : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private InputReader _inputReader;

    [SerializeField] private float _maxSpeed;

    private Animator _anim;
    private Rigidbody2D _rb;
    private JumpAbility _jump;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _jump = GetComponent<JumpAbility>();
    }

    private void OnEnable()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.gravityScale = GameConstants.Platformer_DefaultGravity;
    }

    private void FixedUpdate()
    {
        Move();
        if (_rb.linearVelocity.magnitude > 0.1f)
        {
            _anim.SetBool("running", true);
        }
        else
        {
            _anim.SetBool("running", false);
        }
    }

    private void Move()
    {
        Vector2 direction = new Vector2(_inputReader.HorizontalDirection * _maxSpeed, _rb.linearVelocity.y);
        _rb.linearVelocity = direction;
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
        _jump.SetEnabled(enabled);
    }
}