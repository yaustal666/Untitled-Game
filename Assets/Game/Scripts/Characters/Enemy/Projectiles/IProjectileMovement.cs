using UnityEngine;

public interface IProjectileMovement
{
    void SetTarget(Transform target, Transform projectile);
    void Move();
}