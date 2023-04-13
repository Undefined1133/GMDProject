using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    public List<GameObject> enemiesToAttack = new();

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Checking if layering is working wtf");
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject;
            if (enemy != null) enemiesToAttack.Add(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            var enemy = other.gameObject;
            if (enemy != null) enemiesToAttack.Remove(enemy);
        }
    }
}