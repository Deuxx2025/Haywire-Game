using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator animator;
    private MeowlMovement movement;
    private bool facingRight = true;

    [Header("Run animation speed")]
    public float minAnimSpeed = 0.8f;   // playback speed at minSpeed
    public float maxAnimSpeed = 1.5f;     // playback speed at top speed

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        movement = GetComponent<MeowlMovement>();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        if (moveX > 0) facingRight = true;
        else if (moveX < 0) facingRight = false;

        animator.SetFloat("Speed", Mathf.Abs(moveX));
        animator.SetBool("FacingRight", facingRight);

        float runSpeed = 0f;

        if (movement.isGrounded)
        {
            float speed = Mathf.Abs(movement.velX);
            float t = Mathf.InverseLerp(movement.minSpeed, movement.baseMaxSpeed, speed);
            runSpeed = Mathf.Lerp(minAnimSpeed, maxAnimSpeed, t);
        }

        animator.SetFloat("RunSpeed", runSpeed);
    }
}