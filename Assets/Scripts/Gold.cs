using UnityEngine;

public class Gold : MonoBehaviour
{
	public float amount = 0f;
	PlayerManager playerManager;
	
	void Start()
	{
		playerManager = PlayerManager.instance;
	}
	
	public void SetAmount(float min, float max)
	{
	amount = Random.Range(min, max);
	}
	
		public void PickUp()
	{
		// Reduce the amount of gold and destroy the game object
		playerManager.AddGold(amount);
		Destroy(gameObject);
	}
}
