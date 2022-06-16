using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System;
using Ludiq;

namespace FoxCultGames.Multiplayer.Photon{

    public class ConnectToServer : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField usernameField;
        [SerializeField] private TMP_Text usernameBtnText;

        public event EventHandler<string> OnNicknameValidated;
        public event EventHandler<string> OnNicknameNotValidated;

        [Header("Validation Button Texts")]
        [SerializeField] private string connectText = "Connect";
        [SerializeField] private string validatingText = "Connecting...";

        [Header("Validation Restrictions")]
        [SerializeField] private int minNicknameLength = 3;
        [SerializeField] private int maxNicknameLength = 255;
        [SerializeField] private List<string> nicknameFilters = new List<string>();

        [Header("Validation Error Messages")]
        [SerializeField] private string nicknameToShortMessage;
        [SerializeField] private string nicknameToLongMessage;
        [SerializeField] private string nicknameNotAllowedMessage;

        [Header("Menu Panels")]
        [SerializeField] private GameObject NicknamePanel;


        void Start()
        {
            OnNicknameValidated += ConnectPlayer;

            OnNicknameNotValidated += (object o, string message) => { usernameBtnText.text = connectText; };

            usernameBtnText.text = connectText;
        }

        public override void OnConnectedToMaster(){
            SceneManager.LoadScene("Lobby");
        }

        /// <summary>
        /// Checks if player nickname is valid. Checks for: length, excluded words
        /// 
        /// If not valid - error will be displayed
        /// </summary>
        public void ValidateNickname(){
            string nickname = usernameField.text;
            string messageError = null;

            usernameBtnText.text = validatingText;

            // Validates nickname
            if(nickname.Length < minNicknameLength)
                messageError = nicknameToShortMessage;
            if(nickname.Length > maxNicknameLength)
                messageError = nicknameToLongMessage;
            if(nicknameFilters.FindAll(filter => nickname.Contains(filter)).Count > 0)
                messageError = nicknameNotAllowedMessage;

            // Fires event if nickname is invalid
            if(messageError.NullIfEmpty() != null){
                OnNicknameNotValidated?.Invoke(this, messageError);
                return;
            }

            // Fires event if nickname is valid
            OnNicknameValidated?.Invoke(this, nickname);
        }

        /// <summary>
        /// Connects player to server with valid nickname
        /// </summary>
        /// <param name="o">Sender</param>
        /// <param name="nickname">Validated nickname</param>
        void ConnectPlayer(object o, string nickname){
            // Sets nickname
            PhotonNetwork.NickName = nickname;
            // Whether scenes should be sync for all players
            PhotonNetwork.AutomaticallySyncScene = true;
            // Connect player to server with valid nickname
            PhotonNetwork.ConnectUsingSettings();
        }
    }
}