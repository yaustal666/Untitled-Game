using UnityEngine;

public abstract class Enemy : MonoBehaviour, IDamagable
{
    protected SpriteRenderer _sr;
    protected Animator _anim;
    protected Hurtbox _hurtbox;

    protected void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
        _anim = GetComponent<Animator>();
        _hurtbox = GetComponentInChildren<Hurtbox>();
        _hurtbox.Owner = this;
    }

    protected virtual void OnDeath()
    {
        //SendLootToPlayer();
    }

    public virtual void TakeDamage(DamageData damageInfo)
    {
    }
}