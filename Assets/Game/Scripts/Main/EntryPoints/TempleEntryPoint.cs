using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Unity.Cinemachine;
using UnityEngine;

public class TempleEntryPoint : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private PlayerSpawner _playerSpawner;

    public Transform playerSpawnPoint;
    [SerializeField] private CinemachineCamera mainCamera;

    private async void Start()
    {
        var sceneContainer = gameObject.scene.GetSceneContainer();

        var playerObject = _playerSpawner.SpawnPlayer(playerSpawnPoint.position);
        GameObjectInjector.InjectRecursive(playerObject, sceneContainer);

        await UniTask.Yield(PlayerLoopTiming.LastInitialization);

        _player.SetMode(PlayerMode.Platformer);

        mainCamera.Target.TrackingTarget = playerObject.GetComponent<Transform>();
    }
}