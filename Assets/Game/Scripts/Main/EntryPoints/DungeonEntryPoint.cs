using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using Reflex.Extensions;
using Reflex.Injectors;
using Unity.Cinemachine;
using UnityEngine;

public class DungeonEntryPoint : MonoBehaviour
{
    [Inject] private Player _player;
    [Inject] private PlayerSpawner _playerSpawner;

    [SerializeField] private GameObject dungeonPrefab;
    [SerializeField] private CinemachineCamera mainCamera;

    private async void Start()
    {
        await SetupScene();
    }

    private async UniTask SetupScene()
    {
        var dungeonObject = Instantiate(dungeonPrefab);
        GameObjectInjector.InjectRecursive(dungeonObject, gameObject.scene.GetSceneContainer());
        var dungeon = dungeonObject.GetComponent<Dungeon>();
        dungeon._confiner = mainCamera.GetComponent<CinemachineConfiner2D>();

        await UniTask.WaitUntil(() => dungeon.Ready);

        var sceneContainer = gameObject.scene.GetSceneContainer();

        var playerObject = _playerSpawner.SpawnPlayer(dungeon.GetStartRoomPosition());
        GameObjectInjector.InjectRecursive(playerObject, sceneContainer);

        mainCamera.Target.TrackingTarget = playerObject.GetComponent<Transform>();
        _player.SetMode(PlayerMode.TopDown);

        dungeon.PlayerTransform = playerObject.GetComponent<Transform>();
    }
}