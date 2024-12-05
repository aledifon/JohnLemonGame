using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public int smoothing;       // Following camera speed

    Vector3 offset;             // Init distance between the camera and the player
    void Start()
    {
        offset = transform.position - player.position;   
    }

    // The camera position is normally calculated on the LateUpdate
    // 1st. The player is moved on the Update or FixedUpdate
    // 2nd. The camera is moved on the LateUpdate
    void LateUpdate()
    {
        // Position which I want to move the camera
        Vector3 desiredPosition = player.position + offset;

        // Camera movement
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 
                                        smoothing * Time.deltaTime);
    }
}
