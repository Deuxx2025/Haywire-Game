using UnityEngine;

public class MeowlMovement : MonoBehaviour
{
    public Rigidbody2D rb;

    public float xInput;

    [Header("Speed")]
    public float minSpeed = 5;
    public float baseMaxSpeed = 15;
    public float accelRate = 5f;
    public float brakeRate = 20f;
    public float releaseRate = 40f;

    [Header("Speed boost")]
    public float boostDelay = 1.5f;
    public float boostAmount = 2f;
    public int maxBoosts = 1;

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
        CheckGround();

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }

    void FixedUpdate()
    {
        CheckGround();

        if (damageCD <= 0)
        {
            // Blocked by a wall = Drop momentum
            if (xInput != 0 && Mathf.Abs(rb.linearVelocity.x) < 0.1f && Mathf.Abs(velX) > 0.1f)
            {
                velX = 0;
                ResetBoost();
            }

            HandleHorizontal();
            rb.linearVelocity = new Vector2(velX, rb.linearVelocity.y);
        }
        else
        {
            damageCD -= Time.fixedDeltaTime;
            velX = rb.linearVelocity.x;
            ResetBoost();
        }
    }

    void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(feetPos.position, 0.15f, groundLayer);
    }

    void HandleHorizontal()
    {
        bool reversing = xInput != 0 && velX != 0 && Mathf.Sign(velX) != xInput;

        if (xInput == 0)
        {
            velX = Mathf.MoveTowards(velX, 0, releaseRate * Time.fixedDeltaTime);
            ResetBoost();
        }
        else if (reversing)
        {
            velX = Mathf.MoveTowards(velX, 0, brakeRate * Time.fixedDeltaTime);
            ResetBoost();
        }
        else
        {
            if (Mathf.Abs(velX) < minSpeed)
                velX = xInput * minSpeed;

            if (isGrounded)
            {
                float speed = Mathf.MoveTowards(Mathf.Abs(velX), currentMax, accelRate * Time.fixedDeltaTime);
                velX = xInput * speed;
                HandleBoost();
            }
        }
    }

    void HandleBoost()
    {
        if (boostsDone >= maxBoosts) return;

        if (Mathf.Abs(velX) >= currentMax - 0.01f)
        {
            boostTimer += Time.fixedDeltaTime;
            if (boostTimer >= boostDelay)
            {
                currentMax += boostAmount;
                velX = xInput * currentMax;
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