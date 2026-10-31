using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int maxHp = 1;
    public int hp = 1;
    public int damage = 1;
    public Bullet Bullet;

    void Start()
    {
        hp = maxHp;
    }
    
    void Update()
    {
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            hp -= Bullet.damage;
        }
    }
}
