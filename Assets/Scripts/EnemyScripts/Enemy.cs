using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Haptics;

public class Enemy : MonoBehaviour
{

    public LoseManager loseManager;
    //public EnemyAI enemyAI;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("You Lose!");

            //enemyAI = GetComponent<EnemyAI>();

            //LoseManager loseManager = FindObjectOfType<LoseManager>();

            if (loseManager != null)
            {
                //enemyAI.ResetEffects();
                //InputSystem.ResetHaptics();
                loseManager.ShowLoseScreen();
            }
        }
    }
}