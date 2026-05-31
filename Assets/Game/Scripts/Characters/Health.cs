using System;

public class Health
{
    public event Action Dead;

    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public Health(PlayerStats stats)
    {
        MaxHealth = stats.GetStatValue(StatType.MaxHealth);
        CurrentHealth = MaxHealth;
    }

    public Health (float maxHealth)
    {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
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

    public void MaxHealthUpgrade(float percentage)
    {
        MaxHealth *= (1 + percentage);
        CurrentHealth *= (1 - percentage);

        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
    }
}