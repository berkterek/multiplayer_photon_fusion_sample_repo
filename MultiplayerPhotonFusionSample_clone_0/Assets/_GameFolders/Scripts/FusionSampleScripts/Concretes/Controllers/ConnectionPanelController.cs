using System.Threading;
using Cysharp.Threading.Tasks;
using Fusion;
using MultiplayerPhotonFusionSample.Helpers;
using MultiplayerPhotonFusionSample.Managers;
using TMPro;
using UnityEngine;

namespace MultiplayerPhotonFusionSample.Controllers
{
    public class ConnectionPanelController : MonoBehaviour
    {
        [SerializeField] GameObject _hostButton;
        [SerializeField] GameObject _joinButton;
        [SerializeField] TMP_InputField _roomCodeInputField;
        [SerializeField] GameObject _enterButton;
        [SerializeField] GameObject _backButton;
        [SerializeField] GameObject _cancelButton;
        [SerializeField] TMP_Text _infoText;

        CancellationTokenSource _cancellationTokenSource;
        string _hostWaitingMessage = "Waiting to create game";
        string _clientWaitingMessage = "Waiting to join game";
        float _timeToChangeDot = 0.5f;
        bool _isCancel;

         private void Start()
        {
            SetDefaultState();
        }
        
        public async void OnHostButtonClicked()
        {
            _cancelButton.SetActive(true);
            _hostButton.SetActive(false);
            _joinButton.SetActive(false);
            
            WaitingTextTask(true);

            await GameManager.Instance.NetworkManager.StartGameAsync(GameMode.Host,
                SessionCodeGeneratorHelper.Generate());

            await UniTask.WaitForSeconds(1f);
            _isCancel = true;
            
            if(_cancelButton != null) _cancelButton.SetActive(false);
        }
        
        public async void OnClientEnteredButtonClicked()
        {
            _cancelButton.SetActive(true);
            _roomCodeInputField.gameObject.SetActive(false);
            _enterButton.SetActive(false);
            _backButton.SetActive(false);
            
            WaitingTextTask(false);

            await GameManager.Instance.NetworkManager.StartGameAsync(GameMode.Client, _roomCodeInputField.text);

            await UniTask.WaitForSeconds(1f);
            _isCancel = true;
            
            if(_cancelButton != null) _cancelButton.SetActive(false);
        }
        
        public void OnCancelButtonClicked()
        {
            _infoText.text = string.Empty;
            SetDefaultState();
        }

        public void OnJoinButtonClicked()
        {
            _cancelButton.SetActive(false);
            _hostButton.SetActive(false);
            _joinButton.SetActive(false);
            _roomCodeInputField.gameObject.SetActive(true);
            _enterButton.SetActive(true);
            _backButton.SetActive(true);
        }

        public void OnBackButtonClicked()
        {
            SetDefaultState();
        }
        
        private void SetDefaultState()
        {
            _hostButton.SetActive(true);
            _joinButton.SetActive(true);
            _roomCodeInputField.gameObject.SetActive(false);
            _enterButton.SetActive(false);
            _backButton.SetActive(false);
            _cancelButton.SetActive(false);
            _infoText.text = string.Empty;
        }
        
        private async void WaitingTextTask(bool isHost)
        {
            while (true)
            {
                _infoText.text = isHost ? _hostWaitingMessage : _clientWaitingMessage;
            
                await UniTask.WaitForSeconds(_timeToChangeDot);
                _infoText.text += ".";
                await UniTask.WaitForSeconds(_timeToChangeDot);
                _infoText.text += ".";
                await UniTask.WaitForSeconds(_timeToChangeDot);
                _infoText.text += ".";
                await UniTask.WaitForSeconds(_timeToChangeDot);
                _infoText.text = _infoText.text.Remove(_infoText.text.Length - 3);

                if (_isCancel)
                {
                    _infoText.SetText(string.Empty);
                    break;
                }
            }
        }
    }
}