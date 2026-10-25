using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MeleeWeapon, RangedWeapon; //Get the weapons GameObjects
    public bool swapWeapon;
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
    }
}
