using UnityEngine;

public class Enemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Lose!");

            LoseManager loseManager = FindObjectOfType<LoseManager>();

            if (loseManager != null)
            {
                loseManager.ShowLoseScreen();
            }
        }
    }
}