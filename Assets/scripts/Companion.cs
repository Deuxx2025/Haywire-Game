using UnityEngine;

public class Companion : MonoBehaviour
{
    public Rigidbody2D _rb;
    public Transform playerPosition;
    public GameManager enemyList;
    public float playerDetection = 3f;
    public float enemyDetection = 10f;
    public float speed = 4f;
    public int damage = 2;
    public bool onPursue;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        for (int i = 0; i < enemyList.EnemyPool.Count; i++)
        {
            Vector2 enemyDirection = (Vector2)enemyList.EnemyPool[i].transform.position - _rb.position;

            float enemyDistance = enemyDirection.magnitude;

            if (enemyList.EnemyPool[i].activeSelf == true && enemyDistance < enemyDetection)
            {
                onPursue = true;
                _rb.linearVelocity = enemyDirection.normalized * speed;
                print("on pursue");
            }
        }
    }

    public void PlayerFollow()
    {

        if (onPursue) return;

        Vector2 playerDirection = (Vector2)playerPosition.position - _rb.position;

        float playerDistance = playerDirection.magnitude;

        if (playerDistance > playerDetection)
        {
            _rb.linearVelocity = playerDirection.normalized * speed;
            print("following player");
        }
    } 
}
