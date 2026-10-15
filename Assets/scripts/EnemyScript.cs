using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Deal Damage");
            gameObject.transform.localScale = new Vector3(
                transform.localScale.x,
                transform.localScale.y - 0.1f,
                transform.localScale.z
            );
            collision.gameObject.GetComponent<Rigidbody2D>().linearVelocity = (collision.transform.position - transform.position) * 5;
            collision.gameObject.GetComponent<MeowlMovement>().damageCD = 0.1f;
        }
        else
        {
            gameObject.transform.localScale = new Vector3(
                transform.localScale.x,
                0.5f,
                transform.localScale.z
            );
        }
    }
}
