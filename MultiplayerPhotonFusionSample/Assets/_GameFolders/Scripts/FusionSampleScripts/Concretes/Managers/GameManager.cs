using System;
using UnityEngine;

namespace MultiplayerPhotonFusionSample.Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] NetworkManager _networkManager;
        [SerializeField] LobbyUiManager _lobbyUiManager;

        public NetworkManager NetworkManager => _networkManager;
        public LobbyUiManager LobbyUiManager => _lobbyUiManager;

        public static GameManager Instance { get; private set; }

        void Awake()
        {
            if (Instance == null) Instance = this;
        }
    }
}