using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
		public CharacterController controller;
		PlayerManager playerManager;
		public float speed = 5f;
		public float turnSmoothTime = 0.1f;
		float turnSmoothVelocity;
		bool isCursorOn;
		public Camera cam;
		public Interactable interactable;
		private BoxCollider itemCollider;
		private List<Interactable> interactablesToPickup = new List<Interactable>();
		private List<GameObject> goldToPickUp = new List<GameObject>();
		private CharacterCombat combat; 
		public float dashDistance = 100f;
		public float dashDuration = 0.5f;
		private bool isDashing = false;
		private float currentDashTime = 0f;
		private Vector3 dashStartPosition; 
		public GameObject playerCharacter;
		public GameObject slashAnimation;
		GameObject instantiatedAnimation;
		Animator characterAnimator;
		bool isAttacking;


	void Start()
	{
		isAttacking = false;
		playerManager = PlayerManager.instance;
		characterAnimator = playerCharacter.GetComponent<Animator>();
		itemCollider = gameObject.GetComponent<BoxCollider>();
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		isCursorOn = false;
		combat = GetComponent<CharacterCombat>();

	}
	// Update is called once per frame, after not enough testing figured that better to check for input inside update

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.LeftAlt))
		{  
			if(isCursorOn) 
			{ 
				Cursor.lockState = CursorLockMode.Locked; 
				Cursor.visible = false; 
				isCursorOn = false;
			}else
			{
				Cursor.lockState = CursorLockMode.None; 
				Cursor.visible = true; 
				isCursorOn = true; 
			} 
		} 
		if(Input.GetKeyDown(KeyCode.Mouse1) && !isDashing)
		{
			PlayerStats playerStats = playerManager.player.GetComponent<PlayerStats>();
			if(!(playerStats.currentMana.GetValue() < 5))
			{ 
				playerStats.currentMana.SetValue(playerStats.currentMana.GetValue() - 5); 
				// Start the dash
				isDashing = true; 
				currentDashTime = 0f;
				dashStartPosition = transform.position;
			}
			playerManager.manaBar.SetMana(playerStats.currentMana.GetValue());
		}
		//CAMERA + PLAYER MOVEMENT
		//-----------------------------------------------------------------------------------------
		float horizontalInput = Input.GetAxisRaw("Horizontal");
		float vertical = Input.GetAxisRaw("Vertical");
		Vector3 direction = new Vector3(horizontalInput, 0f, vertical).normalized;

		if(direction.magnitude >= 0.1f)
		{
			float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
			float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
			if(!isAttacking)
			{
				transform.rotation = Quaternion.Euler(0f, angle, 0f); 
			}
			Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
			Move(moveDirection);
		}else
		{
			if(!isAttacking)
			{
			   characterAnimator.Play("Idle");
			}
		}
		void Move(Vector3 moveDirection)
		{ 
			if(!isAttacking){ 
				if (isDashing){ 
					// Calculate the dash distance covered so far
					float dashCoveredDistance = Vector3.Distance(transform.position, dashStartPosition); 
					// Calculate the factor to multiply the movement by
					float dashMovementFactor = dashDistance / dashDuration;
					// If the dash distance is not covered yet, move the controller in the dash direction
					if (dashCoveredDistance < dashDistance) 
					{ 
						controller.Move(moveDirection.normalized * speed * dashMovementFactor  * Time.deltaTime); 
						currentDashTime += Time.deltaTime; 
					}else{ 
						// End the dash
						isDashing = false; 
						currentDashTime = 0f; 
					} 
					// If the dash duration is over, end the dash
					if (currentDashTime >= dashDuration){ 
						isDashing = false; 
						currentDashTime = 0f; 
					} 
				}else{ 
					// Move the controller in the regular direction
					characterAnimator.Play("Move"); 
					controller.Move(moveDirection.normalized * speed * Time.deltaTime + Physics.gravity * Time.deltaTime); 
				} 
			} 
		}
		//------------------------------------------------------------------------------------------
	}
	
	void CompleteAttack()
	{
		Debug.Log("Completing attack :D");
		if(instantiatedAnimation != null)
		{
			Destroy(instantiatedAnimation);
		}
		isAttacking = false;
	}
	
	void FixedUpdate()
	{
		// PICK UP ITEMS/GOLD
		//------------------------------------------------------------------------------------------
		if(Input.GetKey("z"))
		{
				if(interactablesToPickup.Count > 0)
				{
					Debug.Log(interactablesToPickup[0]);
					OnEnterInteractZone(interactablesToPickup[0]);
					interactablesToPickup.RemoveAt(0);
				}else if(goldToPickUp.Count > 0)
				{
					if(goldToPickUp[0].GetComponent<Gold>() != null)
					{
					 Gold pickingUp = goldToPickUp[0].GetComponent<Gold>();
					 pickingUp.PickUp();
					 goldToPickUp.RemoveAt(0);
					}
				}
		}
		//------------------------------------------------------------------------------------------
		if(Input.GetKey("f"))
		{
			if(!isAttacking)
			{
			isAttacking = true;
			characterAnimator.Play("Attack1");
			var playerTransform = transform;
			instantiatedAnimation = Instantiate(slashAnimation, playerTransform.position, playerTransform.rotation);
			Invoke(nameof(CompleteAttack), combat.attackDelay);
			Transform playerObj = transform.Find("PlayerObj");
			PlayerObject playerObjectScript = playerObj.GetComponent<PlayerObject>();
			List<GameObject> enemies = playerObjectScript.enemiesToAttack;
			List<CharacterStats> enemyStats = new List<CharacterStats>();
			foreach(GameObject enemy in enemies)
			{
				if(enemy !=null)
				{
				enemyStats.Add(enemy.GetComponent<CharacterStats>());
				}
			}
			if(enemyStats.Count != 0)
			{
				combat.Attack(enemyStats);
			}
			}
		}

		
		void OnEnterInteractZone(Interactable newInteractable) 
		{
			interactable = newInteractable;
			newInteractable.OnEnterInteractZone(transform); 
		} 
	}
	
	void OnTriggerEnter(Collider other)
	{
		Debug.Log("TRIGGERED COLLISION ");
		if(other.gameObject.CompareTag("Pickup"))
		{
			Interactable interactable = other.gameObject.GetComponent<Interactable>();
			if(interactable !=null)
				{
					interactablesToPickup.Add(interactable);
				}
		}else if(other.gameObject.CompareTag("Gold"))
		{
			Debug.Log(other.gameObject.tag);
			goldToPickUp.Add(other.gameObject);
		}
	}
	
	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.CompareTag("Pickup"))
		{
			Interactable interactable = other.gameObject.GetComponent<Interactable>();
			if(interactable !=null)
				{
					interactablesToPickup.Remove(interactable);
				}
		}else if(other.gameObject.CompareTag("Gold"))
		{
			goldToPickUp.Remove(other.gameObject);
		}
	}
	
	public void SetSpeed(float speedToAdd)
	{
		speed += speedToAdd;
	}
}
