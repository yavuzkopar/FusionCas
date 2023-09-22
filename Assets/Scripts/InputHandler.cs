using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    Vector2 moveInputVector = Vector2.zero;
    Vector2 viewInputVector = Vector2.zero;
    bool isJumpButtonPressed = false;
    bool isFireButtonPressed = false;
    bool interactWithObject = false;
    bool isDropPressed = false;


    LocalCameraHandler cameraHandler;
    CharacterMover characterMover;
    private void Awake()
    {
        cameraHandler = GetComponentInChildren<LocalCameraHandler>();
        characterMover = GetComponent<CharacterMover>();
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    void Update()
    {
        if (!characterMover.Object.HasInputAuthority) return;

        viewInputVector.x = Input.GetAxis("Mouse X");
        viewInputVector.y = Input.GetAxis("Mouse Y") * -1f;

        cameraHandler.SetviewInput(viewInputVector);

        moveInputVector.x = Input.GetAxis("Horizontal");
        moveInputVector.y = Input.GetAxis("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            isJumpButtonPressed = true;
        }
        if (Input.GetMouseButtonDown(0))
        {
            isFireButtonPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            interactWithObject = true;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            isDropPressed = true;
        }
    }

    public NetworkInputData GetNetworkInput()
    {
        NetworkInputData networkInputData = new NetworkInputData();

        networkInputData.aimForwardVector = cameraHandler.transform.forward;

        networkInputData.movementInput = moveInputVector;

        networkInputData.isJumpPressed = isJumpButtonPressed;
        networkInputData.isFirePressed = isFireButtonPressed;
        networkInputData.isInteracted = interactWithObject;
        networkInputData.isDropped = isDropPressed;

        isJumpButtonPressed = false;
        isFireButtonPressed = false;
        interactWithObject = false;
        isDropPressed = false;

        return networkInputData;
    }
}
