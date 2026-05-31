using System;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public event Action<string, IDamagable> OnHit;

    [field: SerializeField] public string AttackName { get; private set; }
    [SerializeField] private List<Hitbox> hitboxList;

    private void OnEnable()
    {
        foreach (Hitbox hitbox in hitboxList)
        {
            hitbox.Hit += (target) => OnHit?.Invoke(AttackName, target);
        }
    }

    public void SetHitbox(int id)
    {
        hitboxList[id].SetEnabled(true);
    }

    public void ResetHitbox(int id)
    {
        hitboxList[id].SetEnabled(false);
    }

    public void ResetAllHitboxes()
    {
        foreach (var hitbox in hitboxList)
        {
            if (hitbox != null)
            {
                hitbox.SetEnabled(false);
            }
        }
    }
}