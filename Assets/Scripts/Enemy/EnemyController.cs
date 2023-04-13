using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float lookRadius = 10f;
    private NavMeshAgent agent;
    private Transform target;
    private CharacterCombat combat;
    public GameObject bear;
    private Animator bearAnimator;
    public float lastKnownLocationDuration = 5f; // how long to remember the player's last known location
    private Vector3 lastKnownLocation;
    private float lastKnownLocationTime;
    public EnemyStats enemyStats;

    private bool isAttacking;

    //Target stats here, to be used in a invoked method
    private readonly List<CharacterStats> targetStats = new();


    // Start is called before the first frame update
    private void Start()
    {
        enemyStats.destroyEnemyTime = 3f;
        bearAnimator = bear.GetComponent<Animator>();
        target = PlayerManager.instance.player.transform;
        agent = GetComponent<NavMeshAgent>();
        combat = GetComponent<CharacterCombat>();
    }

    // Update is called once per frame
    private void Update()
    {
        //Play death animation if dead
        if (enemyStats != null && enemyStats.isDead)
        {
            GetComponent<Collider>().enabled = false;
            agent.enabled = false;
            bearAnimator.Play("Death");
            return;
        }

        //If there's a player, the enemy is not dead and not attacking get the distance from enemy to player
        if (target != null && !enemyStats.isDead && !isAttacking)
        {
            var distance = Vector3.Distance(target.position, transform.position);
            if (distance <= lookRadius)
            {
                lastKnownLocation = target.position;
                lastKnownLocationTime = Time.time;
                if (agent != null)
                {
                    agent.SetDestination(lastKnownLocation);
                    agent.stoppingDistance = 1f;

                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        bearAnimator.Play("Attack1");
                        isAttacking = true;
                        Invoke(nameof(FinishAttack), 1f);
                        FaceTarget();
                    }
                    else
                    {
                        bearAnimator.Play("RunForward");
                        FaceTarget();
                    }
                }
            }
            else if (Time.time - lastKnownLocationTime <= lastKnownLocationDuration)
            {
                agent.SetDestination(lastKnownLocation);
                bearAnimator.Play("RunForward");
                FaceTarget();
            }
            else
            {
                bearAnimator.Play("Idle");
            }
        }
    }

    private void FinishAttack()
    {
        var enemyAttackColliderTransform = transform.Find("AttackCollider");
        var enemyAttackCollider = enemyAttackColliderTransform.GetComponent<EnemyAttackCollider>();
        if (enemyAttackCollider.playersToAttack.Count > 0 && 
            enemyAttackCollider.playersToAttack[0] != null)
        {
            var playerToAttack = enemyAttackCollider.playersToAttack[0];
            Debug.Log(playerToAttack.tag);
            targetStats.Add(playerToAttack.GetComponent<CharacterStats>());
            if (targetStats.Count > 0 && targetStats != null)
            {
                combat.Attack(targetStats);
            }
        }
        isAttacking = false;
    }

    private void FaceTarget()
    {
        var direction = (lastKnownLocation - transform.position).normalized;
        var lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookRadius);
    }
}