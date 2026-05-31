using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public class Projectile : MonoBehaviour
{
    public Rigidbody2D _rb { get; private set; }
    [SerializeField] private ProjectileView _view;

    public ProjectileData _data;
    private IObjectPool<Projectile> _originPool;

    private Transform _target;
    private bool _isLaunched = false;

    private CancellationTokenSource _lifetimeCts;

    private IProjectileMovement  _movement;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
       _view = GetComponentInChildren<ProjectileView>();
    }

    public void Initialize(ProjectileData data, IObjectPool<Projectile> pool)
    {
        _data = data;
        _originPool = pool;

        switch (data.movementType)
        {
            case ProjectileMovementType.Line:
                _movement = new ProjectileLineMovement(this);
                break;
            case ProjectileMovementType.Wave:
                break;
            case ProjectileMovementType.Homing:
                break;
        }
    }

    public void Launch(Transform target)
    {
        _target = target;

        _rb.linearVelocity = Vector2.zero;
        _rb.angularVelocity = 0f;

        _view.Initialize(_data);
        _isLaunched = true;

        StartLifetimeTimer();
        _movement.SetTarget(_target, transform);
    }

    private void FixedUpdate()
    {
        if (!_isLaunched) return;

        _movement.Move();

        //if (_data.movementType == MovementType.Wave)
        //{
        //    Vector2 forwardDir = transform.right;
        //    Vector2 perpendicular = new Vector2(-forwardDir.y, forwardDir.x);

        //    Vector2 baseVelocity = forwardDir * _data.baseSpeed;
        //    Vector2 waveVelocity = perpendicular * Mathf.Cos(_timeCounter * _data.customParameterA) * _data.customParameterA * _data.customParameterB;

        //    _rb.linearVelocity = baseVelocity + waveVelocity;
        //}
        //else if (_data.movementType == MovementType.Homing && _target != null)
        //{
        //    // customParameterA = Скорость поворота ракеты
        //    Vector2 direction = (Vector2)_target.position - _rb.position;
        //    direction.Normalize();

        //    float rotateAmount = Vector3.Cross(direction, transform.right).z;
        //    _rb.angularVelocity = -rotateAmount * _data.customParameterA;
        //    _rb.linearVelocity = transform.right * _data.baseSpeed;
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isLaunched) return;

        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<Hurtbox>(out var hurtbox))
            {
                hurtbox.Owner.TakeDamage(_data.baseDamage);
            }
            _view.PlayHitEffect(_data, transform.position);
            ReturnToPool();
        }
        else if (collision.CompareTag("Wall"))
        {
            _view.PlayHitEffect(_data, transform.position);
            ReturnToPool();
        }
    }

    private void StartLifetimeTimer()
    {
        CancelLifetimeTimer();
        _lifetimeCts = new CancellationTokenSource();
        ApplyLifetimeAsync(_data.lifetime, _lifetimeCts.Token).Forget();
    }

    private async UniTaskVoid ApplyLifetimeAsync(float duration, CancellationToken token)
    {
        bool isCanceled = await UniTask.WaitForSeconds(duration, cancellationToken: token).SuppressCancellationThrow();
        if (isCanceled || token.IsCancellationRequested) return;

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        CancelLifetimeTimer();
        _isLaunched = false;
        _originPool?.Release(this);
    }

    private void CancelLifetimeTimer()
    {
        if (_lifetimeCts != null)
        {
            _lifetimeCts.Cancel();
            _lifetimeCts.Dispose();
            _lifetimeCts = null;
        }
    }

    private void OnDestroy() => CancelLifetimeTimer();
}
