using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Sources referenced: https://www.youtube.com/watch?v=_QajrabyTJc */
public class MouseLook : MonoBehaviour
{

    /* Affects how much the view will move relative to mouse input */
    public float mouseSensitivity = 100f;
    /* How far the first person camera can pitch up/down */
    public float maxCameraPitch = 90f;
    /* Reference to the transform of our player body capsule */
    public Transform playerBody; // Gets set by dragging/dropping in the Unity editor

    /* Keeps track of the camera's current pitch, i.e. its rotation about its x-axis */
    private float cameraPitch = 0f;
    /* Keeps track of the view's yaw, i.e. the rotation about the player body's y-axis */
    private float cameraYaw = 0f;

    // Start is called before the first frame update
    void Start()
    {
        /*
         Lock the cursor so that it doesn't show up when moving the mouse to move the camera. Escape unlocks the cursor
         so the user can click off the screen, click back on the screen to lock the cursor again.
         */
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        /* TODO: Need to generalize this to support gamepad */
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Multiply by dt to make mouse movement framerate independent
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        /*
         Use the mouse x-input to rotate the player body around its y-axis relative to itself; this yaws the player's view left/right
         by rotating the player body itself, which rotates the camera with it. By applying yaw rotation to the body instead of the camera,
         the body effectively acts like a gimbal and prevents gimbal lock (if we tried to apply both yaw and pitch to either the body or
         camera, we would get gimbal lock). This rotation is relative to the player body itself b/c no second argument is supplied to the Rotate function.
         */
        //playerBody.Rotate(Vector3.up * mouseX); // This is a simpler, alternative way of yawing the camera that doesn't require an additional state variable (cameraYaw).
        cameraYaw += mouseX;
        playerBody.localRotation = Quaternion.Euler(0f, cameraYaw, 0f); 

        /*
         Responsible for pitching the player's view up/down. We do this by pitching the camera itself and not the entire player body. This script is
         attached to our camera object, so we can access the camera's rotation values through the keyword 'transform'. 
         */
        cameraPitch -= mouseY; // Update the current pitch based on change mouseY position
        cameraPitch = Mathf.Clamp(cameraPitch, -maxCameraPitch, maxCameraPitch); // Need to clamp to prevent the camera from pitching too far up or down.
        transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f); // The quaternion class creates an actual rotation object that we an apply

        /*
         This method does not work for pitching the camera because it does not allow us to clamp the range of rotation. We need to maintain the state
         of the current pitch rotation so we can clamp it before setting the euler value on the quaternion when generating a new rotation.
         */
        // transform.Rotate(Vector3.right * mouseY * -1f);
    }
}
