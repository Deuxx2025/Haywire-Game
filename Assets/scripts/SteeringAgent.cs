using System.IO;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SteeringAgent : MonoBehaviour
{
    public enum Behavior { Seek, Arrive, Wander, Flee, Evade, Pursue}

    [Header("Setup")]
    public Transform target;
    public Behavior behavior = Behavior.Seek;

    [Header("Detection")]
    public float detectionRadius = 600f;

    [Header("Movement")]
    public float maxSpeed = 5f; //Enemy's maximum speed
    public float maxForce = 20f; //how sharply the enemy can change direction
    public float slowingRadius = 3f; //Arrive Only
    public float stopDistance = 0.1f; //Arrive only

    [Header("Jumping")]
    public LayerMask groundLayer;
    public float jumpForce = 12f;
    public float jumpCooldown = 0.4f;
    public float obstacleCheckDistance = 0.6f;
    public float maxObstacleHeight = 2f; // Maximum Height
    
    Collider2D col;
    float nextJumpTime;

    [Header("Wander (waypoints)")]
    public Transform[] wanderNodes;
    public float nodeReachDistance = 0.3f;
    public float wanderSpeedMultiplier = 0.5f;
    public float pauseAtNode = 1f;

    [Header("Evade/Pursue")]
    public float maxPredictionTime = 1f; // How far to predict

    int nodeIndex;
    float pauseUntil;

    Rigidbody2D rb;
    Rigidbody2D targetRb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    void Start()
    {
        if (target != null)
            targetRb = target.GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        bool playerDetected = target != null &&
            Vector2.Distance(transform.position, target.position) <= detectionRadius;

        float steering;

        switch (behavior)
        {
            case Behavior.Wander:
                steering = Wander();   // always patrols, regardless of the player
                break;

            case Behavior.Seek:
                steering = playerDetected ? Seek() : Brake();
                break;

            case Behavior.Arrive:
                steering = playerDetected ? Arrive() : Brake();
                break;

            case Behavior.Flee:
                steering = playerDetected ? Flee() : Brake();
                break;

            case Behavior.Evade:
                steering = playerDetected ? Evade() : Brake();
                if (Mathf.Abs(steering) > 0.1f)
                    TryJump(Mathf.Sign(steering));
                break;

            case Behavior.Pursue:
                steering = playerDetected ? Pursue() : Brake();
                if (Mathf.Abs(steering) > 0.1f)
                    TryJump(Mathf.Sign(steering));
                break;

            default:
                steering = Brake();
                break;
        }

        rb.AddForce(new Vector2(steering, 0f));
    }

    bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            col.bounds.center, col.bounds.size, 0f, Vector2.down, 0.1f, groundLayer);
        return hit.collider != null;
    }

    // If there's a jumpable obstacle directly ahead
    bool ObstacleAhead(float dir)
    {
        Bounds b = col.bounds;
        Vector2 size = new Vector2(b.size.x, b.size.y * 0.6f);
        Vector2 origin = new Vector2(b.center.x, b.min.y + 0.1f + size.y * 0.5f);
        Vector2 direction = new Vector2(dir, 0f);

        bool blocked = Physics2D.BoxCast(origin, size, 0f, direction,
            obstacleCheckDistance, groundLayer).collider != null;
        if (!blocked) return false;

        Vector2 raised = origin + Vector2.up * maxObstacleHeight;
        bool tooTall = Physics2D.BoxCast(raised, size, 0f, direction,
            obstacleCheckDistance, groundLayer).collider != null;

        return !tooTall;
    }

    void TryJump(float dir)
    {
        if (Time.time < nextJumpTime || !IsGrounded()) return;

        if (ObstacleAhead(dir))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            nextJumpTime = Time.time + jumpCooldown;
        }
    }

    float Seek()
    {
        float dir = Mathf.Sign(target.position.x - transform.position.x);
        float desiredVelX = dir * maxSpeed;
        float steer = desiredVelX - rb.linearVelocity.x;
        return Mathf.Clamp(steer, -maxForce, maxForce);
    }

    float Arrive()
    {
        float offset = target.position.x - transform.position.x;
        float distance = Mathf.Abs(offset);

        if (distance < stopDistance)
        {
            return Mathf.Clamp(-rb.linearVelocity.x, -maxForce, maxForce);
        }

        float speed = distance < slowingRadius
            ? maxSpeed * (distance / slowingRadius)
            : maxSpeed;

        float desiredVelX = Mathf.Sign(offset) * speed;
        float steer = desiredVelX - rb.linearVelocity.x;
        return Mathf.Clamp(steer, -maxForce, maxForce);
    }

    float Flee()
    {
        float dir = -Mathf.Sign(target.position.x - transform.position.x);
        float desiredVelX = dir * maxSpeed;
        float steer = desiredVelX - rb.linearVelocity.x;
        return Mathf.Clamp(steer, -maxForce, maxForce);
    }

    float Wander()
    {
        Debug.Log($"Wander running | nodes: {(wanderNodes == null ? "null" : wanderNodes.Length.ToString())} | velocity.x: {rb.linearVelocity.x:F2}");

        if (wanderNodes == null || wanderNodes.Length == 0)
            return Brake();

        // Standing still at a node
        if (Time.time < pauseUntil)
            return Brake();

        float offset = wanderNodes[nodeIndex].position.x - transform.position.x;

        // Reached the node: go to the next one
        if (Mathf.Abs(offset) <= nodeReachDistance)
        {
            nodeIndex = (nodeIndex + 1) % wanderNodes.Length;
            pauseUntil = Time.time + pauseAtNode;
            return Brake();
        }

        float desiredVelX = Mathf.Sign(offset) * maxSpeed * wanderSpeedMultiplier;
        float steer = desiredVelX - rb.linearVelocity.x;
        return Mathf.Clamp(steer, -maxForce, maxForce);
    }

    float Evade()
    {
        float distance = Mathf.Abs(target.position.x - transform.position.x);

        // Predict how long it would take the target to reach the player
        float targetSpeed = targetRb != null ? Mathf.Abs(targetRb.linearVelocity.x) : 0f;
        float predictionTime = targetSpeed > 0.01f
            ? Mathf.Min(distance / targetSpeed, maxPredictionTime)
            : 0f;

        // Where the target will be
        float futureX = target.position.x +
            (targetRb != null ? targetRb.linearVelocity.x : 0f) * predictionTime;

        // Flee from that predicted position
        float dir = -Mathf.Sign(futureX - transform.position.x);
        float desiredVelX = dir * maxSpeed;
        float steer = desiredVelX - rb.linearVelocity.x;
        return Mathf.Clamp(steer, -maxForce, maxForce);
    }

    float Pursue()
    {
        float distance = Mathf.Abs(target.position.x - transform.position.x);
        float targetVelX = targetRb != null ? targetRb.linearVelocity.x : 0f;

        float predictionTime = Mathf.Min(distance / maxSpeed, maxPredictionTime);
        float futureX = target.position.x + targetVelX * predictionTime;

        float dir = Mathf.Sign(futureX - transform.position.x);
        float desiredVelX = dir * maxSpeed;
        float steer = desiredVelX - rb.linearVelocity.x;
        return Mathf.Clamp(steer, -maxForce, maxForce);
    }

    float Brake()
    {
        return Mathf.Clamp(-rb.linearVelocity.x, -maxForce, maxForce);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
