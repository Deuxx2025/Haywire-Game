using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    //public MeowlMovement playerMovement;
    private SpriteRenderer weaponRender;
    private Collider2D weaponCollider;
    public Transform player;
    public float time; 
    //public float currentDirection;
    //public float lastDirection;
    public bool countdownReady;
    public float normal;
    public Vector2 mousePos, characterPos, pointDirection;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        weaponRender = GetComponent<SpriteRenderer>();
        weaponCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        characterPos = Camera.main.WorldToScreenPoint(player.transform.position);

        pointDirection = mousePos - characterPos;

        if (pointDirection.x < 0)
        {
            normal = -1;
        }
        else if (pointDirection.x > 0)
        {
            normal = 1;
        }

        // lastDirection = currentDirection;
        // currentDirection = playerMovement.xInput;
        gameObject.transform.position = new Vector3 (player.position.x + normal, player.position.y, 0);

        // if(lastDirection == 0)
        // {
        //     lastDirection = 1;
        // }

        // if (currentDirection == 0)
        // {
        //     currentDirection = lastDirection;
        // }

        if (countdownReady)
        {
            time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponRender.enabled = true;
            weaponCollider.enabled = true;
            countdownReady = true;
        }
        else if (time >= 1)
        {
            weaponRender.enabled = false;
            weaponCollider.enabled = false;
            time = 0;
            countdownReady = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        print("hi");
    }
}
