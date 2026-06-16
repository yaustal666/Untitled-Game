using UnityEngine;

public class PlatformerPathfinding : MonoBehaviour, IPathfinding
{
    public Vector2 Direction { get; private set; }
    public float DistanceToTarget { get; private set; }

    [SerializeField] private float movementSpeed = 3.5f;

    private Transform targetTransform;
    private bool isUpdating = false;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            DistanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        }

        if (!isUpdating) return;

        var direction = (Vector2)(targetTransform.position - transform.position).normalized;
        direction = new Vector2 (direction.x, 0);
        _rb.linearVelocity = direction * movementSpeed;
    }

    public void FollowTarget(Transform target)
    {
        targetTransform = target;
        if (!isUpdating) SetNavigationActive(true);
    }

    public void MoveToPoint(Vector3 point)
    {
        throw new System.NotImplementedException();
    }

    public void SetNavigationActive(bool active)
    {
        _rb.linearVelocity = Vector2.zero;
        isUpdating = active;
    }

    public void Stop()
    {
        isUpdating = false;
    }
}