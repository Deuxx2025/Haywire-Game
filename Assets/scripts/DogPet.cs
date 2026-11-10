using UnityEngine;

public class DogPet : Pet
{
    [SerializeField] private Animator animator;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float arriveDistance = 1.5f;

    [Header("Bite")]
    public float biteRange = 0.8f;      // how close before it bites
    public float biteRadius = 0.5f;     // small, so stacked enemies all get hit
    public float biteCooldown = 0.6f;
    public int biteDamage = 1;
    public LayerMask enemyLayer;

    private Rigidbody2D rb;
    private bool reachedPlayer;
    private float biteTimer;
    private float retargetTimer;
    private Transform target;
    private float facing = 1f;

    [Header("Spawn")]
    public float spawnMargin = 2.1f;   // how far past the screen edge it appears

    public override void Init(Transform playerTransform)
    {
        base.Init(playerTransform);

        Camera cam = Camera.main;
        float halfWidth = cam.orthographicSize * cam.aspect;
        float side = Random.value < 0.5f ? -1f : 1f;
        float x = cam.transform.position.x + side * (halfWidth + spawnMargin);

        transform.position = new Vector3(x, player.position.y, 0f);
        rb.linearVelocity = Vector2.zero;
    }

    void Awake() => rb = GetComponent<Rigidbody2D>();

    void FixedUpdate()
    {
        if (player == null) return;

        biteTimer -= Time.fixedDeltaTime;
        retargetTimer -= Time.fixedDeltaTime;

        // Go to the player, then hunt enemies.
        if (!reachedPlayer)
        {
            target = player;
            if (Mathf.Abs(player.position.x - transform.position.x) < arriveDistance)
                reachedPlayer = true;
        }
        else if (retargetTimer <= 0f)
        {
            target = FindNearestEnemy();
            retargetTimer = 0.25f;
        }

        // If there's no enemies left, wait near the player.
        Transform goal = (target != null && target.gameObject.activeInHierarchy) ? target : player;
        bool huntingEnemy = goal != player;

        float dx = goal.position.x - transform.position.x;
        float stopDistance = huntingEnemy ? biteRange * 0.5f : arriveDistance;

        if (Mathf.Abs(dx) > stopDistance)
        {
            facing = Mathf.Sign(dx);
            rb.linearVelocity = new Vector2(facing * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
        }

        if (huntingEnemy && biteTimer <= 0f && Mathf.Abs(dx) <= biteRange)
            Bite();
    }

    void Bite()
    {
        animator.SetTrigger("Attack");

        Vector2 bitePos = (Vector2)transform.position + new Vector2(facing * 0.5f, 0f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(bitePos, biteRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Enemy enemy))
            {
                enemy.TakeDamage(biteDamage);
            }
        }
        biteTimer = biteCooldown;
    }

    Transform FindNearestEnemy()
    {
        Transform nearest = null;
        float best = float.MaxValue;

        foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
        {
            float d = Mathf.Abs(e.transform.position.x - transform.position.x);
            if (d < best) { best = d; nearest = e.transform; }
        }
        return nearest;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position + new Vector3(facing * 0.5f, 0f, 0f), biteRadius);
    }
}