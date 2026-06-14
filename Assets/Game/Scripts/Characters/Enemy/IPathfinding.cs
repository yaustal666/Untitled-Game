using UnityEngine;

public interface IPathfinding
{
    void SetNavigationActive(bool active);
    void MoveToPoint(Vector3 point);
    void FollowTarget(Transform target);
    void Stop();

    Vector2 Direction { get; }
    float DistanceToTarget {  get; }
}