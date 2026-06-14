using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.AI;

public enum EnemyState
{
    Idle,
    Chasing,
    Attack,
    Dead
}

public class Slime : Enemy
{
    [Inject] private GameEvents _events;
    [SerializeField] private LootTable _lootTable;
    private Rigidbody2D _rb;
    [SerializeField] private float knockbackForce;
    [SerializeField] private int knockbackDelay;
    private float damageFlashTime = 0.25f;
    private Material _mat;

    private Health _health;

    [SerializeField] private RangedAttack _spitAttack;
    [SerializeField] PlayerDetector _detector;

    private IPathfinding _pathfinding;
    private Transform _target;

    private EnemyState _currentState;

    private float _spitCooldown = 2f;
    private float _lastSpitTime = 0f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _pathfinding = GetComponent<IPathfinding>();
        _detector.PlayerDetected += OnPlayerDetected;
        _currentState = EnemyState.Idle;

        _spitAttack.Initialize(this);

        _mat = _sr.material;

        _health = new Health(50f);
        _health.Dead += OnDead;
    }

    private void OnDead()
    {
        _anim.SetTrigger("death");
        _currentState = EnemyState.Dead;

        var message = new EnemyDeathMessage();
        message.LootTable = _lootTable;
        _events.Publish(message);

    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if (_detector != null)
        {
            _detector.PlayerDetected -= OnPlayerDetected;
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Idle:
                break;

            case EnemyState.Chasing:
                HandleChasingState();
                break;

            case EnemyState.Attack:
                HandleAttackingState();
                break;

            case EnemyState.Dead:
                break;
        }
    }

    private void OnPlayerDetected(Transform playerTransform)
    {
        _target = playerTransform;
        _detector.enabled = false;
        EnterChasingState();
    }

    public void SpitAttack()
    {
        _spitAttack.ExecuteAttack(_target);
        _lastSpitTime = Time.time;
    }

    private void EnterChasingState()
    {
        _anim.SetBool("Walking", true);
        _currentState = EnemyState.Chasing;
        _pathfinding.SetNavigationActive(true);
        _pathfinding.FollowTarget(_target);
    }

    private void HandleChasingState()
    {
        if (_pathfinding.DistanceToTarget < 8f && _pathfinding.DistanceToTarget > 2f)
        {
            EnterAttackState();
        }
    }

    private void EnterAttackState()
    {
        _pathfinding.SetNavigationActive(false);
        _currentState = EnemyState.Attack;
    }

    private void HandleAttackingState()
    {
        if (Time.time - _lastSpitTime > _spitCooldown)
        {
            _lastSpitTime = Time.time;
            _anim.SetTrigger("attack");
        }

        if (_pathfinding.DistanceToTarget > 10f)
        {
            EnterChasingState();
        }
    }

    public override void TakeDamage(DamageData damageInfo)
    {
        if (_currentState == EnemyState.Dead) return;
        if (_health.CurrentHealth <= damageInfo.Damage)
        {
            _health.TakeDamage(damageInfo.Damage);
            DamageFlash().Forget();
            return;
        }
        _anim.SetTrigger("damage");
        _health.TakeDamage(damageInfo.Damage);
        DamageFlash().Forget();
        ApplyKnockback(damageInfo);
    }

    private async UniTask DamageFlash()
    {
        _mat.SetFloat("_FlashPower", 1f);
        await UniTask.WaitForSeconds(damageFlashTime);
        _mat.SetFloat("_FlashPower", 0f);
    }

    private void ApplyKnockback(DamageData damageInfo)
    {
        Vector2 myPosition = transform.position;
        Vector2 knockbackDirection = (myPosition - damageInfo.DamageSourcePosition).normalized;

        _pathfinding.SetNavigationActive(false);
        _rb.linearVelocity = knockbackDirection * knockbackForce;
        DelayedEnableNavigation().Forget();
    }

    private async UniTask DelayedEnableNavigation()
    {
        await UniTask.Delay(knockbackDelay);
        _rb.linearVelocity = Vector2.zero;
        if (_currentState != EnemyState.Dead && _target != null)
        {
            _pathfinding.SetNavigationActive(true);
            _pathfinding.FollowTarget(_target);
        }
        else if (_currentState != EnemyState.Dead && _target == null)
        {
            _pathfinding.SetNavigationActive(true);
        }
    }
}
