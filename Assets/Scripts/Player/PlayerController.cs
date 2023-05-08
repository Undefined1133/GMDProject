using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CharacterController controller;
    private PlayerManager playerManager;
    private GameAudioManager gameAudioManager;
    public float speed = 5f;
    public float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private bool isCursorOn;
    public Camera cam;
    public CinemachineFreeLook virtualCamera;
    private bool cameraEnabled = true;
    public Interactable interactable;
    private BoxCollider itemCollider;
    private readonly List<Interactable> interactablesToPickup = new();
    private readonly List<GameObject> goldToPickUp = new();
    private CharacterCombat combat;
    public float dashDistance = 100f;
    public float dashDuration = 0.5f;
    private bool isDashing;
    private float currentDashTime;
    private Vector3 dashStartPosition;
    public GameObject playerCharacter;
    public GameObject slashAnimation;
    public GameObject levelUpAnimation;
    private GameObject instantiatedLevelUpAnimation;
    public GameObject healingAnimation;
    public GameObject controlPanel;
    private GameObject instantiatedHealingAnimation;
    private GameObject instantiatedAnimation;
    public GameObject healingCircleAnimation;
    private GameObject instantiatedHealingCircleAnimation;
    private Animator characterAnimator;
    private bool isHealing;
    private bool isAttacking;
    private PlayerStats playerStats;
    private bool isTakingDamage;
    public float timeToHeal = 5f; // Set this to the time interval at which you want to trigger the heal method
    public Skill spin;
    public GameObject statUi;
    private bool isHealOnCooldown;
    private Coroutine healingCoroutine;
    private bool isInsideHealingCircle;
    private float lastSoundTime;
    private readonly float soundCooldown = 1.1f; // for example, half a second
    private bool isRunning;
    private readonly List<AudioClip> runningClips = new();

    public delegate void DealtDamageEventHandler();

    public event DealtDamageEventHandler dealtDamage;


    private void Start()
    {
        isAttacking = false;
        playerManager = PlayerManager.instance;
        gameAudioManager = GameAudioManager.instance;
        playerStats = playerManager.player.GetComponent<PlayerStats>();
        playerStats.takenDamage += OnTakenDamage;
        playerStats.onLevelUp += OnLevelUp;
        playerStats.onHeal += OnHeal;
        playerStats.died += OnDeath;
        characterAnimator = playerCharacter.GetComponent<Animator>();
        itemCollider = gameObject.GetComponent<BoxCollider>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isCursorOn = false;
        combat = GetComponent<CharacterCombat>();
        if (gameAudioManager.runningClip1 == null || gameAudioManager.runningClip2 == null ||
            gameAudioManager.runningClip3 == null) return;
        runningClips.Add(gameAudioManager.runningClip1);
        runningClips.Add(gameAudioManager.runningClip2);
        runningClips.Add(gameAudioManager.runningClip3);
    }
    // Update is called once per frame, after not enough testing figured that better to check for input inside update

    private void Update()
    {
        if (playerStats.isDead)
        {
            characterAnimator.Play("StayDeath");
        }
        if (isRunning && Time.time - lastSoundTime >= soundCooldown && runningClips is {Count: > 0})
        {
            var randomNumber = Random.Range(0, 3);

            gameAudioManager.runningSource.PlayOneShot(runningClips[randomNumber]);
            lastSoundTime = Time.time;
        }

        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            if (isCursorOn)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                isCursorOn = false;
                cameraEnabled = true;
                virtualCamera.ForceCameraPosition(cam.transform.position, Quaternion.identity);
                virtualCamera.enabled = cameraEnabled;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                cameraEnabled = false;
                virtualCamera.enabled = cameraEnabled;
                Cursor.visible = true;
                isCursorOn = true;
            }
        }
        if (Input.GetKeyDown(KeyCode.H)) controlPanel.SetActive(!controlPanel.activeSelf);

        if (Input.GetKeyDown(KeyCode.C)) statUi.SetActive(!statUi.activeSelf);

        if (Input.GetKeyDown(KeyCode.Mouse1) && !isDashing)
        {
            if (!(playerStats.currentMana.GetValue() < 5))
            {
                playerStats.currentMana.SetValue(playerStats.currentMana.GetValue() - 5);
                // Start the dash
                isDashing = true;
                currentDashTime = 0f;
                dashStartPosition = transform.position;
            }

            playerManager.manaBar.SetMana(playerStats.currentMana.GetValue());
            playerStats.OnManaUsed();
        }

        //CAMERA + PLAYER MOVEMENT
        //------------------------------------------------------------------------------------------
        var horizontalInput = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        var direction = new Vector3(horizontalInput, 0f, vertical).normalized;

        if (!isCursorOn && direction.magnitude >= 0.1f)
        {
            var targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.transform.eulerAngles.y;
            var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity,
                turnSmoothTime);
            if (!isAttacking) transform.rotation = Quaternion.Euler(0f, angle, 0f);
            var moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Move(moveDirection);
        }
        else
        {
            if (!isAttacking && !isTakingDamage && !playerStats.isDead)
            {
                gameAudioManager.runningSource.Stop();
                isRunning = false;
                characterAnimator.Play("Idle");
            }
        }

        void Move(Vector3 moveDirection)
        {
            if (!isAttacking && !playerStats.isDead) 
            {
                if (isDashing)
                {
                    // Calculate the dash distance covered so far
                    var dashCoveredDistance = Vector3.Distance(transform.position, dashStartPosition);
                    // Calculate the factor to multiply the movement by
                    var dashMovementFactor = dashDistance / dashDuration;
                    // If the dash distance is not covered yet, move the controller in the dash direction
                    if (dashCoveredDistance < dashDistance)
                    {
                        controller.Move(moveDirection.normalized * speed * dashMovementFactor * Time.deltaTime);
                        currentDashTime += Time.deltaTime;
                    }
                    else
                    {
                        // End the dash
                        isDashing = false;
                        currentDashTime = 0f;
                    }
                    // If the dash duration is over, end the dash
                    if (currentDashTime >= dashDuration)
                    {
                        isDashing = false;
                        currentDashTime = 0f;
                    }
                }
                else
                {
                    isRunning = true;
                    // Move the controller in the regular direction
                    characterAnimator.Play("Move");
                    controller.Move(
                        moveDirection.normalized * speed * Time.deltaTime + Physics.gravity * Time.deltaTime);
                }
            }
        }
    }
    //------------------------------------------------------------------------------------------


    private void CompleteAttack()
    {
        if (instantiatedAnimation != null) Destroy(instantiatedAnimation);
        Debug.Log("Complete attack triggered :PPPPPPPPPP setting is attacking to false");
        isAttacking = false;
    }

    private void OnDeath()
    {
        characterAnimator.Play("Death");
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
        // PICK UP ITEMS/GOLD
        //------------------------------------------------------------------------------------------
        if (Input.GetKey("z"))
        {
            if (interactablesToPickup.Count > 0)
            {
                Debug.Log(interactablesToPickup[0]);
                OnEnterInteractZone(interactablesToPickup[0]);
                interactablesToPickup.RemoveAt(0);
            }
            else if (goldToPickUp.Count > 0)
            {
                if (goldToPickUp[0].GetComponent<Gold>() != null)
                {
                    var pickingUp = goldToPickUp[0].GetComponent<Gold>();
                    pickingUp.PickUp();
                    goldToPickUp.RemoveAt(0);
                }
            }
        }
        //------------------------------------------------------------------------------------------


        // SIMPLE ATTACK
        //------------------------------------------------------------------------------------------
        if (Input.GetKey("f") && !isDashing)
            if (!isAttacking)
            {
                isAttacking = true;
                //Stopping running audio... looks like a bad implementation not sure
                gameAudioManager.runningSource.Stop();
                characterAnimator.Play("Attack1");
                var playerTransform = transform;
                if (slashAnimation != null)
                    instantiatedAnimation =
                        Instantiate(slashAnimation, playerTransform.position, playerTransform.rotation);
                Invoke(nameof(CompleteAttack), combat.attackDelay);
                var playerObj = transform.Find("PlayerObj");
                var playerObjectScript = playerObj.GetComponent<PlayerAttackCollider>();
                var enemies = playerObjectScript.enemiesToAttack;
                var enemyStats = new List<CharacterStats>();
                gameAudioManager.OnAttackPlay();
                foreach (var enemy in enemies)
                    if (enemy != null)
                        enemyStats.Add(enemy.GetComponent<CharacterStats>());
                if (enemyStats.Count != 0)
                {
                    combat.Attack(enemyStats);
                    dealtDamage?.Invoke();
                }
            }
        //------------------------------------------------------------------------------------------

        //SPIN ATTACK
        //------------------------------------------------------------------------------------------
        if (Input.GetKey("1"))
            if (!isAttacking)
            {
                isAttacking = true;
                characterAnimator.Play("SpinAttack");
                Invoke(nameof(CompleteAttack), 0.5f);
            }
        //------------------------------------------------------------------------------------------

        //HEALING CIRCLE
        //------------------------------------------------------------------------------------------
        if (Input.GetKey("2"))
            if (healingCircleAnimation != null && !isHealOnCooldown)
            {
                isHealOnCooldown = true;
                var playerTransformPosition = transform.position;
                var spawnLocation = new Vector3(playerTransformPosition.x, playerTransformPosition.y - 0.5f,
                    playerTransformPosition.z);
                instantiatedHealingCircleAnimation = Instantiate(healingCircleAnimation, spawnLocation,
                    transform.rotation);
                Invoke(nameof(DestroyHealingCircleAnimation), 4f);
            }
        //------------------------------------------------------------------------------------------

        void OnEnterInteractZone(Interactable newInteractable)
        {
            interactable = newInteractable;
            newInteractable.OnEnterInteractZone(transform);
        }
    }

    private void DestroyHealingCircleAnimation()
    {
        isHealOnCooldown = false;
        if (instantiatedHealingCircleAnimation != null) Destroy(instantiatedHealingCircleAnimation);
    }

    private void OnTakenDamage()
    {
        if (!isTakingDamage)
        {
            isTakingDamage = true;
            characterAnimator.Play("ReceiveDamage");
            Invoke(nameof(CompleteReceiveDamage), 0.5f);
        }
    }

    private void CompleteReceiveDamage()
    {
        isTakingDamage = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);
        if (other.gameObject.CompareTag("Pickup"))
        {
            var interactable = other.gameObject.GetComponent<Interactable>();
            if (interactable != null) interactablesToPickup.Add(interactable);
        }
        else if (other.gameObject.CompareTag("Gold"))
        {
            goldToPickUp.Add(other.gameObject);
        }
        else if (other.gameObject.CompareTag("HealCircle") && !isHealing && healingCoroutine == null)
        {
            isInsideHealingCircle = true;
            StartHealOverTime(10, 4);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Pickup"))
        {
            var interactable = other.gameObject.GetComponent<Interactable>();
            if (interactable != null) interactablesToPickup.Remove(interactable);
        }
        else if (other.gameObject.CompareTag("Gold"))
        {
            goldToPickUp.Remove(other.gameObject);
        }
        else if (other.gameObject.CompareTag("HealCircle") && isHealing)
        {
            isHealing = false;
            isInsideHealingCircle = false;
            //And turn off healing animation :D
        }
    }

    private IEnumerator HealOverTime(float healAmount, float duration)
    {
        var elapsedTime = 0f;
        isHealing = true;
        while (elapsedTime < duration && isInsideHealingCircle && instantiatedHealingCircleAnimation != null)
        {
            playerStats.Heal(Mathf.FloorToInt(healAmount));
            elapsedTime += 1f;
            yield return new WaitForSeconds(1f);
        }

        isHealing = false;
        healingCoroutine = null;
    }

    // Call this method to start the heal over time skill
    public void StartHealOverTime(float healAmount, float duration)
    {
        healingCoroutine = StartCoroutine(HealOverTime(healAmount, duration));
    }

    private void OnHeal()
    {
        var playerTransform = transform;
        var spawnPosition = new Vector3(playerTransform.position.x, playerTransform.position.y - 0.5f,
            playerTransform.position.z);
        if (healingAnimation != null && instantiatedHealingAnimation == null)
        {
            instantiatedHealingAnimation =
                Instantiate(healingAnimation, spawnPosition, playerTransform.rotation);
            instantiatedHealingAnimation.transform.parent = playerTransform;
        }

        Invoke(nameof(DestroyHealingAnimation), 1f);
    }

    public void OnLevelUp()
    {
        var playerTransform = transform;
        if (levelUpAnimation != null && playerManager != null)
        {
            gameAudioManager.OnLevelUpPlay();
            instantiatedLevelUpAnimation =
                Instantiate(levelUpAnimation, playerTransform.position, playerTransform.rotation);
            // Scale the instantiated game object
            instantiatedLevelUpAnimation.transform.localScale =
                new Vector3(3, 3, 3); // Set the scale factor as per your requirement
            instantiatedLevelUpAnimation.transform.parent = playerTransform;
        }

        Invoke(nameof(DestroyLevelUpAnimation), 2f);
    }

    private void DestroyLevelUpAnimation()
    {
        if (instantiatedLevelUpAnimation != null) Destroy(instantiatedLevelUpAnimation);
    }

    private void DestroyHealingAnimation()
    {
        if (instantiatedHealingAnimation != null) Destroy(instantiatedHealingAnimation);
    }

    public void SetSpeed(float speedToAdd)
    {
        speed += speedToAdd;
    }
}