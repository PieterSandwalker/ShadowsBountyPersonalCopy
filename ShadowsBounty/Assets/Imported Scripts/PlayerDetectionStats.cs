using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionStats : MonoBehaviour
{
    public float audibleFactor = 0.0f;
    public float visibilityFactor = 0.0f;
    public int detected = 0;
    public GameObject[] lightPoints;
    public GameObject skyLight;
    public string lightTag = "Light";
    public string skylightTag = "SkyLight";
    public float lightBalance = 100.0f;
    public float noMovementThreshold = 2.0f;
    public float minimalMovementThreshold = 7.0f;
    public float overtMovementThreshold = 12.0f;

    public float zeroNoticibility = 0.0f;
    public float minimalNoticibility = 10.0f;
    public float overtNoticibility = 30.0f;
    public float obnoxiousNoticibility = 50.0f;

    public float standNoticibility = 7.0f;
    public float crouchNoticibility = 2.0f;
    public float slideNoticibility = 4.0f;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    public float skyLightWeight = 100.0f;

    public float speedUnitWeight = 1.0f;
    public float visibilityWeight = 1.0f;
    public float audibilityWeight = 1.0f;
    public float VisibilityFactor { get => visibilityFactor; set => visibilityFactor = value; }
    public float AudibleFactor { get => audibleFactor; set => audibleFactor = value; }

    // Start is called before the first frame update
    void Start()
    {
        lightPoints = GameObject.FindGameObjectsWithTag(lightTag);
        skyLight = GameObject.FindGameObjectsWithTag(skylightTag)[0];
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVisibility();
        UpdateAudibility();
    }

    void UpdateVisibility()
    {
        float readLightValue = 0.0f;

        // Bit shift the index of the layer (13) to get a bit mask
        int layerMask = 1 << 13;

        // This would cast rays only against colliders in layer 8.
        // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
        layerMask = ~layerMask;

        foreach (var light in lightPoints)
        {
            float currentdistance = Vector3.Distance(gameObject.transform.position, light.transform.position);


            RaycastHit currenthit;
            Vector3 currentdirection = (gameObject.transform.position - light.transform.position).normalized;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(light.transform.position, currentdirection, out currenthit, Mathf.Infinity, layerMask))
            {
                if (currenthit.collider.gameObject.tag == "Player")
                {
                    Light current = light.GetComponent<Light>();
                    readLightValue += current.intensity * lightBalance/currentdistance;
                }
            }
        }
        //now do for skyLight
        float distance = Vector3.Distance(gameObject.transform.position, skyLight.transform.position);

        RaycastHit hit;
        Vector3 direction = (gameObject.transform.position - skyLight.transform.position).normalized;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(skyLight.transform.position, direction, out hit, Mathf.Infinity, layerMask))
        {
            if (hit.collider.gameObject.tag == "Player")
            {
                Light current = skyLight.GetComponent<Light>();
                readLightValue += current.intensity * lightBalance * skyLightWeight/ distance;
            }
        }

        float readStatusValue = 0.0f;
        PlayerMovement2 status = GetComponent<PlayerMovement2>();
        if (transform.localScale == crouchScale)
        {
            readStatusValue += crouchNoticibility;
        }
        else if (status.sliding)
        {
            readStatusValue += slideNoticibility;
        }
        else
        {
            readStatusValue += standNoticibility;
        }

        visibilityFactor = visibilityWeight*(readLightValue + readStatusValue); //add arbitrary weight

    }
    void UpdateAudibility()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        //if (rb.velocity.magnitude <= noMovementThreshold) audibleFactor = zeroNoticibility;
        //else if (rb.velocity.magnitude <= minimalMovementThreshold) audibleFactor = minimalNoticibility;
        //else if (rb.velocity.magnitude <= overtMovementThreshold) audibleFactor = overtNoticibility;
        //else audibleFactor = obnoxiousNoticibility;
        audibleFactor = rb.velocity.magnitude * speedUnitWeight;
        audibleFactor *= audibilityWeight;

    }
}
