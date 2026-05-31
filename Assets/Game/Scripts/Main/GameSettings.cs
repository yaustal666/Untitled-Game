using UnityEngine;

[CreateAssetMenu(fileName = "GameSettings", menuName = "ScriptableObjects/GameSettings")]
public class GameSettings : ScriptableObject
{
    public GameObject PlayerPrefab;
    public GameObject UIRootPrefab;
    public TextAsset Dialogs;
}