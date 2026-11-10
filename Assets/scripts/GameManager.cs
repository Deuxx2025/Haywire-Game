using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public GameObject MeleeWeapon, RangedWeapon, Companion, Player; //Get the GameObjects
    public List<GameObject> EnemyPool; 
    public int enemiesCalled;
    public float maxCoolDown;
    public float coolDown;
    public float yOffsetEnemySpawn;
    public bool swapWeapon, stopEnemies, cooldownReady;
    public Vector3 screenPos;

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


        if (stopEnemies)
        {
            StopCoroutine("InstantiateEnemies");
            RoundChange();
        }
        else
        {
            StartCoroutine(InstantiateEnemies(3f));
            coolDown = maxCoolDown;
        }

        if (Input.GetKey(KeyCode.Alpha3) && Companion.activeSelf == false)
        {
            Companion.SetActive(true);
            Companion.transform.position = new Vector3(Player.transform.position.x + 3, Player.transform.position.y, 0);
            StartCoroutine(CompanionAction(15));
        }
    }

    public IEnumerator InstantiateEnemies(float seconds)
    {
        for (int i = 0; i < EnemyPool.Count; i++)
        {
            yield return new WaitForSeconds(seconds);
            if (EnemyPool[i].activeSelf == false)
            {
                if (stopEnemies) break;
                GetRandomPosition();
                EnemyPool[i].transform.position = screenPos;
                EnemyPool[i].SetActive(true);
                enemiesCalled++;
            }

            if (enemiesCalled >= EnemyPool.Count)
            {
                stopEnemies = true;
            }
        }
    }

    public void GetRandomPosition()
    {
        int randomIndex = Random.Range(0, 2);

        if (randomIndex == 0)
        {
            screenPos = Camera.main.ScreenToWorldPoint(new Vector3(-100, Player.transform.position.y + yOffsetEnemySpawn, 0));   
        }
        else
        {
            screenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + 100, Player.transform.position.y + yOffsetEnemySpawn, 0));
        }

        screenPos = new Vector3(screenPos.x, screenPos.y, 0);
    }

    public void RoundChange()
    {

        if (cooldownReady)
        {
            coolDown -= Time.deltaTime;
        }

        if (AreAllEnemiesInactive())
        {
            cooldownReady = true;
        }

        if (coolDown <= 0)
        {
            cooldownReady = false;
            enemiesCalled = 0;
            stopEnemies = false;
        }
    }

    bool AreAllEnemiesInactive(){
	    foreach(GameObject enemy in EnemyPool) {
		    if(enemy.activeSelf) {
			    return false;
		    }
	    }
	return true;
}

    public IEnumerator CompanionAction(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Companion.SetActive(false);
    }
}
