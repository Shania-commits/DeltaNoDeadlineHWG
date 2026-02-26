using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private bool playerInRange = false;   // Track if player is inside trigger
    private PlayerInventory playerInventory;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            playerInRange = true;
            playerInventory = inventory;
            Debug.Log("Press E to pick up the key.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Player left the trigger
        if (other.GetComponent<PlayerInventory>() != null)
        {
            playerInRange = false;
            playerInventory = null;
        }
    }

    private void Update()
    {
        // Only pick up if player is in range and presses E
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerInventory.hasKey = true;
            Debug.Log("Key picked up!");
            Destroy(gameObject);
        }
    }
}