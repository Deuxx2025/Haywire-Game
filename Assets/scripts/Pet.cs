using UnityEngine;

public abstract class Pet : MonoBehaviour
{
    public float duration = 15f;
    protected Transform player;

    public virtual void Init(Transform playerTransform)
    {
        player = playerTransform;
        if (duration > 0) Destroy(gameObject, duration);
    }
}