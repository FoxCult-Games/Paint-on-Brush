using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;

    public PhotonView view;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    Vector2 moveAxis = Vector2.zero;

    void Awake()
    {
        // Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();
        view = GetComponent<PhotonView>();
    }

    void FixedUpdate()
    {
        if(view.IsMine){
            if(moveAxis == Vector2.zero) return;

            // Get targetMovingSpeed.
            float targetMovingSpeed = IsRunning ? runSpeed : speed;
            if (speedOverrides.Count > 0)
            {
                targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
            }

            // Get targetVelocity from input.
            Vector2 targetVelocity = new Vector2( moveAxis.x * targetMovingSpeed, moveAxis.y * targetMovingSpeed);

            // Apply movement.
            rigidbody.velocity = transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y);
    
        }
    }

    public void getMovementAxis(InputAction.CallbackContext ctx){
        moveAxis = ctx.ReadValue<Vector2>();
    }

    public void getRunning(InputAction.CallbackContext ctx){
        IsRunning = ctx.performed && canRun;
    }
}