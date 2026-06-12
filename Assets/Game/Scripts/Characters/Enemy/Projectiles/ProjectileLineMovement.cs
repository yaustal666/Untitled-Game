public sealed class ProjectileLineMovement : ProjectileMovement
{
    public ProjectileLineMovement(Projectile projectile) : base(projectile)
    {
    }

    public override void MoveToTarget()
    {
        _projectileRB.linearVelocity = _direction * _speed;
    }
}