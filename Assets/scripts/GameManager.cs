using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MeleeWeapon, RangedWeapon; //Get the weapons GameObjects
    public List<GameObject> EnemyPool;
    public bool swapWeapon;
    public int random;
    public Vector3 v3Pos;
    void Start()
    {
        
    }

    void Update()
    {

        v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, 0.5f, 10.0f));
        
        //toggle logic
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            swapWeapon = !swapWeapon; 
        } 
        
        if (swapWeapon)
        {
            RangedWeapon.SetActive(true);
            MeleeWeapon.SetActive(false);
        }
        else
        {
            RangedWeapon.SetActive(false);
            MeleeWeapon.SetActive(true);
        }

        StartCoroutine(InstantiateEnemies(3f));
    }

    public IEnumerator InstantiateEnemies(float seconds)
    {
        for (int i = 0; i < EnemyPool.Count; i++)
        {
            yield return new WaitForSeconds(seconds);
            GetRandomPosition();
            if (EnemyPool[i].activeSelf == false)
            {
                EnemyPool[i].transform.position = new Vector3(v3Pos.x * random, v3Pos.y, 0);
                EnemyPool[i].SetActive(true);
            }
        }
    }

    public void GetRandomPosition()
    {
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            random = -1;
        }
        else
        {
            random = 1;
        }
    }
}
