using UnityEngine;
using UnityEngine.InputSystem;

public class RangedAttack : MonoBehaviour
{
    public Transform player;    //player location
    public Transform muzzle;    //muzzle location
    public float muzzleOffset = 0.492f; //offset value to put the muzzle at the tip of the object
    public float time;  //number to keep the object active for a while      || time and countdownReady are non important for this script yet
    public bool countdownReady; //boolean to help with the time logic
    public float normal;    //force a normalize value for the trasform
    public float yOffset = 0.282f;  //offset of the ranged weapon to be at the 'head' of the player
    public Vector2 mousePos, characterPos, pointDirection;  //Vector 2 for mouse, player and direction location

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
            time += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shoot();
            countdownReady = true;
        }
        else if (time >= 1)
        {
            time = 0;
            countdownReady = false;
        }
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
}
