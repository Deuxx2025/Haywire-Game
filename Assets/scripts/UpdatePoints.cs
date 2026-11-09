using UnityEngine;
using TMPro;

public class UpdatePoints : MonoBehaviour
{
    public TMP_Text pointsDisplay;

    void Update()
    {
        pointsDisplay.text = DataManager.Instance.points.ToString();
    }
}
