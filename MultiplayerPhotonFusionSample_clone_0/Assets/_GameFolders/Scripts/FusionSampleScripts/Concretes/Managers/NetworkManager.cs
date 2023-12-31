using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using Fusion.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MultiplayerPhotonFusionSample.Managers
{
    public class NetworkManager : MonoBehaviour, INetworkRunnerCallbacks
    {
        [SerializeField] string _roomCode;
        [SerializeField] NetworkRunner _networkRunnerPrefab;

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

        public async UniTask StartGameAsync(GameMode gameMode, string sessionCode, CancellationToken cancellationToken)
        {
            _roomCode = sessionCode;
            if (_networkRunner == null)
            {
                _networkRunner = Instantiate(_networkRunnerPrefab,transform);
                _networkRunner.AddCallbacks(this);
                _networkRunner.ProvideInput = true;
            }

            var tryHost = gameMode == GameMode.Host;
            Debug.Log(tryHost ? "Try host" : "Try Join");
            
            try
            {
                var result = await _networkRunner.StartGame(new StartGameArgs()
                {
                    GameMode = gameMode,
                    SessionName = _roomCode,
                    Scene = SceneManager.GetActiveScene().buildIndex,
                    SceneManager = gameObject.GetComponent<NetworkSceneManagerDefault>(),
                });
            
                if (result.Ok && !cancellationToken.IsCancellationRequested)
                {
                    Debug.Log(tryHost ? "Game started successfully":"Game joined successfully");
                    Debug.Log($"Room Code => {_roomCode}");
                }
                else
                {
                    Debug.LogError($"Game failed to {(tryHost ? "start" : "join")} with result: {result.ShutdownReason}");
                    ShutdownAsync();
                }
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
                ShutdownAsync();
            }
        }

        private async void ShutdownAsync()
        {
            if (_networkRunner != null)
            {
                await _networkRunner.Shutdown();
                _networkRunner = null;
            }
        }
    }    
}

