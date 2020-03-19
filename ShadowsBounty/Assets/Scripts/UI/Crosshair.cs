using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour {
    public Transform playerCamera;
    public Image crosshairImage;
    public Color notFoundColor;
    public Color foundColor;
    public float colorSpeed;

    private void Update() {

        //TODO: Add check for if we're looking at something that can be grappled onto
        crosshairImage.color = Color.Lerp(crosshairImage.color,
            Physics.Raycast(playerCamera.position, playerCamera.forward) ? foundColor : notFoundColor,
            colorSpeed * Time.deltaTime);
    }
}