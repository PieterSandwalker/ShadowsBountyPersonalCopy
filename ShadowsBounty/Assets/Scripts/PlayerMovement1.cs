using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Source: https://www.youtube.com/watch?v=XAC8U9-dTZU */

public class PlayerMove : MonoBehaviour
{
    /* */
    public CharacterController controller;
    /* */
    public Transform groundCheck;
    /* */
    public LayerMask groundMask;
    /* */
    public float movementSpeed = 12.0f;
    /* */
    public float jumpHeight = 5.0f;
    /* */
    public float playerMass = 1f;
    /* */
    public float gravity = -9.81f;
    /* */
    public float groundCheckDistance = 0.4f;

    /* */
    private Vector3 velocity;
    /* */
    private bool isGrounded;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask); // Project a sphere around the bottom of the player to check if it is on the ground

        if (isGrounded && velocity.y < 0) // If the player is on the ground and they are "falling" still...
        {
            controller.slopeLimit = 45f;
            velocity.y = -2f; // Rather than just setting velocity to 0, set to a negative value to force the player fully onto the ground in case isGrounded is set to true too early
        }

        // NOTE: These should automatically map to a gamepad
        float x = Input.GetAxisRaw("Horizontal"); // Move the player left/right relative to the direction they're facing
        float z = Input.GetAxisRaw("Vertical"); // Move the player forward/backward relative to the direction they're facing

        /*
         Multiplying by transform.right and transform.forward ensures that we move relative to the player and don't just use
         global coordinates (e.g. if we just did Vector3(x, 0f, z), this wouldn't work). Then just sum the two vectors
         so we only have to work with the resultant rather than the separate components.
         */
        Vector3 movementDirection = transform.right * x + transform.forward * z;
        movementDirection = movementDirection.normalized; // Normalize so there aren't speed increases when moving diagonally

        controller.Move(movementDirection * movementSpeed * Time.deltaTime); // Move the character using the character controller reference

        if (Input.GetButtonDown("Jump") && isGrounded) // NOTE: This input also should automatically map
        {
            controller.slopeLimit = 100f; // Updating the slope limit when jumping makes jumping a bit smoother in cases when the player jumps against an object
            velocity.y = Mathf.Sqrt(-2f * jumpHeight * gravity * playerMass); // TODO: Add some check in case gravity is set to be positive this doesn't throw an error
        }

        velocity.y += playerMass * gravity  * Time.deltaTime; // Update velocity based on acceleration due to gravity

        controller.Move(velocity * Time.deltaTime); // Multiply by dt again b/c free fall is proportional to time squared
    }
}
