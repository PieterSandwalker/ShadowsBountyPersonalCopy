using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootsteps : MonoBehaviour
{
    /* Most of this code is based on this tutorial: https://www.youtube.com/watch?v=Tb3NGKgq1t8 */
    public PlayerMovement2 player;
    public AudioSource audioSource;

    public float modifier = 0.5f;

    public AudioClip[] stoneAudioClips;
    public AudioClip[] grassAudioClips;
    public AudioClip[] woodAudioClips;

    private AudioClip previousAudioClip;
    private float airTime;
    private float distanceCovered;

    void Update()
    {
        PlaySoundIfFallling();
        if (IsPlayerWalking()) distanceCovered += player.rb.velocity.magnitude * Time.deltaTime * modifier;
        if (distanceCovered > 2f)
        {
            TriggerNextClip();
            distanceCovered = 0f;
        }
    }

    private bool IsPlayerWalking()
    {
        return player.rb.velocity.magnitude > 0 && (player.grounded || player.wallrunning) && !player.sliding;
    }

    AudioClip GetAudioClipFromArray(AudioClip[] audioClips)
    {
        int attempts = 3;
        AudioClip selectedClip = audioClips[Random.Range(0, audioClips.Length - 1)];

        //Get a random audio clip; try to make it not be the same as the last clip played
        while (selectedClip == previousAudioClip && attempts > 0)
        {
            selectedClip = audioClips[Random.Range(0, audioClips.Length - 1)];
            attempts--;
        }

        //Update previous clip
        previousAudioClip = selectedClip;

        return selectedClip;
    }

    void TriggerNextClip()
    {
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.volume = Random.Range(0.8f, 1.1f);

        if (IsPlayerWalking())
        {
            audioSource.PlayOneShot(GetAudioClipFromArray(stoneAudioClips), 1f); //For now just use stone footsteps sound for everything; will try to add material detection later
        }
    }

    void PlaySoundIfFallling()
    {
        if (!player.grounded) airTime += Time.deltaTime;
        else if (airTime > 0.25f)
        {
            TriggerNextClip();
            airTime = 0f;
        }
    }
}
