using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            inventory.hasKey = true;
            Debug.Log("Crowbar picked up!");
            Destroy(gameObject);
        }
    }
}