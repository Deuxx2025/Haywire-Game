using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private SpriteRenderer weaponRender;    //getting the visual of the game object
    private Collider2D weaponCollider;  //getting the collider of the game object
    public Transform player;    //player location
    public float time;  //number to keep the object active for a while
    public bool countdownReady; //boolean to help with the time logic
    public float normal;    //force a normalize value for the trasform
    public Vector2 mousePos, characterPos, pointDirection;  //Vector 2 for mouse, player and direction location
    
    void Start()
    {
        weaponRender = GetComponent<SpriteRenderer>();  //getting the components for the visual and the collider
        weaponCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        mousePos = Input.mousePosition;
        characterPos = Camera.main.WorldToScreenPoint(player.transform.position);

        if (pointDirection.x < 0)
        {
            normal = -1;
        }
        else if (pointDirection.x > 0)
        {
            normal = 1;
        }

        gameObject.transform.position = new Vector3 (player.position.x + normal, player.position.y, 0);

        if (countdownReady)
        {
            time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            weaponRender.enabled = true;
            weaponCollider.enabled = true;              //when the player presses the button the object appears
            countdownReady = true;
            pointDirection = mousePos - characterPos;
        }
        else if (time >= 1)
        {
            weaponRender.enabled = false;
            weaponCollider.enabled = false;             //when the time passes the object disappears
            time = 0;
            countdownReady = false;
        }
    }

    //collider check
    public void OnTriggerEnter2D(Collider2D collider)
    {
        print("the player has attacked something");
    }
}
