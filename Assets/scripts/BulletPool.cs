using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    public static BulletPool instance;  //singleton to access this script accross all the scenes
    private List<GameObject> pooledBullets = new List<GameObject>();    //list of type GameObject for the bullets
    private int amountToPool = 10;  //amount of maximum capacity of bullets

    [SerializeField] private GameObject bulletPrefab;   //bullet prefab GameObject

    void Awake()
    {
        if (instance == null)
        {                               //if there is no instance but this script exist then make this script an instance
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(bulletPrefab); //for the amount of bullets instantiate them
            obj.SetActive(false);                       //de activate them
            pooledBullets.Add(obj);                     //and add them to the list
        }
    }

    //method that returns a GameObject
    public GameObject GetPooledBullets()
    {
        for (int i = 0; i < pooledBullets.Count; i++)
        {
            if (!pooledBullets[i].activeInHierarchy)    //if there is an object not active in the hierarchy we return that object
            {
                return pooledBullets[i];
            }
        }

        return null;    //if all the bullets are active and we want to shoot more we return a null
    }
}
