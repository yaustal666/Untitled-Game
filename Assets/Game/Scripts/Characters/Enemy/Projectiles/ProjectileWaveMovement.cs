using UnityEngine;

public sealed class ProjectileWaveMovement : ProjectileMovement
{
    private float _waveFrequency;
    private float _waveMagnitude;

    public ProjectileWaveMovement(Projectile projectile) : base(projectile)
    {
        _waveFrequency = projectile.Data.param1;
        _waveMagnitude = projectile.Data.param2;
    }

    public override void MoveToTarget()
    {
        Vector2 perpendicular = new Vector2(-_direction.y, _direction.x);

        Vector2 baseVelocity = _direction * _speed;
        Vector2 waveVelocity = perpendicular * Mathf.Cos(Time.time * _waveFrequency) * _waveMagnitude;
        Vector2 velocity = baseVelocity + waveVelocity;
        _projectileRB.linearVelocity = velocity;
        RotateProjectile(velocity);
    }
}