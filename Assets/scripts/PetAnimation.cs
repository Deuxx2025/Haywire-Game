using UnityEngine;

public class PetAnimation : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (animator == null) animator = GetComponentInChildren<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        float vx = rb.linearVelocity.x;
        bool moving = Mathf.Abs(vx) > 0.1f;

        animator.SetBool("IsMoving", moving);

        if (moving)
            spriteRenderer.flipX = vx < 0; // ALWAYS MAKE SPRITES FACE RIGHT OTHERWISE CHARACTER WILL MOONWALK
    }
}