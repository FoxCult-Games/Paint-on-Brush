using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PlayerItem : MonoBehaviour
{
    // TODO: Make player fully customizable
    // https://youtu.be/D28Drg9MCi4?t=519

    [SerializeField] private TextMeshProUGUI playerName;

    /// <summary>
    /// Changes Player Selection GUI
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayerInfo(Player player){
        playerName.text = player.NickName;
    }
}
