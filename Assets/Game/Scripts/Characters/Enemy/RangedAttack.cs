using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using UnityEngine.Pool;

[Serializable]
public class RangedAttack
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;

    [SerializeField] private ProjectileData projectileData;

    [SerializeField] private int shotsInQueue = 3;
    [SerializeField] private float delayBetweenShots = 0.15f;

    [SerializeField] private int defaultPoolCapacity = 5;
    [SerializeField] private int maxPoolSize = 20;

    [SerializeField] private bool isTracking = false;

    private IObjectPool<Projectile> _localPool;
    private CancellationTokenSource _queueCts;

    public void Initialize(MonoBehaviour mb)
    {
        _localPool = new ObjectPool<Projectile>(
            createFunc: () => { 
                var projectile = UnityEngine.Object.Instantiate(projectilePrefab, mb.transform.position, Quaternion.identity).GetComponent<Projectile>();
                projectile.Initialize(projectileData, _localPool);
                return projectile;
            },
            actionOnGet: p => p.gameObject.SetActive(true),
            actionOnRelease: p => p.gameObject.SetActive(false),
            actionOnDestroy: p => UnityEngine.Object.Destroy(p.gameObject),
            collectionCheck: false,
            defaultCapacity: defaultPoolCapacity,
            maxSize: maxPoolSize
        );
    }

    public void ExecuteAttack(Transform target)
    {
        CancelCurrentQueue();
        _queueCts = new CancellationTokenSource();
        ExecuteBurstAttackAsync(target, _queueCts.Token).Forget();
    }

    private async UniTaskVoid ExecuteBurstAttackAsync(Transform target, CancellationToken token)
    {
        var currentTarget = target;
        if (!isTracking)
        {
            GameObject staticPoint = new GameObject("Static_Target");
            staticPoint.transform.position = target.position;
            currentTarget = staticPoint.transform;
        }

        for (int i = 0; i < shotsInQueue; i++)
        {
            SpawnSingleProjectile(currentTarget);

            bool isCanceled = await UniTask.WaitForSeconds(delayBetweenShots, cancellationToken: token).SuppressCancellationThrow();
            if (isCanceled) return;
        }

        if (!isTracking)
        {
            GameObject.Destroy(currentTarget.gameObject);
        }
    }

    private void SpawnSingleProjectile(Transform target)
    {
        Vector2 direction = (target.position - firePoint.position).normalized;

        Projectile bullet = _localPool.Get();
        bullet.transform.position = firePoint.position;

        bullet.Launch(target);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void CancelCurrentQueue()
    {
        if (_queueCts != null)
        {
            _queueCts.Cancel();
            _queueCts.Dispose();
            _queueCts = null;
        }
    }

    private void OnDestroy()
    {
        CancelCurrentQueue();
        _localPool?.Clear();
    }
}
