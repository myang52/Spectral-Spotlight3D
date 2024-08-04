using UnityEngine;

public class BatteryCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Battery"))
        {
            Battery battery = other.GetComponent<Battery>();
            if (battery != null)
            {
                battery.ConsumeBattery(); // Call method to consume battery
            }
        }
    }
}