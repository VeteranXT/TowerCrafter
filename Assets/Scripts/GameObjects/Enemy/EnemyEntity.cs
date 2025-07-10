using System.Xml.Serialization;
using UnityEngine;
using Pathfinding;
using UnityEditorInternal;
using System;
public class EnemyEntity : MonoBehaviour
{
    [Header("General Stats")]
    [SerializeField] private CharacterStat armor = new CharacterStat();
    [SerializeField] private HealthSystem health = new HealthSystem(100f);
    [SerializeField] private CharacterStat moveSpeed = new CharacterStat();
    [SerializeField] private CharacterStat attackSpeed = new CharacterStat();
    [SerializeField] private CharacterStat minDamage = new CharacterStat();
    [SerializeField] private CharacterStat maxDamage = new CharacterStat();
    [SerializeField] private CharacterStat attackRange = new CharacterStat();
    [SerializeField] private bool isAlive = true;
    [SerializeField] private bool isPurged = false;
    [Space]
    [Header("Pathfinding")]
    [SerializeField] private Transform orginalTarget;
    [Space]
    [Header("Refrences")]
    [SerializeField] private AIPath path;
    [SerializeField] private AIDestinationSetter destination;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    public float GetMoveSpeed { get { return moveSpeed.Value; } }
   


    private void Update()
    {
        path.canMove = isAlive;
        //Do not loop animation if dead
        if (isAlive)
            animator.speed = 1;
        else
            animator.speed = 0;

    }



    public static EnemyEntity CreateInstance(EntityData prefab, Transform spawnLocation, Transform target)
    {
        EnemyEntity enemy = Instantiate(prefab.GetPrefab.GetComponent<EnemyEntity>(), spawnLocation, false);
        enemy.FetchRefrences(target,enemy);
        return enemy;
    }

    private void FetchRefrences(Transform target, EnemyEntity entity)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.interpolation = RigidbodyInterpolation2D.Extrapolate;
        rb.sleepMode = RigidbodySleepMode2D.NeverSleep;
        rb.gravityScale = 0f;


        animator = GetComponent<Animator>();

        path = GetComponent<AIPath>();
        path.maxSpeed = GetMoveSpeed;
        destination = GetComponent<AIDestinationSetter>();
        destination.target = target;

        health = entity.health;
        maxDamage = entity.maxDamage;
        minDamage = entity.minDamage;
        moveSpeed = entity.moveSpeed;
        armor = entity.armor;
        attackRange = entity.attackRange;
        attackSpeed = entity.attackSpeed;
        isPurged = entity.isPurged;
    }
}