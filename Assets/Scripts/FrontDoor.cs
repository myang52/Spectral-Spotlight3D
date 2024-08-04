
using UnityEngine;
using TMPro;




public class FrontDoor : MonoBehaviour
{


    [SerializeField]
    private TextMeshProUGUI promptText; // Reference to UI text displaying the prompt

    private bool playerNearby = false;
    private PlayerController playerController;

   private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            if (playerController != null && playerController.hasKey == false)
            {
                playerNearby = true;
                promptText.text = "You need the key. Look around for items!";
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
            if (playerController.hasKey == false)
            {
               
                promptText.text = "This is totally a yesy!";
                promptText.gameObject.SetActive(false); // Hide prompt after purchase
               
            }
            else
            {
                
            }
        }
    }
}