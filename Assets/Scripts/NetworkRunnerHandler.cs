using Fusion;
using Fusion.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NetworkRunnerHandler : MonoBehaviour
{
    
    public NetworkRunner networkRunnerPrefab;

    NetworkRunner networkRunner;

    private void Awake()
    {
        NetworkRunner runnerInScene = FindObjectOfType<NetworkRunner>();
        if (runnerInScene != null)
        {
            networkRunner = runnerInScene;
        }
    }
    void Start()
    {
        if (networkRunner == null)
        {


            networkRunner = Instantiate(networkRunnerPrefab);
            networkRunner.name = "Network runner";

          //  var clientTask = InitializeNetworkRunner(networkRunner, GameMode.AutoHostOrClient, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);

          //  Debug.Log($"Server NetworkRunner started.");
        }
    }

    protected virtual Task InitializeNetworkRunner(NetworkRunner runner, GameMode gameMode,string sessionName, NetAddress address, SceneRef scene, Action<NetworkRunner> initialized)
    {
        var sceneManager = runner.GetComponents(typeof(MonoBehaviour)).OfType<INetworkSceneManager>().FirstOrDefault();

        if (sceneManager == null)
        {
            //Handle networked objects that already exits in the scene
            sceneManager = runner.gameObject.AddComponent<NetworkSceneManagerDefault>();
        }

        runner.ProvideInput = true;

        return runner.StartGame(new StartGameArgs
        {
            GameMode = gameMode,
            PlayerCount = 4,
            Address = address,
            Scene = scene,
            SessionName = sessionName,
            CustomLobbyName = "Lobby",
            Initialized = initialized,
            SceneManager = sceneManager
        });
    }
    public void OnJoinLobby()
    {
        var clientTask = JoinLobby();
    }
    public event Action OnJoin;
    async Task JoinLobby()
    {
        string lobbyID = "Lobby";
        var result = await networkRunner.JoinSessionLobby(SessionLobby.Custom,lobbyID);
        if(result.Ok )
        {
            OnJoin?.Invoke();
        }

    }
    public void CreateGame(string sessionName)
    {
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Host,sessionName, NetAddress.Any(), 1, null);
    }
    public void JoinGame(SessionInfo sessionInfo)
    {
        var clientTask = InitializeNetworkRunner(networkRunner, GameMode.Client,sessionInfo.Name, NetAddress.Any(), SceneManager.GetActiveScene().buildIndex, null);
    }
}
