using System;
using UnityEngine;

public enum PlayerMode
{
    TopDown,
    Platformer
}

public enum PlayerStatus
{
    Jump,
    Attack,
    Dead
}

public class Player : IDisposable
{
    public event Action PlayerDead;
    public event Action PlayerGotHit;
    public event Action<PlayerMode> PlayerChangedMode;

    public Health Health { get; private set; }
    public PlayerStats Stats { get; private set; }
    public Inventory Inventory { get; private set; }
    public PlayerMode CurrentMode { get; private set; }
    public PlayerStatus CurrentStatus { get; private set; }

    public Player(ISaveRegistry saveRegistry, GameEvents eventBus)
    {
        Stats = new PlayerStats();
        Health = new Health(Stats);
        Inventory = new Inventory(saveRegistry, eventBus);
        Health.Dead += Die;
        CurrentMode = PlayerMode.TopDown;
    }

    public void TakeDamage(float damage)
    {
        if (CurrentStatus == PlayerStatus.Dead) return;
        Health.TakeDamage(damage);
        PlayerGotHit?.Invoke();
    }

    public void SetMode(PlayerMode mode)
    {
        Debug.Log("SWITCH MODE PLAYER");
        CurrentMode = mode;
        PlayerChangedMode?.Invoke(mode);
    }

    private void Die()
    {
        PlayerDead?.Invoke();
    }

    public void RespawnPlayer()
    {

    }

    public void Dispose()
    {
    }
}