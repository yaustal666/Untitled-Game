using System;

public class Health : IDisposable
{
    public event Action Dead;
    private CharacterStat _maxHealth;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public Health(PlayerStats stats)
    {
        MaxHealth = stats.GetBaseValue(StatType.MaxHealth);
        CurrentHealth = MaxHealth;
        _maxHealth = stats.GetStat(StatType.MaxHealth);
        _maxHealth.StatChanged += MaxHealthUpgrade;
    }

    public Health (float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void Dispose()
    {
        if (_maxHealth != null)
        {
            _maxHealth.StatChanged -= MaxHealthUpgrade;
        }
    }

    public float HealthPercentage => CurrentHealth / MaxHealth;

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        if (CurrentHealth <= 0)
        {
            Dead?.Invoke();
        }

        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;
    }

    public void Heal(float heal)
    {
        CurrentHealth += heal;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
    }

    public void MaxHealthUpgrade(float value)
    {
        MaxHealth = value;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
    }
}