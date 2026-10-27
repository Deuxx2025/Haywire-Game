using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MeleeWeapon, RangedWeapon; //Get the weapons GameObjects
    public List<GameObject> EnemyPool;
    public bool swapWeapon;
    public Vector2 screenSize;
    void Start()
    {
        screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
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
                EnemyPool[i].SetActive(true);
            }
        }
    }
}
