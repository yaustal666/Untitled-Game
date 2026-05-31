using UnityEngine;

public class PlatformerPathfinding : MonoBehaviour, IPathfinding
{
    public Vector2 Direction { get; private set; }
    public float DistanceToTarget { get; private set; }

    public void FollowTarget(Transform target)
    {
        throw new System.NotImplementedException();
    }

    public void MoveToPoint(Vector3 point)
    {
        throw new System.NotImplementedException();
    }

    public void SetNavigationActive(bool active)
    {
        throw new System.NotImplementedException();
    }

    public void Stop()
    {
        throw new System.NotImplementedException();
    }
}