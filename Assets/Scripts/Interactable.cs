using UnityEngine;

public class Interactable : MonoBehaviour
{
public float radius = 3f;
public Transform interactionTransform;
bool inInteractZone = false;
Transform player;

public void OnEnterInteractZone(Transform playerTransform)
{
	inInteractZone = true;
	player = playerTransform;
}

// public void OnExitInteractZone()
// {
// 	inInteractZone = false;
// 	player = null;

// }
void OnDrawGizmosSelected()
{
	// if(interactionTransform ==null)
	// {
	// 	interactionTransform = transform;
	// }
	Gizmos.color = Color.yellow;
	Gizmos.DrawWireSphere(interactionTransform.position, radius);
}
void Update()
{
	if(inInteractZone){
		float distance = Vector3.Distance(player.position, interactionTransform.position);
		if(distance <= radius)
		{
			Interact();
			inInteractZone = false;
		}
	}
}
//Virtual because we want to call this method in other classes, however we want to be able to overwrite it's behaviour
public virtual void Interact ()
{
	Debug.Log("Interacted with " + transform);
	// THIS METHOD IS MEANT TO BE OVERWRITTEN
}


}
