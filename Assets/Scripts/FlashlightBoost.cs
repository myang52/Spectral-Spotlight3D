
using UnityEngine;
using TMPro;

public class FlashlightBoost : MonoBehaviour
{
    [SerializeField]
    private int cost = 5; // Cost in coins
    [SerializeField]
    private TextMeshProUGUI promptText; // Reference to UI text displaying the prompt
    [SerializeField]
    private Light spotlight; // Reference to the spotlight
     AudioManager audioManager; //audio

     

    private bool playerNearby = false;
    private PlayerController playerController;


private void Awake(){

    audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
}


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerNearby = true;
                promptText.text = "Beam Length Boost: " + cost + " coins. Press E to buy.";
                promptText.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;
            promptText.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerNearby && playerController != null && playerController.interactionAction.triggered)
        {
            if (playerController.SpendCoins(cost))
            {
                audioManager.PlaySFX(audioManager.upgrade);
                playerController.maxBeamLength *= 2;
                spotlight.intensity = 25;
                spotlight.range = 14;
                promptText.text = "Beam length increased!";
                promptText.gameObject.SetActive(false); // Hide prompt after purchase
                Destroy(gameObject); // Remove the FlashlightBeamBoost item from the scene
            }
            else
            {
                promptText.text = "Not enough coins!";
                 audioManager.PlaySFX(audioManager.tooPoor);
            }
        }
    }
}