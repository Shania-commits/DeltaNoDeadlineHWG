using UnityEngine;
using UnityEngine.InputSystem;

public class KeyPickup : MonoBehaviour
{
    private bool playerInRange = false;   // Track if player is inside trigger
    private PlayerInventory playerInventory;
    private PlayerControlScript playerControlScript;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        PlayerControlScript control = other.GetComponent<PlayerControlScript>();
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();
        if (inventory != null)
        {
            playerInRange = true;
            playerInventory = inventory;
            playerControlScript = control;
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
            playerControlScript = null;
        }
    }

    private void Update()
    {
        // Only pick up if player is in range and presses E

        if (playerInRange && playerControlScript.actionAvailable)
        {
            playerInventory.hasKey = true;
            Debug.Log("Key picked up!");
            Destroy(gameObject);
        }
    }
}