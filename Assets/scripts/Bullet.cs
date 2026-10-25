using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f;
    public Rigidbody2D rb;

    public void Fire(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
    }

}
