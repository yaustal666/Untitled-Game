using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Unity.Cinemachine;
using UnityEngine;

public abstract class EntryPoint : MonoBehaviour
{
    [Inject] protected Player _player;
    [Inject] protected PlayerSpawner _playerSpawner;

    [SerializeField] protected Transform playerSpawnPoint;
    [SerializeField] protected CinemachineCamera mainCamera;

    protected async void Start()
    {
        await SetupScene();
    }

    protected virtual async UniTask SetupScene()
    {
        await UniTask.Yield();
    }
}