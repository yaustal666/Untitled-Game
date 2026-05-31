using Cysharp.Threading.Tasks;
using Reflex.Core;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public async UniTask LoadScene(string sceneName, Container sessionScope)
    {
        void OverrideParent(Scene scene, ContainerBuilder builder) => builder.SetParent(sessionScope);
        ContainerScope.OnSceneContainerBuilding += OverrideParent;

        await LoadSceneAsync(sceneName);

        ContainerScope.OnSceneContainerBuilding -= OverrideParent;
    }

    public async UniTask LoadScene(string sceneName)
    {
        await LoadSceneAsync(sceneName);
    }

    private async UniTask LoadSceneAsync(string sceneName)
    {
        await SceneManager.LoadSceneAsync(sceneName).ToUniTask();
    }
}