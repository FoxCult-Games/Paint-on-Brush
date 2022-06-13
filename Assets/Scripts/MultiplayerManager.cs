using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEditor;
using System;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager instance;
    public int MAX_PLAYERS = 4;

    [SerializeField] private InputActionAsset playerInputs;
    [SerializeField] private bool isSelectionPanelEnabled = false;

    private List<PlayerJoiningData> players = new List<PlayerJoiningData>();

    void Awake()
    {
        PlayerInputManager manager = GetComponent<PlayerInputManager>();

        instance = this;
        DontDestroyOnLoad(this);

        // GetComponent<PlayerInputManager>().JoinPlayer();
        MAX_PLAYERS = manager.maxPlayerCount;

        if(!GameObject.Find("CameraCenter")) GetComponent<CameraCenterController>().enabled = false;
        else GetComponent<CameraCenterController>().enabled = true;
    }

    /// <summary>
    /// Adds player whether is space. Removes first inactive player is there is not space.
    /// </summary>
    /// <param name="player"></param>
    public void AddPlayer(PlayerInput player){
        if(!isSelectionPanelEnabled) return;
        Debug.Log("Adding player...");

        PlayerJoiningData playerData = new PlayerJoiningData(player, true); 

        if(players.Contains(playerData) || (!players.Contains(playerData) && players.Count() >= MAX_PLAYERS)) return;

        if(players.Count >= MAX_PLAYERS){
            PlayerJoiningData firstDeactivedPlayer = players.First(p => !p.isPlayerActive());

            Debug.Log(firstDeactivedPlayer);
        }

        players.Add(playerData);
        Debug.Log(players.Count());
        InputActionAsset inputActions = playerInputs;
        player.actions = inputActions;
        FindObjectOfType<MenuManager>()?.OnPlayerJoinGUI(players.IndexOf(playerData));
    }

    /// <summary>
    /// Deactivates player on device losing connection
    /// </summary>
    /// <param name="player"></param>
    public void DeactivatePlayer(PlayerInput player){
        players.FirstOrDefault(p => p.GetPlayer() == player).DeactivatePlayer();
        Debug.Log("Player has been deactivated");
    }

    /// <summary>
    /// Reconnects player when device restores connection
    /// </summary>
    /// <param name="player"></param>
    public void ReconnectPlayer(PlayerInput player){
        players.FirstOrDefault(p => p.GetPlayer() == player).ActivatePlayer();
        Debug.Log("Player has reconnected!");
    }

    public void RemovePlayer(PlayerInput player){
        PlayerJoiningData playerData = players.FirstOrDefault(p => p.GetPlayer() == player);
        FindObjectOfType<MenuManager>()?.OnPlayerLeftGUI(players.IndexOf(playerData));
        players.Remove(playerData);
    }

    public List<PlayerJoiningData> GetPlayers(){
        return players;
    }

    public List<PlayerInput> GetPlayersInputs(){
        List<PlayerInput> _players = new List<PlayerInput>();
        players.ForEach(player => {
            _players.Add(player.GetPlayer());
        });
        return  _players;
    }

    public PlayerInput GetPlayer(PlayerInput player){
        return players.FirstOrDefault(p => p.GetPlayer() == player).GetPlayer();
    }

    public void EnableSelectionPanel(){
        isSelectionPanelEnabled = true;
    }

    public void DisableSelectionPanel(){
        isSelectionPanelEnabled = false;
    }
}

[Serializable]
public struct PlayerJoiningData{
    PlayerInput player;
    bool isActive;

    public PlayerJoiningData(PlayerInput player, bool isActive){
        this.player = player;
        this.isActive = isActive;
    }

    public void DeactivatePlayer(){
        this.isActive = false;
    }

    public void ActivatePlayer(){
        this.isActive = true;
    }

    public bool isPlayerActive(){
        return this.isActive;
    }

    public PlayerInput GetPlayer(){
        return this.player;
    }
}
