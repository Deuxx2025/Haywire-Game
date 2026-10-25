using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 5f; //speed of the bullet
    public Rigidbody2D rb; //rigidBody of the bullet

    //function that is used to set the direction upon runtime
    public void Fire(Vector2 direction)
    {
        rb.linearVelocity = direction * speed;
    }

    //making the bullet disappear upon contact
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            gameObject.SetActive(false);
        }

        if (collision.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
        }
    }

}
