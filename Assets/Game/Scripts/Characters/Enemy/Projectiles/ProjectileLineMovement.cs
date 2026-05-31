using UnityEngine;

public class ProjectileLineMovement : IProjectileMovement
{
    private Rigidbody2D _rb;
    private Vector2 _direction;
    private float _speed;

    public ProjectileLineMovement(Projectile projectile)
    {
        _rb = projectile._rb;
        _speed = projectile._data.speed;
    }

    public void SetTarget(Transform target, Transform projectile)
    {
        _direction = (Vector2) (target.position - projectile.position).normalized;
    }

    public void Move()
    {
        _rb.linearVelocity = _direction * _speed;
    }
}