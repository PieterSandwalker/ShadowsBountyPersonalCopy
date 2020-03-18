using System;
using System.Diagnostics;
using Unity.Collections;
using UnityEngine;

/* BUGS */
//Grappling hook model shrinks when you crouch
    //Solution: May need to make the player an empty object with the capsule as a component so that only the capsule's size changes
    //Another hackier solution: check if the player is crouching, double the scale of the gun. If they uncrouch, halve it again

/* TODO */
//Add crosshair with script to tell when you're aiming at something you can grapple to
//Possibly add a sphere on the location where you are grappling to

public class GrapplingHook : MonoBehaviour {
    [Header("Grappling")]
    public GrapplingRope grapplingRope;
    public PlayerMovement2 player; //Change to PlayerMovement2, reference rigid body thru player.rb
    public Transform grappleTip;
    public Transform grappleHolder;
    public int whatToGrapple;
    public float maxDistance;
    public float minDistance;
    public float rotationSmooth;

    public Camera playerCamera; //Direct reference to the actual camera component on the player
    public Transform playerOrientation;

    [Header("Raycasts")]
    public float raycastRadius;
    public int raycastCount;

    [Header("Physics")]
    public float pullForce;
    public float pushForce;
    public float yMultiplier;
    public float minPhysicsDistance;
    public float maxPhysicsDistance;
        
    private Vector3 _hit;

    private Vector3 _modelScale;
    private Vector3 _crouchScale = new Vector3(1f, 0.5f, 1f);

    bool lmbPressed = false;

    private void Start()
    {
        _modelScale = transform.localScale;
    }

    private void MyInput()
    {
        if (Input.GetMouseButtonDown(0)) lmbPressed = true;
        else if (Input.GetMouseButtonUp(0)) lmbPressed = false;
    }

    private void Update()
    {
        MyInput();

        //Bandaid fix for scaling bug with grappling gun model. Doesn't work completely. To avoid scaling issues completely would need to restructure player prefab so that the grappling gun wasn't a child of the player capsule
        //Good enough. Scaling weirdness is barely noticeable in first person view.
        if (player.transform.localScale.Equals(_crouchScale))
        {
            //TODO: Need to figure out how much rotation there is about x, use that value to scale in y and z
            //      i.e. if looking straight forward, y_scale = 2, z_scale = 1; if looking straight up, y_scale = 1, z_scale = 2

            float xRot = player.playerCam.transform.rotation.eulerAngles.x;
            if (xRot > 90) xRot = 360 - xRot;
            float xRotNormalized = xRot / 90; //Use as alpha for lerp -> if xRotNormd = 0, y should be 2 and z should be 1. If xRotNormd = 1, y should be 1 and z should be 2
            transform.localScale = new Vector3(1, Mathf.Lerp(2, 1, xRotNormalized), Mathf.Lerp(1, 2, xRotNormalized));
        }
        else
        {
            transform.localScale = _modelScale;
        }
    }

    private void FixedUpdate() {
        if (lmbPressed && grapplingRope.Grappling) { //Check if grapple input button is being pressed
            grappleHolder.rotation = Quaternion.Lerp(grappleHolder.rotation, Quaternion.LookRotation(-(grappleHolder.position - _hit)), rotationSmooth * Time.fixedDeltaTime); //Orient the grappling hook towards its hit location

            //TODO: Use playerOrientation to get transform info for player

            //Apply forces to player
            var distance = Vector3.Distance(playerOrientation.position, _hit);
            if (!(distance >= minPhysicsDistance) || !(distance <= maxPhysicsDistance)) return;
            player.rb.velocity += pullForce * Time.fixedDeltaTime * yMultiplier * Mathf.Abs(_hit.y - playerOrientation.position.y) * (_hit - playerOrientation.position).normalized;
            player.rb.velocity += pushForce * Time.fixedDeltaTime * playerOrientation.forward;
        }
        else {
            grappleHolder.localRotation = Quaternion.Lerp(grappleHolder.localRotation, Quaternion.Euler(0, 0, 0), rotationSmooth * Time.fixedDeltaTime);
        }
    }

    private void LateUpdate()
    {
        //Attach grapple

        var hitInfo = new RaycastHit();

        if (Input.GetMouseButtonDown(0) && RaycastAll(out hitInfo)) { //Only call the frame that grapple input button is initially pressed
            grapplingRope.Grapple(grappleTip.position, hitInfo.point);    
            _hit = hitInfo.point;
        }

        //Release grapple
        if (Input.GetMouseButtonUp(0)) { //Only call the frame that grapple input button is released
            grapplingRope.UnGrapple();
        }

        //Init start position for grapple
        if (lmbPressed && grapplingRope.Grappling) { //Check if grapple input button is being pressed
            grapplingRope.UpdateStart(grappleTip.position);
        }

        grapplingRope.UpdateGrapple();
    }

    //Used to attach grapple within a tolerance range
    private bool RaycastAll(out RaycastHit hit) {
        var divided = raycastRadius / 2f;
        var possible = new NativeList<RaycastHit>(raycastCount * raycastCount, Allocator.Temp);
        var cam = playerCamera.transform; //Player camera ref

        for (var x = 0; x < raycastCount; x++) {
            for (var y = 0; y < raycastCount; y++) {
                var pos = new Vector2(
                    Mathf.Lerp(-divided, divided, x / (float)(raycastCount - 1)),
                    Mathf.Lerp(-divided, divided, y / (float)(raycastCount - 1))
                );
                    
                if (!Physics.Raycast(cam.position + cam.right * pos.x + cam.up * pos.y, cam.forward, out var hitInfo, maxDistance)) continue;
                    
                var distance = Vector3.Distance(cam.position, hitInfo.point);
                if (hitInfo.transform.gameObject.layer != whatToGrapple) continue;
                if (distance < minDistance) continue;
                if (distance > maxDistance) continue;
                    
                possible.Add(hitInfo);
            }
        }

        var arr = possible.ToArray();
        possible.Dispose();

        if (arr.Length > 0) {
            var closest = new RaycastHit();
            var distance = 0f;
            var set = false;

            foreach (var hitInfo in arr) {
                var hitDistance = DistanceFromCenter(hitInfo.point);

                if (!set) {
                    set = true;
                    distance = hitDistance;
                    closest = hitInfo;
                }
                else if (hitDistance < distance) {
                    distance = hitDistance;
                    closest = hitInfo;
                }
            }
                
            hit = closest;
            return true;
        }
            
        hit = new RaycastHit();
        return false;
    }
        
    private float DistanceFromCenter(Vector3 point) {
        return Vector2.Distance(playerCamera.WorldToViewportPoint(point), //Player cam ref
            new Vector2(0.5f, 0.5f));
    }
}
