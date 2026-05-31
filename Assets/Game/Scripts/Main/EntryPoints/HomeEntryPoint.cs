using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Unity.Cinemachine;
using UnityEngine;

public class HomeEntryPoint : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private PlayerSpawner _playerSpawner;

    public Transform playerSpawnPoint;
    [SerializeField] private CinemachineCamera mainCamera;

    private void Start()
    {
        var sceneContainer = gameObject.scene.GetSceneContainer();

        var playerObject = _playerSpawner.SpawnPlayer(playerSpawnPoint.position);
        GameObjectInjector.InjectRecursive(playerObject, sceneContainer);
        mainCamera.Target.TrackingTarget = playerObject.GetComponent<Transform>();
    }
}