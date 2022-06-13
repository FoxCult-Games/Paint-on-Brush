using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandleDeviceStates : MonoBehaviour
{
    public void DeviceDisconnected(PlayerInput player){
        MultiplayerManager.instance?.RemovePlayer(player);
        Debug.Log("Player has been disconnected!");
    }

    public void DeviceRegained(PlayerInput player){
        MultiplayerManager.instance?.ReconnectPlayer(player);
        Debug.Log("Player connected again successfully!");
    }
}
