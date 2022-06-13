using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;

    [SerializeField] private GameObject spawns; 

    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, spawns.transform.position, Quaternion.identity);
    }
}
