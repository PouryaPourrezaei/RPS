using FishNet.Object;
using FishNet.Managing;
using UnityEngine;

public class PlayerSpawner : NetworkBehaviour
{
    public GameObject playerPrefab;

    public override void OnStartServer()
    {
        base.OnStartServer();
        SpawnPlayer();
    }

    [Server]
    private void SpawnPlayer()
    {
        GameObject player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
        Spawn(player);
    }
}
