using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;            // drags the player
    public BoxCollider2D mapBounds;     // the box covering the whole map (including walls, floor and ceiling)

    public float smoothTime = 0.25f;    // How lazy the camera is
    public Vector2 offset;              // offset between the player and the camera

    Camera cam;
    Vector3 velocity;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (target == null || mapBounds == null) return;

        // Camera's intended place
        Vector3 desired = new Vector3(
            target.position.x + offset.x,
            target.position.y + offset.y,
            transform.position.z);

        desired = ClampToBounds(desired);

        // Glide towards said place
        Vector3 smoothed = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);

        transform.position = ClampToBounds(smoothed);
    }

    Vector3 ClampToBounds(Vector3 pos)
    {
        Bounds b = mapBounds.bounds;

        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        float minX = b.min.x + halfW;
        float maxX = b.max.x - halfW;
        float minY = b.min.y + halfH;
        float maxY = b.max.y - halfH;

        pos.x = (minX > maxX) ? b.center.x : Mathf.Clamp(pos.x, minX, maxX);
        pos.y = (minY > maxY) ? b.center.y : Mathf.Clamp(pos.y, minY, maxY);
        return pos;
    }
}