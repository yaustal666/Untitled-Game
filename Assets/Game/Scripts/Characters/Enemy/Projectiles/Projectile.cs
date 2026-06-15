using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

public struct ProjectileTarget
{
    public Transform TargetTransform;
    public Vector3 TargetPosition;
}

public class Projectile : MonoBehaviour
{
    public Rigidbody2D RB { get; private set; }
    public ProjectileData Data { get; private set; }

    private SpriteRenderer _sr;
    private Animation _animation;
    private IObjectPool<Projectile> _originPool;
    private ProjectileMovement _movement;
    private CancellationTokenSource _lifetimeCts;
    private bool _isLaunched = false;

    public void Initialize(ProjectileData data, IObjectPool<Projectile> pool)
    {
        Data = data;
        _originPool = pool;
        _sr.sprite = data.Sprite;
        _animation.clip = data.AnimationClip;

        _movement = data.MovementType switch
        {
            ProjectileMovementType.Line => new ProjectileLineMovement(this),
            ProjectileMovementType.Wave => new ProjectileWaveMovement(this),
            ProjectileMovementType.Homing => new ProjectileHomingMovement(this),
            _ => new ProjectileLineMovement(this)
        };
    }

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        _sr = GetComponent<SpriteRenderer>();
        _animation = GetComponent<Animation>();
    }

    private void OnDestroy() => CancelLifetimeTimer();

    public void Launch(ProjectileTarget target)
    {
        _isLaunched = true;
        StartLifetimeTimer();
        _movement.SetTarget(target);
        _animation.Play();
    }

    private void FixedUpdate()
    {
        if (!_isLaunched) return;
        _movement.MoveToTarget();
    }

    // TODO: remake physics layers and change it
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isLaunched) return;

        if (collision.CompareTag("Player"))
        {
            if (collision.TryGetComponent<Hurtbox>(out var hurtbox))
            {
                DamageData damageInfo = new DamageData
                {
                    Damage = Data.BaseDamage,
                    DamageSourcePosition = transform.position,
                };
                hurtbox.Owner.TakeDamage(damageInfo);
            }
            PlayHitEffect(Data, transform.position);
            ReturnToPool();
        }
        else if (collision.CompareTag("Wall"))
        {
            PlayHitEffect(Data, transform.position);
            ReturnToPool();
        }
    }

    private void StartLifetimeTimer()
    {
        CancelLifetimeTimer();
        _lifetimeCts = new CancellationTokenSource();
        ApplyLifetimeAsync(Data.Lifetime, _lifetimeCts.Token).Forget();
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

    public void PlayHitEffect(ProjectileData data, Vector3 position)
    {
        if (data.HitEffectPrefab != null)
        {
            Instantiate(data.HitEffectPrefab, position, Quaternion.identity);
        }
    }

    public void PlayFireEffect(ProjectileData data, Vector3 position)
    {
        if (data.FireEffectPrefab != null)
        {
            Instantiate(data.FireEffectPrefab, position, Quaternion.identity);
        }
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
}
