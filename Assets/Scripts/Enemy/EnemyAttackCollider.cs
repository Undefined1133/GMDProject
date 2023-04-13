using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackCollider : MonoBehaviour
{
    public List<GameObject> playersToAttack = new();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Triggered by " + other.tag);
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("I SEE REEEEEEEEEED!");
            var player = other.gameObject;
            if (player != null) playersToAttack.Add(player);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player ran away :3");
            var player = other.gameObject;
            if (player != null) playersToAttack.Remove(player);
        }
    }
}
