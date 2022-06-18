using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;
using System.Linq;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private SpawnPoint[] spawnPoints; 

    void Start()
    {
        SpawnPoint[] remainingSpawnPoints = spawnPoints.Where(point => !point.isAssigned()).ToArray();

        int randomSpawnPointIndex = UnityEngine.Random.Range(0, remainingSpawnPoints.Length);
        Transform spawnPoint = remainingSpawnPoints[randomSpawnPointIndex].getPoint();

        PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, Quaternion.identity);
    }

    [Serializable]
    struct SpawnPoint {
        [SerializeField] private Transform point;
        [SerializeField] private bool assigned;

        public SpawnPoint(Transform point) {
            this.point = point;
            assigned = false;
        }

        public bool isAssigned() {
            return this.assigned;
        }

        public Transform getPoint(){
            return this.point;
        }
    }
}
