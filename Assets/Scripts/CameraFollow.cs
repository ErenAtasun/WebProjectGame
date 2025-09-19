using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // takip edilecek player
    public Vector3 offset = new Vector3(0, 0, -10); // kamera player�n arkas�nda
    public float smoothSpeed = 0.125f; // yumu�ak hareket

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }

}
