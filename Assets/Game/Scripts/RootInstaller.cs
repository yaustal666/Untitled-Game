using Reflex.Core;
using UnityEngine;
using Lifetime = Reflex.Enums.Lifetime;
using Resolution = Reflex.Enums.Resolution;

public class RootInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private GameObject _inputReader;
    [SerializeField] private GameSettings _gameSettings;

    public void InstallBindings(ContainerBuilder containerBuilder)
    {
        containerBuilder.OnContainerBuilt += StartGame;

        var inputReader = Instantiate(_inputReader).GetComponent<InputReader>();
        containerBuilder.RegisterValue(inputReader);

        containerBuilder.RegisterValue(_gameSettings);

        containerBuilder.RegisterType(typeof(SaveSystem), new[] { typeof(SaveSystem), typeof(ISaveRegistry) }, Lifetime.Singleton, Resolution.Eager);
        containerBuilder.RegisterType(typeof(Game), Lifetime.Singleton, Resolution.Eager);
    }

    private void StartGame(Container container)
    {
        Game game = container.Resolve<Game>();
        game.ToMainMenu();
    }
}