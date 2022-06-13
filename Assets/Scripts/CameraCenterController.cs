using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraCenterController : MonoBehaviour
{
    [SerializeField] private Transform cameraCenter;



    void LateUpdate()
    {
        cameraCenter.position = GetCenterPoint();
    }

    Vector3 GetCenterPoint(){
        List<PlayerJoiningData> players = MultiplayerManager.instance.GetPlayers();

        if(players.Count == 0) return Vector3.zero;

        if(players.Count == 1){
            return players[0].GetPlayer().transform.position;
        }

        var bounds = new Bounds(players[0].GetPlayer().transform.position, Vector3.zero);
        for(int i = 0; i < players.Count; i++){
            bounds.Encapsulate(players[i].GetPlayer().transform.position);
        }
    
        return bounds.center;
    }
}
