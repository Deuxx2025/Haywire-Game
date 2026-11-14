
using UnityEngine;

[CreateAssetMenu(
    fileName = "NewEnemy",
    menuName = "Haywire/Enemy Data"
)]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;
    public GameObject prefab;

    [Header("Combat")]
    public int maxHp = 1;
    public int damage = 1;

    [Header("Movement")]
    public SteeringAgent.Behavior movementBehavior =
        SteeringAgent.Behavior.Seek;

    public float speed = 10f;
    public float maxForce = 20f;
    public float detectionRadius = 10f;
    public bool canJump = true;

    [Header("Enemy Properties")]
    public bool canBeTrampled;
    public bool explodes;
    public bool shoots;
    public bool flies;

    [Header("Spawning")]
    public int firstRound = 1;
}
