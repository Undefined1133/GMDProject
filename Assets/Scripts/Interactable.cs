using UnityEngine;

public class Interactable : MonoBehaviour
{
    public float radius = 3f;
    public Transform interactionTransform;
    private bool inInteractZone;
    private Transform player;

    public void OnEnterInteractZone(Transform playerTransform)
    {
        inInteractZone = true;
        player = playerTransform;
    }

    private void OnDrawGizmosSelected()
    {
        // if(interactionTransform ==null)
        // {
        // 	interactionTransform = transform;
        // }
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(interactionTransform.position, radius);
    }

    private void Update()
    {
        if (inInteractZone)
        {
            var distance = Vector3.Distance(player.position, interactionTransform.position);
            if (distance <= radius)
            {
                Interact();
                inInteractZone = false;
            }
        }
    }

    public virtual void Interact()
    {
        Debug.Log("Interacted with " + transform);
    }
}