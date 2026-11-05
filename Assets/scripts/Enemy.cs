using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp = 1;
    public int hp = 1;
    public int damage = 1;
    public Bullet Bullet;
    public PlayerAttack MeleeAttack;
    public Companion Pet;

    void Start()
    {
        hp = maxHp;
    }
    
    void Update()
    {
        MeleeAttack.GetComponent<PlayerAttack>();
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }

        if (gameObject.activeSelf == false)
        {
            hp = maxHp;
        }
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
}
