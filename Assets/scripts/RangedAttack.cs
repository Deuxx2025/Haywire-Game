using UnityEngine;
using UnityEngine.InputSystem;

public class RangedAttack : MonoBehaviour
{
    public Transform player;    //player location
    public Transform muzzle;    //muzzle location
    public float muzzleOffset = 0.492f; //offset value to put the muzzle at the tip of the object
    public float cooldown = 2;  //cooldown for the next shot      
    public bool countdownReady; //boolean to help with the time logic
    public float normal;    //force a normalize value for the trasform
    public float yOffset = 0.282f;  //offset of the ranged weapon to be at the 'head' of the player
    public int bulletsLeft;
    public int maxBullets = 2;
    public Vector2 mousePos, characterPos, pointDirection;  //Vector 2 for mouse, player and direction location

    void Awake()
    {
        bulletsLeft = maxBullets;
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        characterPos = Camera.main.WorldToScreenPoint(player.transform.position);   //reading the value of the position 
        pointDirection = Vector2.Normalize(mousePos - characterPos);                //to the player relative to the camera

        if (pointDirection.x < 0)
        {
            normal = -1;
           
        }
        else if (pointDirection.x > 0)
        {
            normal = 1;
        }

        //drawing the position of the game object dynamically
        gameObject.transform.position = new Vector3 (player.position.x + normal, player.position.y + yOffset, 0);
        muzzle.transform.position = new Vector3 (player.transform.position.x + muzzleOffset + normal * 2, player.position.y + yOffset, 0);

        if (countdownReady)
        {
            cooldown -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && cooldown == 2 && bulletsLeft > 0)
        {
            Shoot();
            bulletsLeft--;
            countdownReady = true;
        }
        else if (cooldown <= 0)
        {
            cooldown = 2;
            countdownReady = false;
        }

        ReloadGun();
    }

    //method to place the bullets on the muzzle
    public void Shoot()
    {
        GameObject bullet = BulletPool.instance.GetPooledBullets(); //using the singleton to read how many objects there are

        if (bullet != null)
        {
            bullet.transform.position = muzzle.transform.position;  //placing the bullet at the muzzle
            bullet.SetActive(true); //activating the object for the Bullet script to run
            bullet.GetComponent<Bullet>().Fire(new Vector2(normal, 0)); //giving the direction to the Bullet script
        }
    }

    public void ReloadGun()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < maxBullets)
        {
            bulletsLeft = maxBullets;
        }
    }
}
