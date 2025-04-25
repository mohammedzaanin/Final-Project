using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
   public Transform target;             // The player
    public Vector2 sensitivity = new Vector2(2f, 2f);
    public float distance = 6f;          // Distance behind the player
    public float yOffset = 2f;           // Height offset from player's position
    public float minY = -30;            // Look down limit
    public float maxY = 60f;             // Look up limit

    private float rotationX = 10f;       // Vertical rotation (pitch)
    private float rotationY = 0f;        // Horizontal rotation (yaw)

    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity.x;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity.y;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, minY, maxY); // Limit vertical look

        // Get camera rotation
        Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0);
        Vector3 targetPosition = target.position + Vector3.up * yOffset;

        // Set camera position
        transform.position = targetPosition - rotation * Vector3.forward * distance;
        transform.LookAt(targetPosition);
    }
}
