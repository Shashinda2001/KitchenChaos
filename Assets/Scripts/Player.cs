using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed=7f;
    [SerializeField] private GameInput gameInput;

    private bool isWalking;

    // Update is called once per frame
    private void Update()
    {
       
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        // Convert the 2D input vector to a 3D movement direction (assuming movement on the XZ plane)
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // Check for potential collisions in the movement direction using a capsule cast
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {
            //can't move towards moveDir, try only X movement
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = moveDir.x != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {
                //can't move towards moveDir, try only Z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = moveDir.z != 0 && !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    moveDir = moveDirZ;
                }
            }

        }
       

        if (canMove)
        transform.position += moveDir * Time.deltaTime * moveSpeed;

        isWalking =moveDir != Vector3.zero;
        // Smoothly rotate the player to face the movement direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    internal bool IsWalking()
    {
      return isWalking;
    }
}
