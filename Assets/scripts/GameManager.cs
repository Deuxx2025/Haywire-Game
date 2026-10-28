using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MeleeWeapon, RangedWeapon; //Get the weapons GameObjects
    public List<GameObject> EnemyPool;
    public bool swapWeapon;
    public int random;
    // public Vector3 v3Pos;
    public Vector3 screenPos;
    void Start()
    {
        
    }

    void Update()
    {
        
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
            if (EnemyPool[i].activeSelf == false)
            {
                GetRandomPosition();
                EnemyPool[i].transform.position = screenPos;
                EnemyPool[i].SetActive(true);
            }
        }
    }

    public void GetRandomPosition()
    {
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            screenPos = Camera.main.ScreenToWorldPoint(new Vector3(-100, Screen.height, 0));
            random = -1;
        }
        else
        {
            screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 100, Screen.height, 0));
            random = 1;
        }

        screenPos = new Vector3(screenPos.x, screenPos.y, 0);
    }
}
