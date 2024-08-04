using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    [SerializeField]
    public float playerSpeed = 5.0f; //originally set to private
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 7f;
    [SerializeField]
    private Transform flashlightTransform;
    [SerializeField]
    private GameObject beamObject;
    [SerializeField]
    public float maxBeamLength = 10f;
    [SerializeField]
    private Slider flashlightSlider;
    [SerializeField]
    private float flashlightDepletionRate = 20f;
    [SerializeField]
    private Light spotlight; //spotlight
    [SerializeField]
    private LayerMask ignoreLayers;
    [SerializeField]
    private Slider healthSlider;
    [SerializeField]
    private TextMeshProUGUI coinText; //UI text displaying  coin count
    [SerializeField]
    public bool hasKey = false; //win condition
    [SerializeField]
    private float animationSmoothTime = 0.1f;
    [SerializeField]
    private float animationPlayTransition = 0.15f;

    AudioManager audioManager;

    private CharacterController controller;
    private PlayerInput playerInput;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Transform cameraTransform;
        //controller actions
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction shootAction;
    private InputAction aimAction;
    public InputAction interactionAction;

    
    //aiming and animations
    private Animator animator;
    int jumpAnimation;
    int moveXAnimationParameterId;
    int moveZAnimationParameterId;

    Vector2 currentAnimationBlendVector;
    Vector2 animationVelocity;

    //UI tracker
    private bool isAiming;
    public float flashlightHealth = 100f;
    public float playerHealth = 100f; //was private
    public int coinCount = 0; // Track the number of coins the player has

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        cameraTransform = Camera.main.transform;

        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        shootAction = playerInput.actions["Shoot"];
        aimAction = playerInput.actions["Aim"];
        interactionAction = playerInput.actions["Interact"];

        Cursor.lockState = CursorLockMode.Locked;

        aimAction.performed += _ => StartAiming();
        aimAction.canceled += _ => StopAiming();

        //animation related
        animator = GetComponent<Animator>();
        jumpAnimation = Animator.StringToHash("Pistol Jump");
        moveXAnimationParameterId = Animator.StringToHash("MoveX");
        moveZAnimationParameterId = Animator.StringToHash("MoveZ");

        
        
    }

    private void OnEnable()
    {
        //shootAction.performed += _ => ShootGun();
        aimAction.Enable();
        interactionAction.Enable();
    }

    private void OnDisable()
    {
        //shootAction.performed -= _ => ShootGun();
        aimAction.Disable();
        interactionAction.Disable();
    }

    private void StartAiming()
    {
        if (flashlightHealth > 0)
        {
            audioManager.PlaySFX(audioManager.click); //play click sound effect
            isAiming = true;
            beamObject.SetActive(true);
            spotlight.enabled = true; // Enable Spotlight when aiming
        }
    }

    private void StopAiming()
    {
        isAiming = false;
        beamObject.SetActive(false);
        spotlight.enabled = false; // Disable Spotlight when not aiming
    }

  

    public void Interact()
    {
        // placeholder
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        Vector2 input = moveAction.ReadValue<Vector2>();
        currentAnimationBlendVector = Vector2.SmoothDamp(currentAnimationBlendVector, input, ref animationVelocity, animationSmoothTime); //animation
        Vector3 move = new Vector3(currentAnimationBlendVector.x, 0, currentAnimationBlendVector.y);
        move = move.x * cameraTransform.right.normalized + move.z * cameraTransform.forward.normalized;
        move.y = 0f;
        controller.Move(move * Time.deltaTime * playerSpeed);

        //animations
        animator.SetFloat(moveXAnimationParameterId, currentAnimationBlendVector.x);
        animator.SetFloat(moveZAnimationParameterId, currentAnimationBlendVector.y);

        if (jumpAction.triggered && groundedPlayer)
        {   
            audioManager.PlaySFX(audioManager.jump); //jump sound effect.
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.CrossFade(jumpAnimation, animationPlayTransition);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        float targetAngle = cameraTransform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (isAiming)
        {
            flashlightHealth -= flashlightDepletionRate * Time.deltaTime;
            flashlightHealth = Mathf.Clamp(flashlightHealth, 0, 100f);
            flashlightSlider.value = flashlightHealth / 100f; //canvas slider flashlight

            if (flashlightHealth <= 0)
            {
                StopAiming();
                return;
            }

            UpdateBeam();
        }

        if (interactionAction.triggered)
        {
            Interact();
        }

        if (hasKey == true){

            coinText.text = "You have the key, escape to the front!";
        }
    }

    private void UpdateBeam() //tracks beam
    {
        Vector3 beamDirection = cameraTransform.forward;
        float beamLength = maxBeamLength;

        RaycastHit[] hits = Physics.SphereCastAll(flashlightTransform.position, 0.1f, beamDirection, maxBeamLength, ~ignoreLayers);

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                audioManager.PlaySFX(audioManager.ghostDeath); //ghost death snd effect
                Enemy enemy = hit.collider.GetComponent<Enemy>();
                if (enemy != null)
                {
                  if (Random.value < 0.75f)
                    {
                        enemy.DestroyAndSpawnCoin(); // 70% chance
                    }
                    else if (Random.value < 0.85f)
                    {
                        enemy.DestroyAndSpawnBattery(); // 15% chance
                    }
                    else if (Random.value < 1f)
                    {
                        enemy.DestroyAndSpawnCandy(); // 15% chance
                    }
                }
                return;
            }
        }

        beamObject.transform.position = flashlightTransform.position + beamDirection * beamLength / 2;
        beamObject.transform.rotation = Quaternion.LookRotation(beamDirection);
        beamObject.transform.localScale = new Vector3(beamObject.transform.localScale.x, beamObject.transform.localScale.y, beamLength);

        spotlight.transform.position = flashlightTransform.position;
        spotlight.transform.rotation = Quaternion.LookRotation(beamDirection);
    }

    public void RestoreFlashlightHealth(float amount)
    {
        flashlightHealth = Mathf.Clamp(flashlightHealth + amount, 0, 100f);
        flashlightSlider.value = flashlightHealth / 100f;
    }

  public void RemovePlayerHealth(float dmg)
{
    playerHealth = Mathf.Clamp(playerHealth - dmg, 0, 100f);
    healthSlider.value = playerHealth / 100f;

    if (playerHealth <= 0)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("GameOver");
        
    }
}

    public void RestorePlayerHealth(float hp)
    {
        playerHealth = Mathf.Clamp(playerHealth + hp, 0, 100f);
        healthSlider.value = playerHealth / 100f;
    }

 public bool SpendCoins(int amount) //subtracts coins
    {
        if (coinCount >= amount)
        {
            coinCount -= amount;
            coinText.text = "Coins: " + coinCount;
            return true;
        }
        return false;
    }



    public void AddCoin()
    {
        audioManager.PlaySFX(audioManager.coin); //coin sound eff
        coinCount++;
        coinText.text = "Coins: " + coinCount;
    }

   
    private void OnTriggerEnter(Collider other) //collision with other objects using tags
    {
        if (other.CompareTag("Battery"))
        {   audioManager.PlaySFX(audioManager.battery); //restore battery
            RestoreFlashlightHealth(100f);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Enemy"))
        {   audioManager.PlaySFX(audioManager.damage); //collision with enemy snd eff
            audioManager.PlaySFX(audioManager.ghostDeath);
            RemovePlayerHealth(25f);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Candy"))
        {
            audioManager.PlaySFX(audioManager.regainhealth); //restore health snd eff
            RestorePlayerHealth(100f);
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Coin"))
        {
            AddCoin(); //already added snd eff
            Destroy(other.gameObject);
        }

         if (other.CompareTag("Escape"))
        {
            if (hasKey == true){
                Destroy(other.gameObject);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene("WinScreen");
            }
           
        }
    }
}
