using System.Collections;
using System.Collections.Generic;
using Fusion;
using TMPro;
using UnityEngine;

public class PlayerSpawnerController : NetworkBehaviour, IPlayerJoined
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

    void SpawnPlayer(PlayerRef playerRef)
    {
        if (!Runner.IsServer) return;

        var index = playerRef % spawnPoints.Length;
        var spawnPoint = spawnPoints[index].transform.position;
        Runner.Spawn(playerNetworkPrefab, spawnPoint, Quaternion.identity, playerRef);

    }

    public void PlayerJoined(PlayerRef player)
    {
        SpawnPlayer(player);
    }
}
