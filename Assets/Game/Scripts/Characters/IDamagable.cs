public interface IDamagable
{
    public void TakeDamage(DamageData damageInfo);
}

public struct DamageData
{
    public float Damage;
}