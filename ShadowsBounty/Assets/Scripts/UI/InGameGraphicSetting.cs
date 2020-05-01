using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameGraphicSetting : Bolt.EntityBehaviour<IPlayerMoveState>
{
    public Canvas canvas;
    public Canvas option;

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropDown;

    AudioSource source;
    private void Start()
    {
        source = this.gameObject.GetComponent<AudioSource>();

        resolutions = Screen.resolutions;
        
        resolutionDropDown.ClearOptions();

        List<string> options = new List<string>();

        int currentIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
            {
                currentIndex = i;
            }
        }

        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentIndex;
        resolutionDropDown.RefreshShownValue();
    }

    public void SetQuality(int qualityIndex)
    {
        if (entity.IsOwner) {
            QualitySettings.SetQualityLevel(5 - qualityIndex);
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if (entity.IsOwner)
        {
            Resolution resolution = resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }

    public void SetFullScreen(bool isFullscreen)
    {
        if (entity.IsOwner)
        {
            Screen.fullScreen = isFullscreen;
        }
    }
    public void SetVolume(float volumes)
    {
        if (entity.IsOwner)
        {
            Debug.Log(volumes);
            source.volume = volumes;
        }
    }

    public void Option()
    {
        if (entity.IsOwner)
        {
            canvas.gameObject.SetActive(false);
            option.gameObject.SetActive(true);
        }
    }

    public void Back()
    {
        if (entity.IsOwner)
        {
            canvas.gameObject.SetActive(true);
            option.gameObject.SetActive(false);
        }
    }
}
