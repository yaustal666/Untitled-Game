using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

public class PlayerView : MonoBehaviour, IDamagable
{
    [Inject] private Player _player;
    [Inject] private InputReader _inputReader;

    [SerializeField] private Transform _attackPoint;

    private SpriteRenderer _sr;
    private Rigidbody2D _rb;
    private Animator _anim;
    private Hurtbox _hurtbox;

    private Material _mat;
    private float damageFlashTime = 0.25f;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _hurtbox = GetComponentInChildren<Hurtbox>();
        _hurtbox.Owner = this;
        _mat = _sr.material;
    }

    private void Start()
    {
        _player.PlayerDead += OnDeath;
        _player.PlayerGotHit += OnHit;
    }

    private void OnDeath()
    {
        _anim.SetTrigger("death");
    }

    public void OnDeathAnimationEnd()
    {
        Destroy(gameObject);
    }

    private void OnHit()
    {
        DamageFlash().Forget();
    }

    private void Update()
    {
        Flip();
    }

    private void Flip()
    {
        if (_inputReader.HorizontalDirection < 0)
        {
            _sr.flipX = true;
            _attackPoint.localPosition = new Vector3(-1, 0, 0);
            _attackPoint.localRotation = Quaternion.Euler(0, 180, 0);
        }

        if (_inputReader.HorizontalDirection > 0)
        {
            _sr.flipX = false;
            _attackPoint.localPosition = new Vector3(1, 0, 0);
            _attackPoint.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    public void TakeDamage(DamageData damageInfo)
    {
        _player.TakeDamage(damageInfo);
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

        _player.EnableControl(false);
        _rb.linearVelocity = knockbackDirection * 10;
        DelayedEnableNavigation().Forget();
    }

    private async UniTask DelayedEnableNavigation()
    {
        await UniTask.Delay(200);
        _rb.linearVelocity = Vector2.zero;
        _player.EnableControl(true);
    }

    private void OnDisable()
    {
        _player.PlayerDead -= OnDeath;
        _player.PlayerGotHit -= OnHit;
    }
}