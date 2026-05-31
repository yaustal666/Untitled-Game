using UnityEngine;

public class PlayerSpawner
{
    public GameObject playerPrefab;

    public PlayerSpawner(GameSettings settings)
    {
        playerPrefab = settings.PlayerPrefab;
    }

    public GameObject SpawnPlayer()
    {
        return SpawnPlayer(new Vector3(0, 0, 0));
    }

    public GameObject SpawnPlayer(Vector3 position)
    {
        return Object.Instantiate(playerPrefab, position, Quaternion.identity);
    }
}