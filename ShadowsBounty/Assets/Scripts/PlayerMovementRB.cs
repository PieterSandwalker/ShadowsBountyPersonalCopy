// Some stupid rigidbody based movement by Dani

using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerMovementRB : MonoBehaviour {

    public enum PlayerState { IDLING, CROUCH_IDLING, CROUCH_WALKING, WALKING, SPRINTING, SLIDING, FALLING, WALL_RUNNING, MANTLING, LADDER_CLIMBING }
    public enum WallrunDebugInfo { TOO_SLOW, BAD_APPROACH_ANGLE, NON_HORIZONTAL_IMPACT_NORMAL, NOT_CONTACTING_WALL, GROUNDED }

    private Dictionary<WallrunDebugInfo, bool> WallrunDebugLog;

    private KeyCode crouchKey = KeyCode.C;
    private KeyCode sprintKey = KeyCode.LeftShift;

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public Transform head;
    
    //Other
    private Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float zRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    
    //Movement
    public float moveSpeed = 4500;
    public float walkMaxSpeed = 20;
    public float sprintMaxSpeed = 40f;
    public float wallrunMaxSpeed = 30f;
    public bool grounded;
    public LayerMask whatIsGround;

    //added by Sam's Code {
    public float sprintMultipler = 2f;
    public float crouchMultiplier = 0.66f;
    public float slideMultiplier = 1.5f;
    public float slideToCrouchThreshold = 15f;
    //}

    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;
    
    //Input
    float x, y;
    bool jumpPressed, sprintPressed, crouchPressed;
    
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    /* Ledge Climb */

    //Can edit from script
    public Transform ledgeClearCheck;
    public float verticalCheckDistance = 1.25f;
    public float horizontalCheckDistance = 1.0f;
    public float upwardMantleForce = 5000f;
    public float forwardMantleForce = 750f;
    public float counterMantleForce = 5000f; //Used to make mantling feel less floating by grounding the character more quickly and giving the player back control faster

    //Not viewable or editable from script
    private const string LEDGE_TAG_NAME = "Ledge";

    /* Wallrunning */

    //Can edit from script
    public LayerMask wallLayer;
    public Transform wallContactCheck;
    public float wallrunCameraAngle = 15f;
    public float wallContactCheckRadius = 0.51f;
    public float wallrunRaycastLength = 1f; //Making this longer will allow the player to wall run around curved walls
    public float impactNormalYThreshold = 0.0001f; //Threshold that defines how small y component of the surface impact normal can be for wall running to be possible 
    public float startWallrunThreshold = 12f; //Current max walk speed is ~20 units/s, player slows down significantly when colliding with wall
    public float stopWallrunThresholdS = 12f; //The speed that the player must stay above to initiate or continue wallrunning
    public float stopVerticalMovementThreshold = 1f; //How much vertical velocity the player can have while wallrunning before it gets zeroed out
    public float wallrunSpeedBoost = 2f;
    public float resistanceFactor = 100f; //Affects how quickly the player loses momentum while wall running. Player slows down faster when this is larger. At 0 player can wallrun indefinitely

    //Read only from script
    [SerializeField]
    private PlayerState _movementState = PlayerState.IDLING; // Init player state to standing idle
    public PlayerState movementState { get { return _movementState; } }
    [SerializeField]
    private Vector3 _meanSurfaceImpactNormal;
    public Vector3 meanSurfaceImpactNormal { get { return _meanSurfaceImpactNormal; } }
    [SerializeField]
    private bool _isWallRight;
    public bool isWallRight { get { return _isWallRight; } }
    [SerializeField]
    private int _wallrunTime = 0; //How many frames the player has been wallrunning
    public int wallrunTime { get { return _wallrunTime;  } }

    //Not viewable or editable from script
    private const string WALL_LAYER_NAME = "Wall";

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Start()
    {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        InitWallrunDebugLog();
    }
    
    private void FixedUpdate()
    {
        StateMachineUpdate();
        Movement();
        Debug.LogWarning(rb.velocity.magnitude);
    }

    private void Update()
    {
        MyInput();
        Look();
    }

    /// <summary>
    /// Find user input. Should put this in its own class but im lazy
    /// </summary>
    private void MyInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumpPressed = Input.GetButton("Jump");
        crouchPressed = Input.GetKey(crouchKey);

        //Sprinting
        if (Input.GetKeyDown(sprintKey) && _movementState == PlayerState.WALKING) //Can only transition to sprint state if walking
            StartSprint();
        if (Input.GetKeyUp(sprintKey) && _movementState == PlayerState.SPRINTING)
            StopSprint();

        //Crouching
        if (Input.GetKeyDown(crouchKey))
            StartCrouch();
        if (Input.GetKeyUp(crouchKey))
            StopCrouch();
    }

    private void StateMachineUpdate()
    {
        /* State transitions */
        if (rb.velocity.magnitude > 0.1f) //Transition from idle states to walking states
            if (_movementState == PlayerState.IDLING)
                _movementState = PlayerState.WALKING;
            else if (_movementState == PlayerState.CROUCH_IDLING)
                _movementState = PlayerState.CROUCH_WALKING;
        else if (Mathf.Approximately(rb.velocity.magnitude, 0f)) //Transition from walking states to idle states
            if (_movementState == PlayerState.WALKING)
                _movementState = PlayerState.IDLING;
            else if (_movementState == PlayerState.CROUCH_WALKING)
                _movementState = PlayerState.CROUCH_IDLING;
        //TODO: Add transition from walk to sprint, sprint to walk and sprint to idle

        if (!grounded && _movementState != PlayerState.WALL_RUNNING && _movementState != PlayerState.MANTLING) //Transition to falling
            _movementState = PlayerState.FALLING;
        else if (grounded && _movementState == PlayerState.FALLING) //Transition from falling to grounded
            _movementState = PlayerState.IDLING;
    }

    private void StartSprint()
    {
        moveSpeed *= sprintMultipler;
        _movementState = PlayerState.SPRINTING;
    }

    private void StopSprint()
    {
        moveSpeed /= sprintMultipler;
        _movementState = PlayerState.WALKING;
    }

    //TODO: Add a raycast check that prevents player from uncrouching if there is an object above them
    private void StartCrouch()
    {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (grounded) {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Movement()
    {
        //Extra gravity (only apply when not wallrunning)
        if (_movementState != PlayerState.WALL_RUNNING)
            rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        if (_movementState != PlayerState.WALL_RUNNING) CounterMovement(x, y, mag);
        
        //If holding jump && ready to jump, then jump
        if ((readyToJump || _movementState == PlayerState.WALL_RUNNING) && jumpPressed) Jump(); 

        //Set max speed
        float maxSpeed = walkMaxSpeed;
        
        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (crouchPressed && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        if (_movementState == PlayerState.WALL_RUNNING) maxSpeed = wallrunMaxSpeed;
        if (_movementState == PlayerState.SPRINTING) maxSpeed = sprintMaxSpeed;

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > maxSpeed) x = 0;
        if (x < 0 && xMag < -maxSpeed) x = 0;
        if (y > 0 && yMag > maxSpeed) y = 0;
        if (y < 0 && yMag < -maxSpeed) y = 0;

        //If sprinting, don't allow strafing, block horizontal axis input
        if (_movementState == PlayerState.SPRINTING) x = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        //Movement in air
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }
        
        //Movement while sliding
        if (grounded && crouchPressed) multiplierV = 0f;

        //Check for ledge
        if (IsLedge())
        {
            readyToJump = false;
            _movementState = PlayerState.MANTLING;
            Debug.LogWarning("Ledge detected!");
        }

        /* Apply forces based on player's movement state */
        if (_movementState == PlayerState.WALL_RUNNING)
        {
            float rightOrLeft = 1f; //If 1, wall is right. If -1, wall is left
            if (!_isWallRight) { rightOrLeft = -1f; }

            // Check if the player is still contacting the wall by raycasting in the direction of the wall
            RaycastHit wallHitCheck;
            bool isContactingWall = Physics.Raycast(orientation.transform.position, orientation.transform.right * rightOrLeft, out wallHitCheck, wallrunRaycastLength);

            //TODO: Check if the object we've hit (if any) has the WALL_LAYER_TAG

            //Check if the player is moving fast enough by checking the z component of their velocity (in local space)
            //bool isMovingFastEnough = orientation.transform.InverseTransformDirection(rb.velocity).z >= stopWallrunThresholdSpeed;
            bool isMovingFastEnough = rb.velocity.magnitude >= stopWallrunThresholdS; // Necessary for allowing smooth transition for wallrunning, jumping to another wall and continuing wallrunning

            if (isContactingWall && isMovingFastEnough && !grounded)
            {
                //Apply a gradually increasing resistance force to slow player movement while wallrunning
                Vector3 proj = orientation.transform.forward - Vector3.Dot(orientation.transform.forward, _meanSurfaceImpactNormal) * _meanSurfaceImpactNormal;
                Vector3 horizontalResistance = proj * _wallrunTime * resistanceFactor * Time.deltaTime * -1f;
                Vector3 verticalResistance = Vector3.down * _wallrunTime * resistanceFactor * Time.deltaTime * 0.15f;
                Vector3 resistanceForce = horizontalResistance + verticalResistance;
                rb.AddForce(resistanceForce);

                _wallrunTime++; //Increment wallrun timer

                // Direction to apply force in is the projection of the rigid body's forward vector onto the contact plane of the wall
                Vector3 wallrunForce = proj * y * moveSpeed * wallrunSpeedBoost * Time.deltaTime;
                rb.AddForce(wallrunForce);

                //Counter unwanted vertical (upward) movement
                if (rb.velocity.y > stopVerticalMovementThreshold)
                {
                    Vector3 vel = rb.velocity;
                    rb.velocity = new Vector3(vel.x, 0, vel.z);
                }
            }
            else
            {
                _movementState = PlayerState.FALLING;
                rb.useGravity = true;
            }

            WallrunDebugLog[WallrunDebugInfo.NOT_CONTACTING_WALL] = !isContactingWall;
            WallrunDebugLog[WallrunDebugInfo.TOO_SLOW] = !isMovingFastEnough;
            WallrunDebugLog[WallrunDebugInfo.GROUNDED] = grounded;

            GetWallrunDebugInfo();
        }
        else if (_movementState == PlayerState.MANTLING)
        {
            Mantle();
        }
        else
        {
            //Apply forces to move player
            Vector3 forwardForce = orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV;
            Vector3 sidewaysForce = orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier;
            Vector3 resultant = forwardForce + sidewaysForce;
            rb.AddForce(resultant);
        }
    }

    private void Jump()
    {
        if (readyToJump)
        {
            readyToJump = false;

            if (grounded)
            {
                //Add jump forces
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);
            }
            else if (_movementState == PlayerState.WALL_RUNNING)
            {
                //Add jump forces
                Vector3 jumpOffForce = (Vector3.up * jumpForce * 1.5f) + (_meanSurfaceImpactNormal * jumpForce * 3.5f); //TODO: Tweak this so it scales more with movement speed
                rb.AddForce(jumpOffForce);
            }

            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0)
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);

            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump() { readyToJump = true; }
    
    private float desiredX;
    private void Look()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Handle tilting the camera left/right when entering wallrun, resetting back to neutral when out of wallrun
        if (_movementState == PlayerState.WALL_RUNNING)
        {
            if (_isWallRight) zRotation++; //If the wall is to our right, want to add to z rotation to tilt to the left
            else              zRotation--; //If the wall is to our left, want to subtract to tilt to the right
            zRotation = Mathf.Clamp(zRotation, -wallrunCameraAngle, wallrunCameraAngle);
        }
        else
        {
            // This resets the camera when finished wall running
            if (zRotation > 0)            zRotation--;
            else if (zRotation < 0)       zRotation++;
        }

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, zRotation);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, zRotation);
    }

    private void CounterMovement(float x, float y, Vector2 mag)
    {
        if (!grounded || jumpPressed) return;

        //Slow down sliding
        if (crouchPressed) {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > threshold && Math.Abs(x) < 0.05f || (mag.x < -threshold && x > 0) || (mag.x > threshold && x < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > threshold && Math.Abs(y) < 0.05f || (mag.y < -threshold && y > 0) || (mag.y > threshold && y < 0)) {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > walkMaxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * walkMaxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    /// <summary>
    /// Find the velocity relative to where the player is looking
    /// Useful for vectors calculations regarding movement and limiting movement
    /// </summary>
    /// <returns></returns>
    public Vector2 FindVelRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v)
    {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    /// <summary>
    /// Handle ground detection
    /// </summary>
    private void OnCollisionStay(Collision other)
    {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        //Invoke ground/wall cancel, since we can't check normals with CollisionExit
        float delay = 3f;
        if (!cancellingGrounded) {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded() { grounded = false; }

    private void OnCollisionEnter(Collision collision)
    {
        //TODO: Should wrap any wallrun-specific code in a function, call it here
        //First check if we are colliding against a wall
        if (Physics.CheckSphere(wallContactCheck.position, wallContactCheckRadius, wallLayer) && collision.collider.tag == "Wall")
        {
            //Calculate the mean surface impact normal
            Vector3 meanSurfaceImpactNormal = new Vector3();

            foreach (ContactPoint c in collision.contacts)
            {
                meanSurfaceImpactNormal += c.normal;
            }

            meanSurfaceImpactNormal /= collision.contacts.Length; //Calculate the average impact normal
            _meanSurfaceImpactNormal = meanSurfaceImpactNormal.normalized; //Get the re-normalized value, store in variable so it can be viewable for debugging
            float angleOfApproach = Mathf.Acos(Vector3.Dot(meanSurfaceImpactNormal.normalized, orientation.transform.forward)) * Mathf.Rad2Deg; //The angle at which the player contacts the wall

            bool isImpactHorizontal = meanSurfaceImpactNormal.y >= -impactNormalYThreshold && meanSurfaceImpactNormal.y <= impactNormalYThreshold; //If the surface impact normal is mostly horizontal i.e. low y component 
            bool isGoodApproachAngle = angleOfApproach >= 45f && angleOfApproach <= 160f; // The angle between orientation.transform.forward and meanSurfaceImpactNormal is within a threshold
            //bool isMovingFastEnough = orientation.transform.InverseTransformDirection(rb.velocity).z >= startWallrunThreshold; // The player is moving fast enough
            bool isMovingFastEnough = rb.velocity.magnitude >= startWallrunThreshold;

            //TODO: Rather than checking actual numerical speed, can probably just check the state that the player is in

            if (isImpactHorizontal && isGoodApproachAngle && isMovingFastEnough)
            {
                _movementState = PlayerState.WALL_RUNNING; //Set state to wall running
                rb.useGravity = false; //Disable gravity
                _wallrunTime = 0; //Reset timer
                _isWallRight = Physics.Raycast(orientation.transform.position, orientation.transform.right, wallrunRaycastLength); // Determine to which side of the player the wall is
            }

            WallrunDebugLog[WallrunDebugInfo.NON_HORIZONTAL_IMPACT_NORMAL] = !isImpactHorizontal;
            WallrunDebugLog[WallrunDebugInfo.BAD_APPROACH_ANGLE] = !isGoodApproachAngle;
            WallrunDebugLog[WallrunDebugInfo.TOO_SLOW] = !isMovingFastEnough;

            GetWallrunDebugInfo();
        }
    }

    private bool IsLedge()
    {
        //Vertical raycast: Start @ x units along the forward vector + y units up, direct down
        RaycastHit verticalHit = new RaycastHit();
        Vector3 startPos = head.transform.position + orientation.transform.forward * horizontalCheckDistance + new Vector3(0, 1, 0);
        bool isVerticalContact = Physics.Raycast(startPos, Vector3.down, out verticalHit, verticalCheckDistance);
        Debug.DrawRay(startPos, Vector3.down * verticalCheckDistance, Color.red);

        //Forward raycast: Start @ player head, direct towards forward vector (orientation.transform.forward)
        RaycastHit horizontalHit = new RaycastHit();
        bool isHorizontalContact = Physics.Raycast(head.transform.position, orientation.transform.forward, out horizontalHit, horizontalCheckDistance);
        Debug.DrawRay(head.transform.position, orientation.transform.forward * horizontalCheckDistance, Color.blue);

        //Check if the object is actually a ledge (i.e. has a ledge tag)
        bool isLedge = false;
        if (isVerticalContact) //This check prevents NPE
            isLedge = verticalHit.collider.gameObject.tag.Equals(LEDGE_TAG_NAME);

        //If the vertical raycast hits but horizontal doesn't and both raycasts hit something with the ledge tag
        return isVerticalContact && !isHorizontalContact && isLedge;
    }

    private void Mantle()
    {
        //Perform forward raycast check from node. If check successful, apply upward force. If unsuccessful, apply forward force.
        RaycastHit hit = new RaycastHit();
        bool stillClimbing = Physics.Raycast(ledgeClearCheck.position, orientation.transform.forward, out hit, horizontalCheckDistance);

        rb.useGravity = !stillClimbing; //Use gravity if not climbing anymore

        //If still climbing, climb force is upward. Else, apply a forward and downward force
        Vector3 climbForce = stillClimbing ? orientation.transform.up * upwardMantleForce : (orientation.transform.forward * forwardMantleForce) + (Vector3.down * counterMantleForce);

        rb.AddForce(climbForce * Time.deltaTime);

        //If raycast didn't hit and grounded, no longer ledgeclimbing, set state to IDLE, set canJump to true
        if (!stillClimbing && grounded)
        {
            _movementState = PlayerState.IDLING;
            readyToJump = true;
        }
    }

    private void InitWallrunDebugLog()
    {
        WallrunDebugLog = new Dictionary<WallrunDebugInfo, bool>();
        WallrunDebugLog.Add(WallrunDebugInfo.BAD_APPROACH_ANGLE, false);
        WallrunDebugLog.Add(WallrunDebugInfo.GROUNDED, false);
        WallrunDebugLog.Add(WallrunDebugInfo.NON_HORIZONTAL_IMPACT_NORMAL, false);
        WallrunDebugLog.Add(WallrunDebugInfo.NOT_CONTACTING_WALL, false);
        WallrunDebugLog.Add(WallrunDebugInfo.TOO_SLOW, false);
    }

    private void GetWallrunDebugInfo()
    {
        foreach (KeyValuePair<WallrunDebugInfo, bool> kvp in WallrunDebugLog)
            if (kvp.Value) Debug.LogWarning(kvp.Key);
        ResetWallrunDebugLog();
    }

    private void ResetWallrunDebugLog()
    {
        foreach (WallrunDebugInfo key in Enum.GetValues(typeof(WallrunDebugInfo))) WallrunDebugLog[key] = false;
    }
}
