using UnityEngine;

public class MeowlMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public float xInput;

    [Header("Speed")]
    public float minSpeed = 5;          // speed you start at
    public float baseMaxSpeed = 15;     
    public float accelRate = 5f;        // units gained while accelerating
    public float brakeRate = 20f;       // units lost when reversing direction
    public float releaseRate = 40f;     // units lost when you let go of the keys

    [Header("Speed boost")]
    public float boostDelay = 1.5f;     // seconds at top speed before the extra boost
    public float boostAmount = 2f;      // how much the max speed goes up
    public int maxBoosts = 1;           // how many times the extra boost can happen

    [Header("Info/Debug")]
    public float velX;
    public float currentMax;
    public int boostsDone;
    public float boostTimer;

    [Header("Jump/Ground")]
    public float jumpForce = 5;
    public Transform feetPos;
    public bool isGrounded;
    public LayerMask groundLayer;

    public float damageCD;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentMax = baseMaxSpeed;
    }

    void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(feetPos.position, 0.15f, groundLayer);

        if (damageCD <= 0)
        {
            HandleHorizontal();
            rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);
        }
        else
        {
            damageCD -= Time.deltaTime;
            velX = rb.linearVelocity.x;   // stay in sync with knockback
            ResetBoost();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void HandleHorizontal()
    {
        bool reversing = xInput != 0 && velX != 0 && Mathf.Sign(velX) != xInput;

        if (xInput == 0)
        {
            // No input: slow down and stop boost progression
            velX = Mathf.MoveTowards(velX, 0, releaseRate * Time.deltaTime);
            ResetBoost();
        }
        else if (reversing)
        {
            // Opposite direction held: remove speed up to 0
            velX = Mathf.MoveTowards(velX, 0, brakeRate * Time.deltaTime);
            ResetBoost();
        }
        else
        {
            // Moving the way the player's holding
            if (Mathf.Abs(velX) < minSpeed)
                velX = xInput * minSpeed;

            if (isGrounded)
            {
                float speed = Mathf.MoveTowards(Mathf.Abs(velX), currentMax, accelRate * Time.deltaTime);
                velX = xInput * speed;
                HandleBoost();
            }
        }
    }

    void HandleBoost()
    {
        if (boostsDone >= maxBoosts) return;

        if (Mathf.Abs(velX) >= currentMax - 0.01f)   // at top speed
        {
            boostTimer += Time.deltaTime;
            if (boostTimer >= boostDelay)
            {
                currentMax += boostAmount;            // new max speed
                velX = xInput * currentMax;           // instant jump to new speed
                boostsDone++;
                boostTimer = 0;
            }
        }
    }

    void ResetBoost()
    {
        currentMax = baseMaxSpeed;
        boostsDone = 0;
        boostTimer = 0;
    }
}