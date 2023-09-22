using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CharacterMover : NetworkBehaviour
{

    [SerializeField] float speed;
    float rotY;
    Rigidbody _rigidbody;
    bool camJump;
    [SerializeField] Animator animator;
    private void Awake()
    {
        _rigidbody= GetComponent<Rigidbody>();
        camJump= true;
    }
    public override void FixedUpdateNetwork()
    {
        
        if (GetInput(out NetworkInputData networkInputData))
        {
           
            transform.forward = networkInputData.aimForwardVector;
            Quaternion rotation = transform.rotation;
            rotation.eulerAngles = new Vector3(0,rotation.eulerAngles.y,0);
            transform.rotation = rotation;
            Vector2 moveInput = Vector2.ClampMagnitude(networkInputData.movementInput, 1);
            Vector3 move = transform.forward * moveInput.y + transform.right * moveInput.x;
            transform.position += move * Runner.DeltaTime * speed;
       //   _rigidbody.velocity= move * speed;
            if(networkInputData.isJumpPressed)
            {
                Jump();
            }
            animator.SetFloat("Horizontal", networkInputData.movementInput.x);
            animator.SetFloat("Vertical", networkInputData.movementInput.y);
            if (networkInputData.isInteracted && Object.HasInputAuthority)
            {
                animator.SetTrigger("take");
            }
        }

    }
    private void OnCollisionEnter(Collision collision)
    {
        camJump = true;
    }
    void Jump()
    {
        if (!camJump) return;
        animator.SetTrigger("jump");
        _rigidbody.velocity = Vector3.up * 5;
        camJump = false;
    }
    
}
