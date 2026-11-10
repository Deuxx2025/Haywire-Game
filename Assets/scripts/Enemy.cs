using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Rigidbody2D _rb;
    public int maxHp = 1;
    public int hp = 1;
    public int damage = 1;
    public float speed = 2f;
    public Collider2D enemy;
    public Collider2D wall;
    public Collider2D otherWall;
    public Bullet Bullet;
    public PlayerAttack MeleeAttack;
    public Companion Pet;

    void Start()
    {
        hp = maxHp;
        _rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Collider2D>();
    }
    
    void Update()
    {
        if (hp <= 0)
        {
            gameObject.SetActive(false);
            DataManager.Instance.points++;
        }

        if (gameObject.activeSelf == false)
        {
            hp = maxHp;
        }

        Physics2D.IgnoreCollision(enemy, wall, true);
        Physics2D.IgnoreCollision(enemy, otherWall, true);

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= Bullet.damage;
        } 

        if (collision.gameObject.CompareTag("Companion"))
        {
            hp -= Pet.damage;
        } 
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Weapon"))
        {
            hp -= MeleeAttack.damage;
        }
    }

    public void TakeDamage(int amount)
    {
        hp -= amount;
    }
}
