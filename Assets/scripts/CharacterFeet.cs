using UnityEngine;

public class CharacterFeet : MonoBehaviour
{
    public MeowlMovement meowlMoveScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            meowlMoveScript.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            meowlMoveScript.isGrounded = false;
        }
    }
}
