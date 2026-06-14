using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TopDownPathfinding : MonoBehaviour, IPathfinding
{
    public Vector2 Direction { get; private set; }
    public float DistanceToTarget { get; private set; }

    [SerializeField] private float movementSpeed = 3.5f;
    [SerializeField] private bool useWandering = false;
    [SerializeField] private float waveFrequency = 3f;
    [SerializeField] private float waveMagnitude = 1.5f;

    private NavMeshAgent agent;
    private Transform targetTransform;
    private Vector3 staticTargetPosition;
    private bool hasStaticTarget = false;
    private bool isUpdating = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updateRotation = false;
        agent.updateUpAxis = false;

        agent.speed = movementSpeed;
    }

    private void Update()
    {
        if (targetTransform != null)
        {
            DistanceToTarget = Vector3.Distance(transform.position, targetTransform.position);
        }

        if (!isUpdating) return;

        Vector3 finalDestination = Vector3.zero;

        if (targetTransform != null)
        {
            finalDestination = targetTransform.position;
        }
        else if (hasStaticTarget)
        {
            finalDestination = staticTargetPosition;
        }
        else
        {
            Stop();
            return;
        }

        if (useWandering)
        {
            finalDestination = ApplyWaveOffset(finalDestination);
        }

        agent.SetDestination(finalDestination);

        Direction = (Vector2)agent.velocity.normalized;
    }

    public void SetNavigationActive(bool active)
    {
        isUpdating = active;
        agent.enabled = active;
    }

    public void MoveToPoint(Vector3 point)
    {
        targetTransform = null;
        staticTargetPosition = point;
        hasStaticTarget = true;

        if (!isUpdating) SetNavigationActive(true);
    }

    public void FollowTarget(Transform target)
    {
        hasStaticTarget = false;
        targetTransform = target;

        if (!isUpdating) SetNavigationActive(true);
    }

    public void Stop()
    {
        targetTransform = null;
        hasStaticTarget = false;
        if (agent.enabled && agent.hasPath)
        {
            agent.ResetPath();
        }
    }

    public void ToggleWandering(bool enable)
    {
        useWandering = enable;
    }

    private Vector3 ApplyWaveOffset(Vector3 baseDestination)
    {
        Vector3 direction = (baseDestination - transform.position).normalized;

        if (direction == Vector3.zero) return baseDestination;

        Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);

        Vector3 waveOffset = perpendicular * Mathf.Sin(Time.time * waveFrequency) * waveMagnitude;

        return baseDestination + waveOffset;
    }
}
