using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class DragonMovement : MonoBehaviour
{
  public float moveSpeed = 3f; // Speed of the mob's movement
    public float movePeriod = 5f; // Time between movements
    public float moveRange = 10f; // Maximum distance the mob can move from its starting position

    private Vector3 startPosition; // Starting position of the mob
    private Vector3 targetPosition; // Target position the mob is moving towards
    private bool isMoving = false; // Whether the mob is currently moving
    private bool isFlying = false; // Whether the mob is currently moving
    private bool isAttacking = false;
    CharacterCombat combat;
    public GameObject animal;
    private Animator animalAnimator;
    private GameObject target;
    public EnemyStats dragonStats;
    NavMeshAgent agent;


    void Start()
    {
        dragonStats.destroyEnemyTime = 2f;
        combat = GetComponent<CharacterCombat>();
        agent = GetComponent<NavMeshAgent>();
        animalAnimator = animal.GetComponent<Animator>();
        startPosition = transform.position;
        targetPosition = GetRandomTargetPosition();
        isMoving = true;
    }

    void Update()
    {
        if (dragonStats.isDead)
        {
            isMoving = false;
            animalAnimator.Play("Die");
        }

        if (isMoving && target != null)
        {
            if (agent != null)
            {
                agent.SetDestination(target.transform.position);
                agent.stoppingDistance = 5f;
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    List<CharacterStats> targetStats = new List<CharacterStats>();
                    targetStats.Add(target.GetComponent<CharacterStats>());

                    if (targetStats.Count > 0 && !isAttacking)
                    {
                        Attack(targetStats);
                    }
                }
                else
                {
                    animalAnimator.Play("Run");
                    FaceTarget();
                }
            }
        }
        else if (isMoving)
        {
            PlayAnimation("Run");
            transform.position =
                Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            FaceTarget();
            // Check if we've reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                StartCoroutine(MoveAgainAfterDelay());
                int randomAnimationNumber = Random.Range(1, 3);
                Debug.Log(randomAnimationNumber);
                PlayAnimation("Idle" + randomAnimationNumber);
            }
        } 
        
    }
    void FaceTarget()
    {
        var location = targetPosition;
        if (target != null)
        {
            location = target.transform.position;
        }
        Vector3 direction = (location - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void PlayAnimation(string animation)
    {
        if (animalAnimator != null)
        {
            animalAnimator.Play(animation);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            target = other.gameObject;
            Debug.Log("TOUCHING PLAYER :D");
        }
    }

    void Attack(List<CharacterStats> targetStats)
    {
        isAttacking = true;
        if (!targetStats[0].isDead)
        {
            combat.Attack(targetStats);
            Invoke(nameof(FinishAttack), 1f);
            animalAnimator.Play("Attack1");
            FaceTarget();
        }
    }

    private void FinishAttack()
    {
        isAttacking = false;
    }

    IEnumerator MoveAgainAfterDelay()
    {
        // Wait for the specified period of time
        yield return new WaitForSeconds(movePeriod);

        // Get a new random target position and start moving towards it
        targetPosition = GetRandomTargetPosition();
        isMoving = true;
    }

    private Vector3 GetRandomTargetPosition()
    {
        // Calculate a random point within the move range from the starting position
        Vector3 randomDirection = Random.insideUnitSphere * moveRange;
        randomDirection += startPosition;
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(randomDirection, out navHit, moveRange, 1))
        {
            return navHit.position;
        }
        else
        {
            // If NavMesh.SamplePosition failed, return the starting position
            return startPosition;
        }
    }
}
