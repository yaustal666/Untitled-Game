using UnityEngine;

public interface IDamagable
{
    public void TakeDamage(DamageData damageInfo);
}

public struct DamageData
{
    public float Damage;
    public Vector2 DamageSourcePosition;
}