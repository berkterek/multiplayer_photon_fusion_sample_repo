using System.Collections.Generic;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerPhotonFusionSample.Managers
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] string roomCode;
        
        NetworkRunner _networkRunner;
        
        public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log(nameof(OnPlayerJoined));
        }

        public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
        {
            Debug.Log(nameof(OnPlayerLeft));
        }

        public void OnInput(NetworkRunner runner, NetworkInput input)
        {
        }

        public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input)
        {
        }

        public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason)
        {
        }

        public void OnConnectedToServer(NetworkRunner runner)
        {
        }

        public void OnDisconnectedFromServer(NetworkRunner runner)
        {
        }

        public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token)
        {
        }

        public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason)
        {
        }

        public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message)
        {
        }

        public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList)
        {
        }

        public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data)
        {
        }

        public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken)
        {
        }

        public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data)
        {
        }

        public void OnSceneLoadDone(NetworkRunner runner)
        {
        }

        public void OnSceneLoadStart(NetworkRunner runner)
        {
        }

        private async void StartGame(GameMode gameMode)
        {
            _networkRunner = gameObject.AddComponent<NetworkRunner>();
            _networkRunner.ProvideInput = true;

            var tryHost = gameMode == GameMode.Host;
            Debug.Log(tryHost ? "Try host" : "Try Join");

            var result = await _networkRunner.StartGame(new StartGameArgs()
            {
                GameMode = gameMode,
                SessionName = roomCode,
                Scene = SceneManager.GetActiveScene().buildIndex,
                SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
            });

            if (result.Ok)
            {
                Debug.Log(tryHost ? "Game started successfully":"Game joined successfully");
            }
            else
            {
                Debug.LogError($"Game failed to {(tryHost ? "start" : "join")} with result: {result.ShutdownReason}");   
            }
        }

        [ContextMenu(nameof(StartWithHost))]
        private void StartWithHost()
        {
            StartGame(GameMode.Host);
        }

        [ContextMenu(nameof(StartWithClient))]
        private void StartWithClient()
        {
            StartGame(GameMode.Client);
        }
    }    
}
