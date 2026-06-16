using Cysharp.Threading.Tasks;
using Reflex.Extensions;
using Reflex.Injectors;
using Unity.Cinemachine;
using UnityEngine;

public sealed class DungeonEntryPoint : EntryPoint
{
    [SerializeField] private GameObject dungeonPrefab;

    protected override async UniTask SetupScene()
    {
        var dungeonObject = Instantiate(dungeonPrefab);
        GameObjectInjector.InjectRecursive(dungeonObject, gameObject.scene.GetSceneContainer());
        var dungeon = dungeonObject.GetComponent<Dungeon>();
        dungeon._confiner = mainCamera.GetComponent<CinemachineConfiner2D>();

        await UniTask.WaitUntil(() => dungeon.Ready);

        var sceneContainer = gameObject.scene.GetSceneContainer();

        dungeon.ShowBlackScreen(1000).Forget();

        var playerObject = _playerSpawner.SpawnPlayer(dungeon.GetStartRoomPosition());
        GameObjectInjector.InjectRecursive(playerObject, sceneContainer);

        mainCamera.Target.TrackingTarget = playerObject.GetComponent<Transform>();
        _player.SetMode(PlayerMode.TopDown);

        dungeon.PlayerTransform = playerObject.GetComponent<Transform>();
    }
}