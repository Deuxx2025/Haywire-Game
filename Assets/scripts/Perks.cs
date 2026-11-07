using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;

public class Perks : MonoBehaviour
{
    public int damageAugment = 3;
    public float healthAugment = 0.1f;
    public void Adrenaline(int currentDamage)
    {
        currentDamage += damageAugment;
    }

    public void Meditation(float currentHealth)
    {
        currentHealth += healthAugment;
    }
}
