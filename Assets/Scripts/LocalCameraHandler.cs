using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalCameraHandler : MonoBehaviour
{
    Camera localCamera;
    public Transform followTarget;
    float cameraRotationX;
    float cameraRotationY;
    Vector2 viewInput;
    CharacterMover characterMover;
    private void Awake()
    {
        localCamera = GetComponentInChildren<Camera>();
        characterMover = GetComponentInParent<CharacterMover>();
    }
    void Start()
    {
        if(localCamera.enabled)
        {
            transform.parent = null;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (followTarget == null) return;
        if (!localCamera.enabled) return;
        transform.position = followTarget.position;
        cameraRotationX += viewInput.y * Time.deltaTime * 50;
        cameraRotationY += viewInput.x * Time.deltaTime * 50;
        cameraRotationX = Mathf.Clamp(cameraRotationX,-70,70);
        transform.rotation = Quaternion.Euler(cameraRotationX, cameraRotationY, 0);
    }
    public void SetviewInput(Vector2 viewInput)
    {
        this.viewInput = viewInput; 
    }
}
