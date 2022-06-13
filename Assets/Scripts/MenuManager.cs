using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using FoxCultGames.Multiplayer.Photon;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject MultiplayerManagement;

    [Header("Player Nickname")]
    [SerializeField] private TextMeshProUGUI PlayerNicknameText;
    [SerializeField] private TextMeshProUGUI ErrorMessageText;
    
    [Header("")]
    [SerializeField] private GameObject[] playersPanels;

    void Start()
    {
        GoToMainMenu();

        // Reference to server connection controller
        ConnectToServer connectToServer = FindObjectOfType<ConnectToServer>();

        if(connectToServer){
            connectToServer.OnNicknameValidated += ShowPlayerNickname;

            connectToServer.OnNicknameNotValidated += ShowErrorsOnValidation;
        }

        PlayerNicknameText.gameObject.SetActive(false);
        ErrorMessageText.gameObject.SetActive(false);
    }

    public void GoToMainMenu(){
        MultiplayerManagement.SetActive(false);
        MultiplayerManager.instance?.DisableSelectionPanel();

        MainMenu.SetActive(true);
    }

    public void GoToChoosePlayers(){
        MainMenu.SetActive(false);
        MultiplayerManagement.SetActive(false);

        MultiplayerManager.instance?.EnableSelectionPanel();
    }

    public void GoToMultiplayerManagement(){
        MainMenu.SetActive(false);

        MultiplayerManagement.SetActive(true);
        MultiplayerManager.instance?.DisableSelectionPanel();
    }

    public void OnPlayerJoinGUI(int index){
        EnableAllObjectsInPanel(playersPanels[index].transform);
    }

    public void OnPlayerLeftGUI(int index){
        EnableAllObjectsInPanel(playersPanels[index].transform);
    }

    public void DisableAllObjectsInPanel(Transform go){
        foreach(Transform child in go){
            child.gameObject.SetActive(false);
            if(child.name == "JoinInfo") child.gameObject.SetActive(true);
        }
    }

    public void EnableAllObjectsInPanel(Transform go){
        foreach(Transform child in go){
            child.gameObject.SetActive(true);
            if(child.name == "JoinInfo") child.gameObject.SetActive(false);
        }
    }

    public void Exit(){
        Application.Quit();
    }

    /// <summary>
    /// Shows player nickname in corner
    /// </summary>
    /// <param name="o"></param>
    /// <param name="nickname">Player nickname</param>
    void ShowPlayerNickname(object o, string nickname){
        PlayerNicknameText.text = "Player: " + nickname;
        PlayerNicknameText.gameObject.SetActive(true);
    }

    /// <summary>
    /// Shows error on validating player nickname
    /// </summary>
    /// <param name="e"></param>
    /// <param name="message"></param>
    void ShowErrorsOnValidation(object e, string message){
        ErrorMessageText.text = message;
        ErrorMessageText.gameObject.SetActive(true);
    }
}
