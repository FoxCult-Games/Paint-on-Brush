using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;
using System;

public class PlayerPickingController : MonoBehaviour
{

    [Header("Pickup data")]
    [SerializeField] private float pickUpRange;
    [SerializeField] private Vector3 pickUpRangeoffset;
    [SerializeField] LayerMask itemMask;

    public event EventHandler<GameObject> OnPickUpItem;
    public event EventHandler<GameObject> OnDropItem;

    [Header("Pick up animation")]
    [SerializeField] private Transform handIKTarget; 
    [SerializeField] private Animator animator;

    void Start()
    {
        OnPickUpItem += PickUpItem;
        OnPickUpItem += SetIKTargetPosition;
    }

    public void PickUpItemInput(InputAction.CallbackContext ctx){
        Vector3 center = transform.position - pickUpRangeoffset;

        Collider[] nearItems = Physics.OverlapSphere(center, pickUpRange, itemMask);

        if(nearItems.Length == 0) return;

        float closestDistance = 9999;

        for(int i = 0; i < nearItems.Length; i++){
            float distance = Vector3.Distance(center, nearItems[i].transform.position); 
            if(distance < closestDistance) closestDistance = distance;
        }

        Transform closestItem = nearItems.First(item => Vector3.Distance(center, item.transform.position) == closestDistance).transform;

        OnPickUpItem?.Invoke(this, closestItem.gameObject);
    }

    void PickUpItem(object o, GameObject item) {
        animator.SetTrigger("GrabItem");
    }

    void SetIKTargetPosition(object o, GameObject item){
        handIKTarget.position = item.transform.position;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position - pickUpRangeoffset, pickUpRange);
    }
}
