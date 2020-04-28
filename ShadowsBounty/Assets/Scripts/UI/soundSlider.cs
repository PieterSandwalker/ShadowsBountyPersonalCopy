using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundSlider : MonoBehaviour
{
    AudioSource source;

    void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();
    }
    public void SetVolume(float volumes)
    {
        Debug.Log(volumes);
        source.volume = volumes;
    }
}
