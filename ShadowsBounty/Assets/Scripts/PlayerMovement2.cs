using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement2 : MonoBehaviour, PlayerButtonInput.IPlayerControlsActions {

    //Input
    [Header("Data")]
    [SerializeField]
    private InputData inputData;
    private PlayerButtonInput buttonInput;

    //Assingables
    public Transform playerCam;
    public Transform orientation;
    public Transform head;
    
    //Other
    public Rigidbody rb;

    //Rotation and look
    private float xRotation;
    private float zRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;
    private float yaw; //mouseX before
    private float pitch; //mouseY before
    
    //Movement
    public float movementForce = 4500;
    public float walkMaxSpeed = 10;
    public float crouchWalkMaxSpeed = 6;
    public float crouchSlideMaxSpeed = 16;
    public float sprintMaxSpeed = 15;
    public float wallrunMaxSpeed = 20;
    public LayerMask whatIsGround;

    public float counterMovement = 0.175f;
    private float counterMovementThreshold = 0.01f;
    public float slopeThreshold = -5f; //Angle that determines whether or not player should perform a crouch slide or use a momentum-building slide. Anything less than this is considered steep enough for building momentum.
    public float maxSlopeAngle = 35f; //The max slope of any surface to still be considered ground. Prevents players from walking up inclines that are too steep

    [SerializeField]
    private bool _grounded;
    public bool grounded { get { return _grounded; } }
    [SerializeField]
    private bool _isPlayerOnSlope = false;
    public bool isPlayerOnSlope { get { return _isPlayerOnSlope; } }
    [SerializeField]
    private float _speed = 0f;
    public float currentSpeed { get { return _speed; } }
    [SerializeField]
    private float _maxSpeed = 0f; //State variable
    public float currentMaxSpeed { get { return _maxSpeed; } }
    [SerializeField]
    private bool _sliding = false; //Flag for when player is in crouch slide state (used both for regular crouch slide and momentum-building downward incline slides)
    public bool sliding { get { return _sliding; } }
    [SerializeField]
    private bool _wallrunning = false;
    public bool wallrunning { get { return _wallrunning; } }

    private float _terminalVelocity = 60f; //Velocity that the player cannot exceed, even with momentum-building movements

    //Crouch & Slide
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;

    public bool deferSlide = false;

    //Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;
    
    //Input
    float x, y;
    bool jumping, sprinting, crouching; //These booleans are based entirely on button input
    
    //Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    /* Ledge Climb */

    //Can edit from script
    public Transform ledgeClearCheck;
    public float verticalCheckDistance = 1.25f;
    public float horizontalCheckDistance = 2.0f;
    public float upwardMantleForce = 800f;
    public float forwardMantleForce = 300f;

    /* Wallrunning */

    //Can edit from script
    public Transform wallContactCheck;
    public float wallrunCameraAngle = 15f;
    public float wallrunCameraAngleTransitionRate = 0.01f;
    public float wallContactCheckRadius = 0.51f;
    public float wallrunRaycastLength = 1f; //Making this longer will allow the player to wall run around curved walls
    public float impactNormalYThreshold = 0.0001f; //Threshold that defines how small y component of the surface impact normal can be for wall running to be possible 

    public float hWallBoostForce = 10f; //Initial horizontal force multiplier when player initiates wallrunning. Scales with the forward component of their velocity.
    public float vWallBoostForce = 600f; //Vertical force that gives wallrunning its arc

    public float xWallJumpForceMult = 2f; //Multipliers for the components of the jump off force. Each component gets multiplied by jumpForce.
    public float yWallJumpForceMult = 1f;
    public float zWallJumpForceMult = 0f;

    private int _wallrunFrames; //How many frames the player has been wallrunning. Used for lerping camera rotation.

    [SerializeField]
    private Vector3 _meanSurfaceImpactNormal;
    public Vector3 meanSurfaceImpactNormal { get { return _meanSurfaceImpactNormal; } }
    [SerializeField]
    private bool _isWallRight;
    public bool isWallRight { get { return _isWallRight; } }

    //Not viewable or editable from script
    private const string LEDGE_TAG_NAME = "Ledge";
    private const string WALL_TAG_NAME = "Wall";

    private KeyCode crouchKey = KeyCode.C;
    private KeyCode sprintKey = KeyCode.LeftShift;

    void Awake() {
        rb = GetComponent<Rigidbody>();
        buttonInput = new PlayerButtonInput();
        buttonInput.PlayerControls.SetCallbacks(this);
    }
    
    void Start() {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    
    private void FixedUpdate() {
        _speed = rb.velocity.magnitude;
        //Debug.LogWarning(orientation.transform.InverseTransformDirection(rb.velocity)); //For reference: This is how to get the rigid body's velocity transformed into the orientation node's local coordinate space
        Movement();
    }

    private void Update() {
        MyInput();
        Look();
    }

    //TODO: Refactor to work with new input system
    //Get user input.
    private void MyInput() {

        /* New input system (for mouse + keyboard and gamepad) */
        Vector2 look = inputData.LookAction.ReadValue<Vector2>();

        yaw = look.x * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        pitch = look.y * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        Vector2 movement = inputData.MovementAction.ReadValue<Vector2>();
        x = movement.x;
        y = movement.y;

        /* Legacy input system */

        //Basic movement
        //x = Input.GetAxisRaw("Horizontal");
        //y = Input.GetAxisRaw("Vertical");

        //Camera control
        //yaw = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        //pitch = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        //Get the current pressed state of various keys (jump, sprint, crouch)
        //jumping = Input.GetButton("Jump");
        //crouching = Input.GetKey(crouchKey);
        //sprinting = Input.GetKey(sprintKey);

        //Crouching
        //if (Input.GetKeyDown(crouchKey)) StartCrouch();
        //if (Input.GetKeyUp(crouchKey)) StopCrouch();
    }

    /* Automatically called for the input system */
    private void OnEnable()
    {
        buttonInput.Enable();
    }

    private void OnDisable()
    {
        buttonInput.Disable();
    }

    /* Callback functions that must be implemented for the input interface */
    public void OnJump(InputAction.CallbackContext ctx)
    {
        //ctx.performed = true means the button was pressed. So when the button was pressed we can set the booleans to true. When it's false we set them to false.
        jumping = ctx.performed;
    }

    public void OnSprint(InputAction.CallbackContext ctx)
    {
        sprinting = ctx.performed;
    }

    public void OnCrouch(InputAction.CallbackContext ctx)
    {
        crouching = ctx.performed;
        if (ctx.started) StartCrouch();
        else if (ctx.canceled) StopCrouch();
    }

    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);

        if (rb.velocity.magnitude > 0.5f && !_grounded) deferSlide = true; //If the player is midair when they crouch, set flag to call start slide later when they land
        else if (rb.velocity.magnitude > 0.5f || IsOnSlope()) StartSlide(); //If the player is currently grounded, call start slide now
    }

    private void StartSlide()
    {
        if (_grounded)
        {
            if (!IsOnSlope()) rb.AddForce(orientation.transform.forward * slideForce); //Determine whether we do a regular crouch slide or momentum-building crouch slide
            _sliding = true;
            deferSlide = false;
        }
    }

    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
        _sliding = false;
        deferSlide = false;
    }

    private void Movement() {
        _isPlayerOnSlope = IsOnSlope();

        //Extra check for transitioning directly to a slope slide when crouching on a slope
        if (_isPlayerOnSlope && crouching && !_sliding) _sliding = true;

        //If player pressed crouch while moving through the air and is still holding crouch when they land, perform slide
        if (crouching && deferSlide && _grounded) StartSlide();

        //Extra gravity
        if (!_wallrunning) rb.AddForce(Vector3.down * Time.deltaTime * 10);
        
        //Find actual velocity relative to where player is looking
        Vector2 mag = FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        CounterMovement(x, y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && jumping) Jump();

        //Set max speed -> TODO: Stress test this for bugs
        if (_sliding) _maxSpeed = crouchSlideMaxSpeed; //Note that this does not cap momentum-building slides
        else if (crouching && !_sliding) _maxSpeed = crouchWalkMaxSpeed;
        else if (sprinting && !crouching) _maxSpeed = sprintMaxSpeed;
        else _maxSpeed = walkMaxSpeed;

        if (_sliding && rb.velocity.magnitude < 0.1) _sliding = false; //Transition out of sliding
        else if (_sliding && !_isPlayerOnSlope) return; //Block input while sliding (regular crouch slide, not a down hill momentum-building slide)

        //If sliding down a ramp, add force down so player stays grounded and also builds speed. Note that this is different from a crouch slide (i.e. sliding = true)
        if (_isPlayerOnSlope && crouching && _grounded && readyToJump) {
            rb.AddForce(Vector3.down * Time.deltaTime * 6500); //TODO: Make this multiplier a parameter
            return;
        }
        
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (x > 0 && xMag > _maxSpeed) x = 0;
        if (x < 0 && xMag < -_maxSpeed) x = 0;
        if (y > 0 && yMag > _maxSpeed) y = 0;
        if (y < 0 && yMag < -_maxSpeed) y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;
        
        // Movement in air
        if (!_grounded) {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        //Check for ledge, start mantling if one is detected
        /*
        if (IsLedge())
        {
            Mantle();
            Debug.LogWarning("Ledge detected");
        }
        */

        if (_wallrunning)
        {
            float rightOrLeft = 1f; //If 1, wall is right. If -1, wall is left
            if (!_isWallRight) { rightOrLeft = -1f; }

            // Check if the player is still contacting the wall by raycasting in the direction of the wall
            bool isContactingWall = Physics.Raycast(orientation.transform.position, orientation.transform.right * rightOrLeft, wallrunRaycastLength);

            //Check if the player is moving fast enough to continue wallrunning
            //bool isMovingFastEnough = orientation.transform.InverseTransformDirection(rb.velocity).z >= stopWallrunThresholdSpeed;
            //bool isMovingFastEnough = rb.velocity.magnitude >= stopWallrunThreshold; // Necessary for allowing smooth transition for wallrunning, jumping to another wall and continuing wallrunning

            if (!isContactingWall || _grounded) _wallrunning = false; //Transition out of state
            else
            {
                //TODO: Constrain player's movement to along the plane of the wall(?)
                _wallrunFrames++;
            }
        }
        else
        {
            //Normal movement: apply forces to move player
            rb.AddForce(orientation.transform.forward * y * movementForce * Time.deltaTime * multiplier * multiplierV);
            rb.AddForce(orientation.transform.right * x * movementForce * Time.deltaTime * multiplier);
        }

        //Clamp velocity. Should prevent player from being flung out of map by unforseen force multiplication bugs
        if (rb.velocity.magnitude > _terminalVelocity)
        {
            Vector3 old = rb.velocity;
            old = old.normalized;
            old *= _terminalVelocity;
            rb.velocity = new Vector3(old.x, old.y, old.z);
        }
    }

    private void Jump() {

        if (readyToJump) {

            readyToJump = false; //Block player from jumping while jump is in progress

            if (_grounded)
            {
                //Add vertical jump forces
                rb.AddForce(Vector2.up * jumpForce * 1.5f);
                rb.AddForce(normalVector * jumpForce * 0.5f);
            }
            else if (_wallrunning)
            {
                //Add forces to jump off wall
                Vector3 jumpOffForce = (_meanSurfaceImpactNormal * jumpForce * xWallJumpForceMult) + (Vector3.up * jumpForce * yWallJumpForceMult) + (orientation.transform.forward * jumpForce * zWallJumpForceMult);
                rb.AddForce(jumpOffForce);
            }

            Invoke(nameof(ResetJump), jumpCooldown); //Reset jump flag after short cooldown
        }
    }
    
    private void ResetJump() {
        readyToJump = true;
    }
    
    private float desiredX;
    private void Look() {
        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + yaw;
        
        //Rotate, and also make sure we dont over- or under-rotate.
        xRotation -= pitch;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Handle tilting the camera left/right when entering wallrun, resetting back to neutral when out of wallrun
        if (_wallrunning)
        {
            if (_isWallRight) zRotation += wallrunCameraAngleTransitionRate * Time.deltaTime; //If the wall is to our right, want to add to z rotation to tilt to the left
            else zRotation -= wallrunCameraAngleTransitionRate * Time.deltaTime; //If the wall is to our left, want to subtract to tilt to the right
            zRotation = Mathf.Clamp(zRotation, -wallrunCameraAngle, wallrunCameraAngle);
            /*
            if (_isWallRight) zRotation = Mathf.Lerp(0, wallrunCameraAngle, orientation.transform.InverseTransformDirection(rb.velocity).y / 9);
            else zRotation = Mathf.Lerp(-wallrunCameraAngle, 0, orientation.transform.InverseTransformDirection(rb.velocity).y / 9);
            zRotation = Mathf.Clamp(zRotation, -wallrunCameraAngle, wallrunCameraAngle);
            */
        }
        else
        {
            // This resets the camera when finished wall running
            if (-wallrunCameraAngleTransitionRate * Time.deltaTime < zRotation && zRotation <= wallrunCameraAngleTransitionRate * Time.deltaTime) zRotation = 0; //If the zRotation is within a range of tolerance, just set it to zero
            else if (zRotation > 0) zRotation -= wallrunCameraAngleTransitionRate * Time.deltaTime;
            else if (zRotation < 0) zRotation += wallrunCameraAngleTransitionRate * Time.deltaTime;
        }

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, zRotation);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    private void CounterMovement(float x, float y, Vector2 mag) {
        if (!_grounded || jumping) return;

        //Slow down sliding
        if (_sliding) {
            rb.AddForce(movementForce * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
            return;
        }

        //Counter movement
        if (Math.Abs(mag.x) > counterMovementThreshold && Math.Abs(x) < 0.05f || (mag.x < -counterMovementThreshold && x > 0) || (mag.x > counterMovementThreshold && x < 0)) {
            rb.AddForce(movementForce * orientation.transform.right * Time.deltaTime * -mag.x * counterMovement);
        }
        if (Math.Abs(mag.y) > counterMovementThreshold && Math.Abs(y) < 0.05f || (mag.y < -counterMovementThreshold && y > 0) || (mag.y > counterMovementThreshold && y < 0)) {
            rb.AddForce(movementForce * orientation.transform.forward * Time.deltaTime * -mag.y * counterMovement);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > _maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * _maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }

    //Find the velocity relative to where the player is looking
    //Useful for vectors calculations regarding movement and limiting movement
    public Vector2 FindVelRelativeToLook() {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitue = rb.velocity.magnitude;
        float yMag = magnitue * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitue * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector2(xMag, yMag);
    }

    private bool IsFloor(Vector3 v) {
        float angle = Vector3.Angle(Vector3.up, v);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    
    //Handle ground detection
    private void OnCollisionStay(Collision other) {
        //Make sure we are only checking for walkable layers
        int layer = other.gameObject.layer;
        if (whatIsGround != (whatIsGround | (1 << layer))) return;

        //Iterate through every collision in a physics update
        for (int i = 0; i < other.contactCount; i++) {
            Vector3 normal = other.contacts[i].normal;
            //FLOOR
            if (IsFloor(normal)) {
                _grounded = true;
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

    private void StopGrounded()
    {
        _grounded = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        //Check if player is transitioning into wallrunning state
        if (CanStartWallrun(collision)) StartWallrun();
    }

    private void StartWallrun()
    {
        _wallrunFrames = 0;
        _wallrunning = true; //Set state flag
        _maxSpeed = wallrunMaxSpeed;
        _isWallRight = Physics.Raycast(orientation.transform.position, orientation.transform.right, wallrunRaycastLength); // Determine to which side of the player the wall is

        //Apply an initial instantaneous force upwards and forwards relative to the player's current direction, projected onto the plane of the wall
        Vector3 hProj = orientation.transform.forward - Vector3.Dot(orientation.transform.forward, _meanSurfaceImpactNormal) * _meanSurfaceImpactNormal; //Project the player's forward vector onto the plane of the wall
        Vector3 hForce = hProj * hWallBoostForce * orientation.transform.InverseTransformDirection(rb.velocity).z;
        Vector3 vForce = orientation.transform.up * vWallBoostForce; //This vertical force is key to giving wallrunning a nice arc motion
        rb.AddForce(hForce + vForce);
    }

    private bool CanStartWallrun(Collision collision)
    {
        //First check if we are colliding against a wall
        if (Physics.CheckSphere(wallContactCheck.position, wallContactCheckRadius, whatIsGround) && collision.collider.tag == WALL_TAG_NAME)
        {
            //Calculate the mean surface impact normal
            Vector3 meanSurfaceImpactNormal = new Vector3();

            foreach (ContactPoint c in collision.contacts) meanSurfaceImpactNormal += c.normal;

            meanSurfaceImpactNormal /= collision.contacts.Length; //Calculate the average impact normal
            _meanSurfaceImpactNormal = meanSurfaceImpactNormal.normalized; //Get the re-normalized value, store in variable so it can be viewable for debugging
            float angleOfApproach = Mathf.Acos(Vector3.Dot(meanSurfaceImpactNormal.normalized, orientation.transform.forward)) * Mathf.Rad2Deg; //The angle at which the player contacts the wall

            bool isImpactHorizontal = meanSurfaceImpactNormal.y >= -impactNormalYThreshold && meanSurfaceImpactNormal.y <= impactNormalYThreshold; //If the surface impact normal is mostly horizontal i.e. low y component 
            bool isGoodApproachAngle = angleOfApproach >= 45f && angleOfApproach <= 160f; // The angle between orientation.transform.forward and meanSurfaceImpactNormal is within a threshold

            return isImpactHorizontal && isGoodApproachAngle && !_grounded;
        }
        else return false;
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
        rb.velocity = new Vector3(); //Reset velocity
        rb.AddForce(orientation.transform.up * upwardMantleForce + orientation.transform.forward * forwardMantleForce); //Apply instantaneous upward and forward force
    }

    private bool IsOnSlope()
    {
        if (!_grounded) return false;
        RaycastHit hit;

        //Raycast down, get the normal vector of the surface we're standing on
        if (!Physics.Raycast(transform.position, Vector3.down, out hit, 1.5f)) return false; //TODO: Make distance not a magic number. Also this return is probably redundant

        //Calculate the angle between the ground surface normal and our forward vector
        float slope = Vector3.Angle(hit.normal, orientation.transform.forward) - 90f;

        return slope <= slopeThreshold;
    }
}
