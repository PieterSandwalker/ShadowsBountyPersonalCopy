using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDetectionStats : MonoBehaviour
{
    private float audibleFactor = 0.0f;
    private float visibiityFactor = 55.0f;
    public int detected = 0;

    public float VisibiityFactor { get => visibiityFactor; set => visibiityFactor = value; }
    public float AudibleFactor { get => audibleFactor; set => audibleFactor = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
