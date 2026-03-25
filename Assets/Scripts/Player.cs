using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance { get; private set; }

    public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
    public class OnSelectedCounterChangedEventArgs : EventArgs
    {
        public CleanCounter selectedCounter;
    }

    [SerializeField] private float moveSpeed=7f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask countersLayerMask;

    private CleanCounter selectedCounter;

    private Vector3 lastInteractDir;

    private bool isWalking;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Player instance in the scene!");
        }
        Instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
    }

    private void GameInput_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedCounter != null)
        {
            selectedCounter.Interact();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        HandleMovement();
        HandleInteractions();

    }

    private void HandleInteractions()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // Only update the last interaction direction if the player is moving
        if (moveDir != Vector3.zero)
        {
            lastInteractDir = moveDir;
        }

        float interactDistance = 2f;
        if(Physics.Raycast(transform.position, lastInteractDir, out RaycastHit raycastHit, interactDistance, countersLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out CleanCounter cleanCounter))
            {
               
                if(cleanCounter != selectedCounter )
                {
                    setSelectedCounter(cleanCounter);
                }
            }
            else{                 // We are not looking at any counter, clear the selected counter
                setSelectedCounter(null);
            }
        }
        else
        {
            setSelectedCounter(null);
        }
    //  Debug.Log(selectedCounter);

    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVectorNormalized();
        // Convert the 2D input vector to a 3D movement direction (assuming movement on the XZ plane)
        Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

        // Check for potential collisions in the movement direction using a capsule cast
        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        // If there's an obstacle in the movement direction, try to move only along the X or Z axis
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

        isWalking = moveDir != Vector3.zero;
        // Smoothly rotate the player to face the movement direction
        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
    }

    internal bool IsWalking()
    {
      return isWalking;
    }

    private void setSelectedCounter(CleanCounter selectedCounter)
    {
        this.selectedCounter = selectedCounter;
        OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs
        {
            selectedCounter = selectedCounter
        });
    }
}
