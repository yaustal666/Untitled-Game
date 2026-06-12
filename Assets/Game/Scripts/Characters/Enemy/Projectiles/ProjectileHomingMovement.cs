using UnityEngine;

public sealed class ProjectileHomingMovement : ProjectileMovement
{
    private Transform _target;
    private float _rotationSpeed;

    public ProjectileHomingMovement(Projectile projectile) : base(projectile)
    {
        _rotationSpeed = projectile.Data.param1;
    }

    public override void SetTarget(ProjectileTarget target)
    {
        _target = target.TargetTransform;
        _direction = (Vector2)(target.TargetPosition - _projectileTransform.position).normalized;
    }

    public override void MoveToTarget()
    {
        Vector2 directionToTarget = (_target.position - _projectileTransform.position).normalized;
        float angleDifference = Vector2.SignedAngle(_direction, directionToTarget);

        float maxRotation = _rotationSpeed * Time.deltaTime;
        float rotationAmount = Mathf.Clamp(angleDifference, -maxRotation, maxRotation);
        _direction = Quaternion.Euler(0, 0, rotationAmount) * _direction;

        _projectileRB.linearVelocity = _direction * _speed;

        RotateProjectile(_direction);
    }
}