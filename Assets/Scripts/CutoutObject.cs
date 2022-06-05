using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class CutoutObject : MonoBehaviour
{
    [SerializeField] private List<Transform> targetObjects = new List<Transform>();
    [SerializeField] private LayerMask wallMask;

    public event EventHandler<onPlayerBehindWallArgs> onPlayerBehindWall;
    public class onPlayerBehindWallArgs : EventArgs{
        public RaycastHit[] colliders;
    }

    public event EventHandler<onPlayerNotBehindWallArgs> onPlayerNotBehindWall;
    public class onPlayerNotBehindWallArgs : EventArgs{
        public Transform[] colliders;
    }

    private List<Transform> overflowedWalls = new List<Transform>();

    private Camera mainCamera;

    public void AddTarget(Transform target){
        targetObjects.Add(target);
    }

    /// <summary>
    /// Function (Event) aplying opacity to walls which players are behind 
    /// </summary>
    /// <param name="sender">Event sender</param>
    /// <param name="args">Array of RaycastHit colliders</param>
    void OnPlayerBehindWall(object sender, onPlayerBehindWallArgs args){
        RaycastHit[] colliders = args.colliders;
        Transform[] collidersTransform = colliders.Select(c => c.transform).ToArray();

        // Imposes opacity on walls when player is behind them
        for(int c = 0; c < colliders.Length; c++){
            Material[] materials = colliders[c].transform.GetComponent<Renderer>().materials;

            for(int m = 0; m < materials.Length; m++){
                Color color = materials[m].color;
                color.a = .5f;
                materials[m].color = color;
            }

            // Adds Transform object to list of overflowed walls
            if(!overflowedWalls.Contains(colliders[c].transform)) overflowedWalls.Add(colliders[c].transform);
        }

        // List of no longer overflowed walls
        List<Transform> notLongerOverflowedWalls = new List<Transform>();

        for(int c = 0; c < overflowedWalls.Count; c++){
            if(!collidersTransform.Contains(overflowedWalls[c])){
                notLongerOverflowedWalls.Add(overflowedWalls[c]);
            }
        }

        onPlayerNotBehindWall?.Invoke(this, new onPlayerNotBehindWallArgs { colliders = notLongerOverflowedWalls.ToArray() });
    }

    void OnPlayerNotBehindWall(object sender, onPlayerNotBehindWallArgs args){
        for(int c = 0; c < args.colliders.Length; c++){
            Material[] materials = args.colliders[c].transform.GetComponent<Renderer>().materials;

            for(int m = 0; m < materials.Length; m++){
                Color color = materials[m].color;
                color.a = 1f;
                materials[m].color = color;
            }
            
            overflowedWalls.Remove(args.colliders[c].transform);
        }
    }

    void Awake()
    {
        mainCamera = GetComponent<Camera>();
        onPlayerBehindWall += OnPlayerBehindWall;
        onPlayerNotBehindWall += OnPlayerNotBehindWall;


    }

    void Update()
    {
        if(targetObjects.Count > 0){
            for(int t = 0; t < targetObjects.Count; t++){
                Transform target = targetObjects[t];
                Vector3 offset = target.position - transform.position;
                RaycastHit[] colliders = Physics.RaycastAll(transform.position, offset, offset.magnitude, wallMask);

                onPlayerBehindWall?.Invoke(this, new onPlayerBehindWallArgs { colliders = colliders });
            }
        }
    }
}
