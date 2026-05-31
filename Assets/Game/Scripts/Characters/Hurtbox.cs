using UnityEngine;

public class Hurtbox : MonoBehaviour
{
    private Collider2D _collider;

    public IDamagable Owner;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
        Owner = GetComponentInParent<IDamagable>();
    }

    public void SetEnabled(bool enabled)
    {
        _collider.enabled = enabled;
    }
}