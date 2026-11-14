
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Configuration")]
    public EnemyData data;

    [Header("References")]
    public Rigidbody2D _rb;
    public Collider2D enemy;
    public Collider2D wall;
    public Collider2D otherWall;
    public Bullet Bullet;
    public PlayerAttack MeleeAttack;
    public Companion Pet;

    [Header("Stats")]
    public int maxHp = 1;
    public int hp = 1;
    public int damage = 1;

    private bool isDead;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Collider2D>();
    }

    public void Initialize(EnemyData enemyData, Transform player)
    {
        data = enemyData;

        maxHp = data.maxHp;
        hp = maxHp;
        damage = data.damage;
        isDead = false;

        SteeringAgent steering = GetComponent<SteeringAgent>();

        if (steering != null)
        {
            steering.behavior = data.movementBehavior;
            steering.maxSpeed = data.speed;
            steering.maxForce = data.maxForce;
            steering.detectionRadius = data.detectionRadius;
            steering.canJump = data.canJump;
            steering.SetTarget(player);
        }

        gameObject.SetActive(true);
    }

    void Update()
    {
        if (hp <= 0 && !isDead)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        if (DataManager.Instance != null)
            DataManager.Instance.points++;

        gameObject.SetActive(false);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (Bullet != null)
                TakeDamage(Bullet.damage);
        }

        if (collision.gameObject.CompareTag("Companion"))
        {
            if (Pet != null)
                TakeDamage(Pet.damage);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Weapon"))
        {
            if (MeleeAttack != null)
                TakeDamage(MeleeAttack.damage);
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        hp -= amount;

        if (hp <= 0)
            Die();
    }
}
