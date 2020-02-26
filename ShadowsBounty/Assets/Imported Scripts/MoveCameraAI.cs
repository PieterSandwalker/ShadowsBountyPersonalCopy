using UnityEngine;

public class MoveCameraAI : MonoBehaviour {

    public Transform player;

    void Update() {
        transform.position = player.transform.position;
    }
}
