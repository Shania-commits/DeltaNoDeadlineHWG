using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    private bool isOpening = false;
    private bool playerInRange = false;
    private PlayerInventory currentInventory;

    private void OnTriggerEnter(Collider other)
    {
        PlayerInventory inventory = other.GetComponent<PlayerInventory>();

        if (inventory != null)
        {
            playerInRange = true;
            currentInventory = inventory;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerInventory>() != null)
        {
            playerInRange = false;
            currentInventory = null;
        }
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.X))
        {
            if (currentInventory.hasKey)
            {
                Debug.Log("Door unlocked!");
                isOpening = true;
            }
            else
            {
                Debug.Log("Door is locked. Find the Crowbar.");
            }
        }

        if (isOpening)
        {
            Debug.Log("You Win!");
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            //SceneManager.LoadScene("WinScene"); For when we add a win scene
        }
    }
}