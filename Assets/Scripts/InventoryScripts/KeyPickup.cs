using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    private bool playerInRange = false;
    private bool pickedUp = false;

    private PlayerInventory playerInventory;
    private PlayerControlScript playerControlScript;

    public TaskManager taskManager;
    public GameObject interactText;

    private void OnTriggerEnter(Collider other)
    {
        PlayerControlScript control = other.GetComponent<PlayerControlScript>();
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            playerInRange = true;
            playerInventory = inventory;
            playerControlScript = control;

            if (interactText != null)
                interactText.SetActive(true);

            Debug.Log("Press X to pick up the key.");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInventory>() != null)
        {
            playerInRange = false;
            playerInventory = null;
            playerControlScript = null;

            if (interactText != null)
                interactText.SetActive(false);
        }
    }

    private void Update()
    {
        if (!pickedUp && playerInRange && playerControlScript.actionAvailable)
        {
            pickedUp = true;

            playerInventory.hasKey = true;

            if (taskManager != null)
                taskManager.CompleteTask();

            if (interactText != null)
                interactText.SetActive(false);

            Debug.Log("Key picked up!");

            Destroy(gameObject);
        }
    }
}