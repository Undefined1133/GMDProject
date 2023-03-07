using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObject : MonoBehaviour
{
	public List<GameObject> enemiesToAttack = new List<GameObject>();
	void Start()
	{
		
	}

	void Update()
	{
		
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Player object detected collision :O ");
		if(other.gameObject.tag == "Enemy")
		{
			Debug.Log("ITS AN ENEMY!!!! :O ");
			GameObject enemy = other.gameObject;
			if(enemy != null)
				{
					enemiesToAttack.Add(enemy);
				}
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		Debug.Log("Player object detected someone running away -_- ");
		if(other.gameObject.tag == "Enemy")
		{
			GameObject enemy = other.gameObject;
			if(enemy !=null)
				{
					enemiesToAttack.Remove(enemy);
				}
		}
	}
}
