using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Candy : MonoBehaviour
{
  [SerializeField]
    private float playerHealthRestore = 100f;

    public void ConsumeCandy()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.RestorePlayerHealth(playerHealthRestore);
            Destroy(gameObject); // Destroy the battery after use
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ConsumeCandy();
        }
    }
}
