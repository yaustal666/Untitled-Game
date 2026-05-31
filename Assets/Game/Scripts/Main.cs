using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    public static void LoadMain()
    {
        SceneManager.LoadScene("Game");
    }
}