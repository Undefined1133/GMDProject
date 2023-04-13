using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed of the mob's movement
    public float movePeriod = 5f; // Time between movements
    public float moveRange = 10f; // Maximum distance the mob can move from its starting position

    private Vector3 startPosition; // Starting position of the mob
    private Vector3 targetPosition; // Target position the mob is moving towards
    private bool isMoving = false; // Whether the mob is currently moving

    public GameObject animal;
    private Animator animalAnimator;

    void Start()
    {
        animalAnimator = animal.GetComponent<Animator>();
        startPosition = transform.position;
        targetPosition = GetRandomTargetPosition();
        isMoving = true;
    }

    void Update()
    {
        if (isMoving)
        {
            PlayAnimation("Run");
            // Move towards the target position
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            FaceTarget();
            // Check if we've reached the target position
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                isMoving = false;
                StartCoroutine(MoveAgainAfterDelay());
                PlayAnimation("Idle");
            }
        }
    }
    void FaceTarget()
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
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

    IEnumerator MoveAgainAfterDelay()
    {
        // Wait for the specified period of time
        yield return new WaitForSeconds(movePeriod);

        // Get a new random target position and start moving towards it
        targetPosition = GetRandomTargetPosition();
        isMoving = true;
    }

    Vector3 GetRandomTargetPosition()
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