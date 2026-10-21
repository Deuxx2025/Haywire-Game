using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    public MeowlMovement playerMovement;
    private SpriteRenderer weaponRender;
    private Collider2D weaponCollider;
    public Transform player;
    public float currentDirection;
    public float lastDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponRender = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        lastDirection = currentDirection;
        currentDirection = playerMovement.xInput;
        gameObject.transform.position = new Vector3 (player.position.x + lastDirection, player.position.y, 0);

        if(lastDirection == 0)
        {
            lastDirection = 1;
        }

        if (currentDirection == 0)
        {
            currentDirection = lastDirection;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponRender.enabled = true;
            weaponCollider.enabled = true;
        }
        else
        {
            weaponRender.enabled = false;
            weaponCollider.enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        print("hi");
    }
}
