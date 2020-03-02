using UnityEngine;

// C# example.

public class RaycastAiming : MonoBehaviour
{
    public Transform CastOrigin;

    //values that will be set in the Inspector
    public Vector3 HitLocation;
    public float RotationSpeed = 0.01f;

    //values for internal use
    private Quaternion _lookRotation;
    private Vector3 _direction;

    void Update()
    {

        // Bit shift the index of the layer (8) to get a bit mask
        int layerMask = 1 << 13;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(CastOrigin.transform.position, CastOrigin.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
            HitLocation = hit.point;
        }
        else
        {
            Debug.DrawRay(CastOrigin.transform.position, CastOrigin.transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
        //find the vector pointing from our position to the target
        _direction = (HitLocation - transform.position).normalized;
        //create the rotation we need to be in to look at the target
        _lookRotation = Quaternion.LookRotation(_direction);

        //rotate us over time according to speed until we are in the required rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, _lookRotation, Time.deltaTime * RotationSpeed);
    }
}