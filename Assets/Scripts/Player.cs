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
