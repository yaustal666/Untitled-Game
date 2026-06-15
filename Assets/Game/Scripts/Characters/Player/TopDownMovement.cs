using Reflex.Attributes;
using UnityEngine;

public class TopDownMovement : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private InputReader _inputReader;

    [SerializeField] private float _maxSpeed;

    private Rigidbody2D _rb;
    private Animator _anim;

    private bool isMoving = true;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        _rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        _rb.gravityScale = 0;
    }

    private void Start()
    {
        _player.ControlEnabled += EnableMovement;
    }

    private void OnDisable()
    {
        _player.ControlEnabled -= EnableMovement;
    }

    private void EnableMovement(bool enabled)
    {
        isMoving = enabled;
    }

    private void FixedUpdate()
    {
        if (isMoving)
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
    }

    private void Move()
    {
        _rb.linearVelocity = _maxSpeed * _inputReader.Direction;
    }

    public void SetEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}