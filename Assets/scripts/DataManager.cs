using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public int points; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
