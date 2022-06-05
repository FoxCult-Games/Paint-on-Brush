using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MultiplayerManager : MonoBehaviour
{
    public static MultiplayerManager instance;

    private List<PlayerInput> players = new List<PlayerInput>();

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);

        if(!GameObject.Find("CameraCenter")) GetComponent<CameraCenterController>().enabled = false;
        else GetComponent<CameraCenterController>().enabled = true;
    }

    public void AddPlayer(PlayerInput player){
        players.Add(player);
    }

    public void RemovePlayer(PlayerInput player){
        players.Remove(player);
    }

    public List<PlayerInput> GetPlayers(){
        return players;
    }
}
