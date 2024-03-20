using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerSpawnerController : NetworkBehaviour, IPlayerJoined, IPlayerLeft
{
    [SerializeField] NetworkPrefabRef playerNetworkPrefab = NetworkPrefabRef.Empty;
    [SerializeField] Transform[] spawnPoints;

    public override void Spawned()
    {
        if (!Runner.IsServer) return;

        foreach (var player in Runner.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }

    private void SpawnPlayer(PlayerRef playerRef)
    {
        if (!Runner.IsServer) return;

        var index = playerRef % spawnPoints.Length;
        var spawnPoint = spawnPoints[index].transform.position;
        var playerObject = Runner.Spawn(playerNetworkPrefab, spawnPoint, Quaternion.identity, playerRef);

        Runner.SetPlayerObject(playerRef, playerObject);
    }

    private void DespawnPlayer(PlayerRef playerRef)
    {
        if (!Runner.IsServer) return;

        if (Runner.TryGetPlayerObject(playerRef, out var playerNetworkObject))
        {
            Runner.Despawn(playerNetworkObject);
        }

        //Reset player object
        Runner.SetPlayerObject(playerRef, null);

    }

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player);
    }

    public void PlayerLeft(PlayerRef player)
    {
        DespawnPlayer(player);
    }
}
