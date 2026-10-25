using UnityEngine;
using UnityEngine.InputSystem;

public class RangedAttack : MonoBehaviour
{
    //public MeowlMovement playerMovement;
    private SpriteRenderer weaponRender;
    public Transform player;
    public Transform muzzle;
    public float muzzleOffset = 0.492f;
    public float time; 
    public bool countdownReady;
    public float normal;
    public float yOffset = 0.282f;
    public Vector2 mousePos, characterPos, pointDirection;
    void Start()
    {
        weaponRender = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mousePos = Input.mousePosition;
        characterPos = Camera.main.WorldToScreenPoint(player.transform.position);
        pointDirection = Vector2.Normalize(mousePos - characterPos);

        if (pointDirection.x < 0)
        {
            normal = -1;
           
        }
        else if (pointDirection.x > 0)
        {
            normal = 1;
        }

        gameObject.transform.position = new Vector3 (player.position.x + normal, player.position.y + yOffset, 0);
        muzzle.transform.position = new Vector3 (muzzleOffset + normal * 2, player.position.y + yOffset, 0);

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
            //weaponRender.enabled = false;
            //weaponCollider.enabled = false;
            time = 0;
            countdownReady = false;
        }
    }

    public void Shoot()
    {
        GameObject bullet = BulletPool.instance.GetPooledBullets();

        if (bullet != null)
        {
            bullet.transform.position = muzzle.transform.position;
            bullet.SetActive(true);
            bullet.GetComponent<Bullet>().Fire(new Vector2(normal, 0));
        }
    }
}
