using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using Fusion.Sockets;
using System;
using UnityEngine.SceneManagement;

public class BasicSpawner : MonoBehaviour, INetworkRunnerCallbacks
{
    private NetworkRunner _runner;
    [SerializeField] NetworkPrefabRef _playerPrefab;
    Dictionary<PlayerRef, NetworkObject> spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    SessionListHandler listHandler;

    InputHandler inputHandler;
    [SerializeField] NetworkPrefabRef map;

    private void Awake()
    {
        listHandler = FindObjectOfType<SessionListHandler>(true);
    }
    public void OnConnectedToServer(NetworkRunner runner) { }

    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }

    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }

    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }

    public void OnDisconnectedFromServer(NetworkRunner runner) { }

    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }

    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (inputHandler == null && Player.Local != null)
        {
            inputHandler = Player.Local.GetComponent<InputHandler>();
        }
        if (inputHandler != null)
            input.Set(inputHandler.GetNetworkInput());
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            // Create a unique position for the player
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            
            // Keep track of the player avatars so we can remove it when they disconnect
         //   spawnedCharacters.Add(player, networkPlayerObject);
        }
        Debug.Log("Joined");
    }

    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        //if (spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        //{
        //    runner.Despawn(networkObject);
        //    spawnedCharacters.Remove(player);
        //}
    }

    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ArraySegment<byte> data) { }

    public void OnSceneLoadDone(NetworkRunner runner) 
    {
      //  runner.Spawn(map, Vector3.zero, Quaternion.identity, null);
    }

    public void OnSceneLoadStart(NetworkRunner runner) { }

    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) 
    {
        if (listHandler == null)
        {
            return;
        }
        if(sessionList.Count != 0)
        {
            listHandler.ClearList();
            foreach (SessionInfo item in sessionList)
            {
                listHandler.AddToList(item);
            }
        }
    }

    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }

    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }


}
