using UnityEngine;

public class Battery : MonoBehaviour
{
    [SerializeField]
    private float batteryHealthRestore = 100f;

    public void ConsumeBattery()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.RestoreFlashlightHealth(batteryHealthRestore);
            Destroy(gameObject); // destroy battery after use
        }
    }

    private void OnTriggerEnter(Collider other) //collision
    {
        if (other.CompareTag("Player"))
        {
            ConsumeBattery();
        }
    }
}
