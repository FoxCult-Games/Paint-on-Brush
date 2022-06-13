using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using System;
using TMPro;
using Ludiq;

namespace FoxCultGames.Multiplayer.Photon{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        [SerializeField] private TMP_InputField createInput;
        [SerializeField] private TMP_InputField joinInput;
        [SerializeField] private TextMeshProUGUI createRoomBtnText;
        [SerializeField] private TextMeshProUGUI joinRoomBtnText;
        [SerializeField] private TextMeshProUGUI createErrorMessageText;
        [SerializeField] private TextMeshProUGUI joinErrorMessageText;

        public event EventHandler<string> OnRoomCreated;
        public event EventHandler<string> OnRoomJoined;

        public event EventHandler<string> OnCreateRoomNameNotValidated;
        public event EventHandler<string> OnJoinRoomNameNotValidated;

        [Header("Validation Button Texts")]
        [SerializeField] private string joinRoomText = "Join Room";
        [SerializeField] private string joiningRoomText = "Joining...";
        [SerializeField] private string createRoomText = "Create Room";
        [SerializeField] private string creatingRoomText = "Creating...";

        [Header("Validation Restrictions")]
        [SerializeField] private int minRoomNameLength = 3;
        [SerializeField] private int maxRoomNameLength = 255;
        [SerializeField] private List<string> roomNameFilters = new List<string>();

        [Header("Validation Error Messages")]
        [SerializeField] private string roomNameToShortMessage;
        [SerializeField] private string roomNameToLongMessage;
        [SerializeField] private string roomNameNotAllowedMessage;

        [Header("Menu Panels")]
        [SerializeField] private GameObject SelectPlayersPanel;
        [SerializeField] private GameObject JoinRoomPanel;
        [SerializeField] private GameObject CreateRoomPanel;
        
        [Header("Player Selection")]
        [SerializeField] private TextMeshProUGUI RoomNameText;

        void Start()
        {
            // If player is not connected to server, load MainMenu scene
            if(!PhotonNetwork.IsConnected){
                SceneManager.LoadScene("MainMenu");
                return;
            }

            // Joins player to the lobby
            PhotonNetwork.JoinLobby();

            OnRoomCreated += InitializeRoom;
            OnRoomCreated += SetRoomNameText;

            OnCreateRoomNameNotValidated += ShowCreateErrorMessage;
            OnJoinRoomNameNotValidated += ShowJoinErrorMessage;

            OnCreateRoomNameNotValidated += SetDefaultCreateBtnText;
            OnJoinRoomNameNotValidated += SetDefaultJoinBtnText;

            // Make errors invisible at start
            createErrorMessageText.gameObject.SetActive(false);
            joinErrorMessageText.gameObject.SetActive(false);
            
            // Sets default button text
            createRoomBtnText.text = createRoomText;
            joinRoomBtnText.text = joinRoomText;
        }

        /// <summary>
        /// Start of room creation
        /// </summary>
        public void CreateRoom(){
            string roomName = createInput.text;

            // Checks if room name is valid
            if(!ValidateRoomName(ValidationType.CREATE_ROOM, roomName)) return;

            // Fires event when room name is valid
            OnRoomCreated?.Invoke(this, roomName);
        }

        public void JoinRoom(){
            PhotonNetwork.CreateRoom(joinInput.text);
        }

        public override void OnJoinedRoom(){
            JoinRoomPanel.SetActive(false);
            CreateRoomPanel.SetActive(false);

            SelectPlayersPanel.SetActive(true);
        }

        /// <summary>
        /// Validates room name. Checks for: length, exluded words
        /// 
        /// if not valid - error will be displayed
        /// </summary>
        /// <param name="validationType">Type of validation (create or join)</param>
        /// <param name="room">Room name</param>
        /// <returns></returns>
        bool ValidateRoomName(ValidationType validationType, string room){
            string messageError = null;

            // Sets suitable button text on validating
            if(validationType == ValidationType.CREATE_ROOM)
                createRoomBtnText.text = creatingRoomText;
            else if(validationType == ValidationType.JOIN_ROOM)
                joinRoomBtnText.text = joiningRoomText;

            // Cheks if room name is valid
            if(room.Length < minRoomNameLength)
                messageError = roomNameToShortMessage;
            if(room.Length > maxRoomNameLength)
                messageError = roomNameToLongMessage;
            if(roomNameFilters.FindAll(filter => room.Contains(filter)).Count > 0)
                messageError = roomNameNotAllowedMessage;

            // Fires suitable event if room name is invalid
            if(messageError.NullIfEmpty() != null){
                if(validationType == ValidationType.CREATE_ROOM)
                    OnCreateRoomNameNotValidated?.Invoke(this, messageError);
                else if(validationType == ValidationType.JOIN_ROOM)
                    OnJoinRoomNameNotValidated?.Invoke(this, messageError);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Shows create error message
        /// </summary>
        /// <param name="o"></param>
        /// <param name="message">Error message</param>
        void ShowCreateErrorMessage(object o, string message){
            createErrorMessageText.text = message;
            createErrorMessageText.gameObject.SetActive(true);
        }

        /// <summary>
        /// Shows join error message
        /// </summary>
        /// <param name="o"></param>
        /// <param name="message">Error message</param>
        void ShowJoinErrorMessage(object o, string message){
            joinErrorMessageText.text = message;
            joinErrorMessageText.gameObject.SetActive(true);
        }

        /// <summary>
        /// Sets create button text to default
        /// </summary>
        /// <param name="o"></param>
        /// <param name="message"></param>
        void SetDefaultCreateBtnText(object o, string message){
            createRoomBtnText.text = createRoomText;
        }

        /// <summary>
        /// Sets join button text to default
        /// </summary>
        /// <param name="o"></param>
        /// <param name="message"></param>
        void SetDefaultJoinBtnText(object o, string message){
            joinRoomBtnText.text = joinRoomText;
        }

        /// <summary>
        /// Sets room name visible in lobby corner
        /// </summary>
        /// <param name="o"></param>
        /// <param name="room">Room name</param>
        void SetRoomNameText(object o, string room){
            RoomNameText.text = "Room name: " + room;
        }

        /// <summary>
        /// Creates room with options
        /// </summary>
        /// <param name="o"></param>
        /// <param name="room">Room name</param>
        void InitializeRoom(object o, string room){
            PhotonNetwork.CreateRoom(room, new RoomOptions() {
                MaxPlayers = 4,
            });
        }

        public enum ValidationType{
            CREATE_ROOM,
            JOIN_ROOM
        }
    }

}
