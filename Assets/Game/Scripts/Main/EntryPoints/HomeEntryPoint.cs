using Cysharp.Threading.Tasks;
using Reflex.Extensions;
using Reflex.Injectors;
using UnityEngine;

public sealed class HomeEntryPoint : EntryPoint
{
    protected override async UniTask SetupScene()
    {
        var sceneContainer = gameObject.scene.GetSceneContainer();

        var playerObject = _playerSpawner.SpawnPlayer(playerSpawnPoint.position);
        GameObjectInjector.InjectRecursive(playerObject, sceneContainer);
        mainCamera.Target.TrackingTarget = playerObject.GetComponent<Transform>();
    }
}