using UnityEngine;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float vx = rb.linearVelocity.x;
        bool moving = Mathf.Abs(vx) > 0.1f;

        if (moving)
            spriteRenderer.flipX = vx < 0; // sprites must face right, or the enemy will moonwalk
    }
}