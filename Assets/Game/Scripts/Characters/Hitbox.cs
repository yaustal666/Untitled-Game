using System;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public event Action<IDamagable> Hit;
    [SerializeField] private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Hurtbox>(out var hurtbox))
        {
            Hit?.Invoke(hurtbox.Owner);
        }
    }

    public void SetEnabled(bool enabled)
    {
        _collider.enabled = enabled;
    }
}