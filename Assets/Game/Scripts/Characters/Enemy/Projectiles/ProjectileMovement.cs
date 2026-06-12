using UnityEngine;

public abstract class ProjectileMovement
{
    protected readonly Rigidbody2D _projectileRB;
    protected readonly Transform _projectileTransform;
    protected Vector2 _direction;

    protected float _speed;

    protected ProjectileMovement(Projectile projectile)
    {
        _projectileRB = projectile.RB;
        _projectileTransform = projectile.transform;
        _speed = projectile.Data.Speed;
    }

    public virtual void SetTarget(ProjectileTarget target) 
    {
        _direction = (target.TargetPosition - _projectileTransform.position).normalized;
    }

    public virtual void MoveToTarget()
    {

    }

    protected virtual void RotateProjectile(Vector2 velocity)
    {
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        _projectileTransform.rotation = Quaternion.Euler(0, 0, angle);
    }
}